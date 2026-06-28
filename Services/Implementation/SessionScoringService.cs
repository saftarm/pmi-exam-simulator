using NuGet.Packaging;
using StackExchange.Redis;
using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Models.Session;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation;

public class SessionScoringService : ISessionScoringService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ILogger<SessionScoringService> _logger;

    public SessionScoringService(
        IQuestionRepository questionRepository,
        ILogger<SessionScoringService> logger)
    {
        _questionRepository = questionRepository;
        _logger = logger;
    }

    public async Task<Result<SessionCalculationResult>> ValidateAndScoreAsync(
        SessionSnapshot snapshot,
        Guid sessionId,
        IEnumerable<UserExamResponseDto> responses,
        CancellationToken ct = default)
    {

        // validation


        var submittedQuestionIds = responses.Select(q => q.QuestionId).AsEnumerable();
        var sessionQuestionIds = snapshot.Questions.Select(e => e.QuestionId).AsEnumerable();

           var questionTypeByQuestionId = snapshot.Questions
        .ToDictionary( q => q.QuestionId, q => q.QuestionType);

        var selectedOptionIdsByQuestionIds = responses.ToDictionary(
            response => response.QuestionId,
            response => response.SelectedOptionIds
        );

        var allowedOptionIdsByQuestionId = await _questionRepository.QueryOptionIdsInGroupByQuestionId(sessionQuestionIds);


        var validationContext = new SessionResponseValidationContext {
            SubmittedQuestionIds =  submittedQuestionIds,
            SessionQuestionIds = sessionQuestionIds,
            SelectedOptionIdsByQuestionId = selectedOptionIdsByQuestionIds,
            AllowedOptionIdsByQuestionId = allowedOptionIdsByQuestionId,
            QuestionTypeByQuestionId = questionTypeByQuestionId
            };


        var areResponsesValid = ValidateSubmittedResponses(validationContext);
        if(!areResponsesValid){
            _logger.LogInformation("Session responses validation hasfailed");
            return Result<SessionCalculationResult>.Failure(Errors.InvalidSessionResponse);
        }

        // --------------------

     

        var domainIdBySelectedQuestionId = snapshot.Questions
        .Where(q => submittedQuestionIds
        .Contains(q.QuestionId))
        .ToDictionary(q => q.QuestionId, q => q.DomainId);
        

        var correctOptionIdsByQuestionId = await _questionRepository.QueryCorrectOptionsGroupedByQuestionIds(submittedQuestionIds);

        var calculationContext = new SessionResultCalculationContext {
            SessionId = snapshot.SessionId,
            TotalQuestions = snapshot.TotalQuestions,
            SubmittedOptionsIdsByQuestionId = selectedOptionIdsByQuestionIds,
            QuestionTypeByQuestionId = questionTypeByQuestionId,
            CorrectOptionIdsByQuestionId = correctOptionIdsByQuestionId,
            DomainIdByQuestionId = domainIdBySelectedQuestionId,

        };

        var calculationResult = BuildSessionCalculation(calculationContext);

        return Result<SessionCalculationResult>.Success(calculationResult);
        
       
    }
  

    private bool ValidateSubmittedResponses(SessionResponseValidationContext context
          )
        {

        HashSet<Guid> submittedQuestionIds = [.. context.SubmittedQuestionIds];
        HashSet<Guid> sessionQuestionIds = [.. context.SessionQuestionIds];

        if(!submittedQuestionIds.IsSubsetOf(sessionQuestionIds)){
            _logger.LogInformation("Responses contain question ids that does not belong to session");
            return false;
        }

        HashSet<Guid> selectedOptionIds = [];
        HashSet<Guid> allowedOptionIds = [];

        foreach(var questionId in context.SubmittedQuestionIds){

          
            selectedOptionIds.Clear();
            allowedOptionIds.Clear();
            var currentOptions = context.SelectedOptionIdsByQuestionId[questionId];

            
            selectedOptionIds.AddRange(currentOptions);
            var allowedOptionIdsForQuestion = context.AllowedOptionIdsByQuestionId[questionId];
            allowedOptionIds.AddRange(allowedOptionIdsForQuestion);

            if (selectedOptionIds.Count == 0)
{
    _logger.LogInformation("Submitted question has no selected options");
    return false;
}

            if(!selectedOptionIds.IsSubsetOf(allowedOptionIds)){
                _logger.LogInformation("One of responses contains option that does not belong to other question");
                return false;
            }

              if(context.QuestionTypeByQuestionId.TryGetValue(questionId, out var questionType)){
                if (questionType is QuestionType.SingleChoice or QuestionType.TrueFalse
    && selectedOptionIds.Count > 1) {
        _logger.LogInformation("Single choice question contains multiple submitted answer options");
        return false;
    }
            }

        }
        return true;

    }


    private static SessionCalculationResult BuildSessionCalculation(SessionResultCalculationContext context)
{
        var score = 0m;
        var sessionResponses = new List<UserExamResponse>();
        var statsByDomain = new Dictionary<Guid, (int CorrectCount, int TotalCount)>();
        foreach(var (questionId, correctSessionOptionIds) in context.CorrectOptionIdsByQuestionId) {     
            bool isCorrect = false;
    
            HashSet<Guid> correctOptions = [.. correctSessionOptionIds];
            
            if(context.SubmittedOptionsIdsByQuestionId.TryGetValue(questionId, out var currentSelectedOptions) 
            && context.QuestionTypeByQuestionId.TryGetValue(questionId, out var questionType)
            && context.DomainIdByQuestionId.TryGetValue(questionId, out var domainId)) {

                HashSet<Guid> selectedOptions = [.. currentSelectedOptions];

                if(questionType is QuestionType.MultipleChoice) {
                    var correctCount = selectedOptions.Count(id => correctSessionOptionIds.Contains(id));
                    if(selectedOptions.SetEquals(correctSessionOptionIds)){
                        isCorrect = true;
                    }

                    if(correctSessionOptionIds.Count == 0){
                        throw new DivideByZeroException();
                    }
                    score += (decimal)correctCount/correctSessionOptionIds.Count;

                    foreach(var option in selectedOptions){

                        if(correctSessionOptionIds.Contains(option)){
                            sessionResponses.Add (new UserExamResponse(
                            questionId: questionId,
                            domainId: domainId,
                            selectedOptionId: option,
                            examAttemptId: context.SessionId,
                            isCorrect: true
                        ));

                        
                        }

                        else {
                            sessionResponses.Add (new UserExamResponse(
                            questionId: questionId,
                            domainId: domainId,
                            selectedOptionId: option,
                            examAttemptId: context.SessionId,
                            isCorrect: false
                        ));

                        }

                    }
                }
                else {
                    if(selectedOptions.SetEquals(correctOptions)){
                        score += 1;
                        sessionResponses.Add (new UserExamResponse(
                            questionId: questionId,
                            domainId: domainId,
                            selectedOptionId: selectedOptions.FirstOrDefault(),
                            examAttemptId: context.SessionId,
                            isCorrect: true
                        ));
                        isCorrect = true;
                
                    }
                    else{
                         sessionResponses.Add (new UserExamResponse(
                            questionId: questionId,
                            domainId: domainId,
                            selectedOptionId: selectedOptions.FirstOrDefault(),
                            examAttemptId: context.SessionId,
                            isCorrect: false
                        ));

                    }

                }

            }

             if(context.DomainIdByQuestionId.TryGetValue(questionId, out var currentDomainId))
            {
                if(statsByDomain.TryGetValue(currentDomainId, out var currentStats)) {
                statsByDomain[currentDomainId] = ( 
                    currentStats.CorrectCount + (isCorrect ? 1 : 0),
                    currentStats.TotalCount + 1
                );

            } else {
                statsByDomain[currentDomainId] = (isCorrect ? 1 : 0, 1);
            }
        }
        }

        return new SessionCalculationResult {
            Result = new SessionResultDto {
                ScorePoints = Math.Round(score, 2),
                PercentageScore = score / context.TotalQuestions * 100
            },
            SavedResponses = sessionResponses,
            DomainStats = statsByDomain
        };
        }
}