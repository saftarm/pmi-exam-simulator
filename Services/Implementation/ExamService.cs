using TestAPI.DTO;
using TestAPI.DTO.AnswerOption;
using TestAPI.DTO.Exam;
using TestAPI.DTO.Exam.Requests;
using TestAPI.DTO.ExamAttempt;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models.Pagination;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
  public class ExamService : IExamService
  {
    private readonly ILogger<ExamService> _logger;
    private readonly IExamRepository _examRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IExamAttemptRepository _examAttemptRepository;

    public ExamService(
        ILogger<ExamService> logger,
        IExamRepository examRepository,
        ICategoryRepository categoryRepository,
        IQuestionRepository questionRepository,
        IExamAttemptRepository examAttemptRepository)
    {
      _logger = logger;
      _examRepository = examRepository;
      _categoryRepository = categoryRepository;
      _questionRepository = questionRepository;
      _examAttemptRepository = examAttemptRepository;
    }

    public async Task<Result<IEnumerable<QuestionDto>>> CompileExam(Guid sessionId, Guid examId)
    {
      var examWithDomains = await _examRepository.QueryExamsWithDomainsById(examId);
      if (examWithDomains is null)
      {
        _logger.LogWarning("Exam with ID: {ID} not found", examId);
        return Result<IEnumerable<QuestionDto>>.Failure(Errors.RecordNotFoundById);
      }

      if (examWithDomains.Domains is null || !examWithDomains.Domains.Any())
      {
        _logger.LogWarning("Exam with ID: {ID} has no domains", examId);
        return Result<IEnumerable<QuestionDto>>.Failure(Errors.RangeOfRecordsNotFound);
      }

      Dictionary<Guid, int> numberOfQuestionsPerDomain = [];

      foreach (var domain in examWithDomains.Domains)
      {
        int currentNumberOfQuestions = (int)Math.Round(
            domain.Weight / 100.0 * examWithDomains.NumberOfQuestions,
            MidpointRounding.AwayFromZero);

        numberOfQuestionsPerDomain[domain.Id] = currentNumberOfQuestions;
      }

      var examQuestions = await _questionRepository.QueryQuestionsWithAnswerOptions(numberOfQuestionsPerDomain);

      if (!examQuestions.Any())
      {
        _logger.LogWarning("No questions found for exam {ExamId}", examId);
        return Result<IEnumerable<QuestionDto>>.Failure(Errors.RangeOfRecordsNotFound);
      }

      var compiledQuestionDtos = examQuestions.Select(e => new QuestionDto
      {
        Id = e.Id,
        Title = e.Title,
        AnswerOptionsDtos = [.. e.AnswerOptions!.Select(o => new AnswerOptionDto
        {
          Id = o.Id,
          Text = o.Text!
        })]
      }).ToList();

      _logger.LogDebug(
          "Compiled {Count} questions for session {SessionId}",
          compiledQuestionDtos.Count,
          sessionId);

      return Result<IEnumerable<QuestionDto>>.Success(compiledQuestionDtos);
    }

    public async Task<Result<SessionCalculationResult>> CalculateSessionResult(
        int totalQuestionsInSession,
        Guid sessionId,
        IEnumerable<UserExamResponseDto> responses)
    {
      if (totalQuestionsInSession == 0)
      {
        return Result<SessionCalculationResult>.Failure(Errors.ExamNotFound);
      }

      var optionsIds = responses.Select(r => r.SelectedOptionId).ToList();
      var optionsInDb = await _questionRepository.GetAnswerOptionsByIds(optionsIds);

      var newResponses = new List<UserExamResponse>();

      foreach (var option in optionsInDb)
      {
        newResponses.Add(new UserExamResponse(
            selectedOptionId: option.Id,
            questionId: option.QuestionId,
            domainId: option.DomainId,
            examAttemptId: sessionId,
            isCorrect: option.IsCorrect));
      }

      await _examAttemptRepository.SaveSessionResponses(newResponses);

      int correctCount = newResponses.Count(r => r.IsCorrect);
      decimal percentage = (decimal)correctCount / totalQuestionsInSession * 100;

      return Result<SessionCalculationResult>.Success(new SessionCalculationResult
      {
        Result = new SessionResultDto
        {
          CorrectCount = correctCount,
          PercentageScore = percentage
        },
        SavedResponses = newResponses
      });
    }

    public async Task<Result> CreateExamAsync(CreateExamDto dto)
    {
      var doesExist = await _examRepository.ExamExistsByTitleAsync(dto.Title);
      if (doesExist)
      {
        return Result.Failure(Errors.ExamAlreadyExists);
      }

      var categoryTitle = await _categoryRepository.GetByIdAsync(dto.CategoryId);
      if (categoryTitle == null)
      {
        return Result.Failure(Errors.RecordNotFoundById);
      }

      var newExam = new Exam(
          categoryId: dto.CategoryId,
          title: dto.Title,
          context: dto.Context,
          durationInMinutes: dto.DurationInMinutes,
          numberOfQuestions: dto.NumberOfQuestions)
      {
        Domains = [.. dto.CreateDomainDtos.Select(d => new Domain(
              title: d.Title,
              description: d.Description,
              weight: d.Weight))]
      };
      var rowsAffected = await _examRepository.AddAsync(newExam);

      if (rowsAffected == 0)
      {
        throw new Exception("Database rows were not affected");
      }
      return Result.Success();
    }

    public async Task<Result> ArchiveAsync(Guid examId)
    {
      var exam = await _examRepository.GetByIdAsync(examId);
      if (exam == null) return Result.Failure(Errors.ExamNotFound);
      exam.ChangeStatus(ExamStatus.Archived);
      await _examRepository.UpdateAsync(exam);
      return Result.Success();
    }

    public async Task<IEnumerable<Exam>> GetAllExams()
    {
      return await _examRepository.GetAllExams();
    }

    public async Task<Result<ExamDetailsDto>> GetDetailsByIdAsync(Guid examId)
    {
      var examInDb = await _examRepository.GetByIdAsync(examId);
      if (examInDb == null)
      {
        _logger.LogWarning("Exam {ExamId} not found", examId);
        return Result<ExamDetailsDto>.Failure(Errors.ExamNotFound);
      }

      var examDetailsDto = new ExamDetailsDto
      {
        Title = examInDb.Title,
        Context = examInDb.Context,
        NumberOfQuestions = examInDb.NumberOfQuestions,
        DurationInMinutes = examInDb.DurationInMinutes
      };
      return Result<ExamDetailsDto>.Success(examDetailsDto);
    }

    public async Task DeleteRangeAsync(IEnumerable<Guid> examIds)
    {
      await _examRepository.DeleteRangeAsync(examIds);
    }

    public async Task<Result<IEnumerable<ExamDetailsDto>>> GetPublishedExamsDetailsAsync(PageParameters pageParameters)
    {
      var paginatedExams = await _examRepository.GetPublishedPaginatedExamsAsync(pageParameters);

      if (!paginatedExams.Any())
      {
        _logger.LogError("No published exams found in database");
        return Result<IEnumerable<ExamDetailsDto>>.Failure(Errors.RangeOfRecordsNotFound);
      }

      var examsDetailsDtos = paginatedExams.Select(
          e => new ExamDetailsDto
          {
            Id = e.Id,
            Title = e.Title,
            Context = e.Context,
            DurationInMinutes = e.DurationInMinutes,
            NumberOfQuestions = e.NumberOfQuestions
          });
      return Result<IEnumerable<ExamDetailsDto>>.Success(examsDetailsDtos);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateExamRequest request)
    {
      var exam = await _examRepository.GetByIdAsync(id);

      if (exam == null) return Result.Failure(Errors.ExamNotFound);

      exam.UpdateExamDetails(request);
      await _examRepository.UpdateAsync(exam);

      return Result.Success();
    }

    public async Task DeleteAsync(Guid examId)
    {
      await _examRepository.DeleteAsync(examId);
    }

    public async Task<Result> PublishExam(Guid id)
    {
      var exam = await _examRepository.GetByIdAsync(id);

      if (exam is null)
      {
        return Result.Failure(Errors.ExamNotFound);
      }
      exam.ChangeStatus(ExamStatus.Published);
      await _examRepository.UpdateAsync(exam);

      return Result.Success();
    }
  }
}
