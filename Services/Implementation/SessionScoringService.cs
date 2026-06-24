using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.Entities;
using TestAPI.Models.Session;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation;

public class SessionScoringService : ISessionScoringService
{
    private readonly IQuestionRepository _questionRepository;

    public SessionScoringService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Result<SessionCalculationResult>> ValidateAndScoreAsync(
        SessionSnapshot snapshot,
        Guid actingUserId,
        Guid sessionId,
        IEnumerable<UserExamResponseDto> responses,
        CancellationToken ct = default)
    {
        if (snapshot.SessionId != sessionId || snapshot.UserId != actingUserId)
        {
            return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
        }

        var snapshotByQuestionId = snapshot.Questions.ToDictionary(q => q.QuestionId);
        var responsesList = responses.ToList();
        var responsesByQuestionId = responsesList
            .GroupBy(r => r.QuestionId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var (questionId, questionResponses) in responsesByQuestionId)
        {
            if (!snapshotByQuestionId.TryGetValue(questionId, out var snapshotQuestion))
            {
                return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
            }

            var selectedOptionIds = questionResponses.Select(r => r.SelectedOptionId).ToList();
            if (selectedOptionIds.Distinct().Count() != selectedOptionIds.Count)
            {
                return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
            }

            if (IsSingleSelectType(snapshotQuestion.QuestionType) && selectedOptionIds.Count > 1)
            {
                return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
            }
        }

        var questionIds = snapshot.Questions.Select(q => q.QuestionId).ToList();
        var allOptions = (await _questionRepository.GetAnswerOptionsByQuestionIds(questionIds, ct)).ToList();
        var optionsByQuestionId = allOptions
            .GroupBy(o => o.QuestionId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var optionsById = allOptions.ToDictionary(o => o.Id);

        foreach (var response in responsesList)
        {
            if (!optionsById.TryGetValue(response.SelectedOptionId, out var option))
            {
                return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
            }

            if (option.QuestionId != response.QuestionId)
            {
                return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
            }
        }

        var savedResponses = new List<UserExamResponse>();
        var domainStats = new Dictionary<Guid, (decimal ScorePoints, int QuestionCount)>();
        decimal totalScorePoints = 0m;

        foreach (var snapshotQuestion in snapshot.Questions.OrderBy(q => q.OrderIndex))
        {
            var selectedOptionIds = responsesByQuestionId.TryGetValue(snapshotQuestion.QuestionId, out var questionResponses)
                ? questionResponses.Select(r => r.SelectedOptionId).ToHashSet()
                : [];

            var questionOptions = optionsByQuestionId.GetValueOrDefault(snapshotQuestion.QuestionId) ?? [];
            var correctOptionIds = questionOptions.Where(o => o.IsCorrect).Select(o => o.Id).ToHashSet();
            var domainId = questionOptions.FirstOrDefault()?.DomainId;

            var questionScore = CalculateQuestionScore(
                snapshotQuestion.QuestionType,
                correctOptionIds,
                selectedOptionIds);

            totalScorePoints += questionScore;

            if (domainId is Guid domainGuid && domainGuid != Guid.Empty)
            {
                if (domainStats.TryGetValue(domainGuid, out var existing))
                {
                    domainStats[domainGuid] = (existing.ScorePoints + questionScore, existing.QuestionCount + 1);
                }
                else
                {
                    domainStats[domainGuid] = (questionScore, 1);
                }
            }

            foreach (var selectedOptionId in selectedOptionIds)
            {
                var option = optionsById[selectedOptionId];
                savedResponses.Add(new UserExamResponse(
                    questionId: option.QuestionId,
                    domainId: option.DomainId,
                    selectedOptionId: option.Id,
                    examAttemptId: sessionId,
                    isCorrect: option.IsCorrect));
            }
        }

        var percentageScore = snapshot.TotalQuestions == 0
            ? 0m
            : Math.Round(totalScorePoints / snapshot.TotalQuestions * 100m, 2);

        return Result<SessionCalculationResult>.Success(new SessionCalculationResult
        {
            Result = new SessionResultDto
            {
                ScorePoints = totalScorePoints,
                PercentageScore = percentageScore,
            },
            SavedResponses = savedResponses,
            DomainStats = domainStats,
        });
    }

    private static bool IsSingleSelectType(QuestionType questionType) =>
        questionType is QuestionType.SingleChoice or QuestionType.TrueFalse;

    private static decimal CalculateQuestionScore(
        QuestionType questionType,
        IReadOnlySet<Guid> correctOptionIds,
        IReadOnlySet<Guid> selectedOptionIds)
    {
        if (selectedOptionIds.Count == 0)
        {
            return 0m;
        }

        if (questionType == QuestionType.MultipleChoice)
        {
            if (correctOptionIds.Count == 0)
            {
                return 0m;
            }

            var correctSelected = selectedOptionIds.Count(correctOptionIds.Contains);
            return (decimal)correctSelected / correctOptionIds.Count;
        }

        if (selectedOptionIds.Count != 1)
        {
            return 0m;
        }

        return correctOptionIds.Contains(selectedOptionIds.First()) ? 1m : 0m;
    }
}
