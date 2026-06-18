using TestAPI.DTO.AnswerOption;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation
{
  public class QuestionService : IQuestionService
  {
    private readonly ILogger<QuestionService> _logger;
    private readonly IQuestionRepository _questionRepository;
    private readonly IValidatorResolver _validatorResolver;
    public QuestionService(
    ILogger<QuestionService> logger,
    IQuestionRepository questionRepository,
    IValidatorResolver validatorResolver
    )
    {
      _logger = logger;
      _questionRepository = questionRepository;
      _validatorResolver = validatorResolver;
    }


    // ------------------------------------------------ Admin side logic ----------------------------------------------

    public async Task<Result> DeleteRangeAsync(IEnumerable<Guid> questionIds)
    {
      var rowsAffected = await _questionRepository.DeleteRangeAsync(questionIds);
      if (_logger.IsEnabled(LogLevel.Warning))
      {
        _logger.LogWarning("{RowsAffected} questions have been deleted", rowsAffected);
      }
      return Result.Success();
    }

    // Get Question By Id
    public async Task<QuestionDto> GetByIdAsync(Guid questionId)
    {

      var question = await _questionRepository.GetByIdAsync(questionId);

      if (question == null)
      {
        throw new KeyNotFoundException("Question not found");
      }

      var questionDto = new QuestionDto
      {
        Id = question.Id,
        Title = question.Title,
        AnswerOptionsDtos = [.. question.AnswerOptions!.Select(o => new AnswerOptionDto {
              Id = o.Id,
              Text = o.Text!,
              })]
      };
      return questionDto;
    }

    public async Task<Result> CreateQuestionAsync(CreateQuestionDto createQuestionDto)
    {
      // validation
      var newQuestion = new Question(
          title: createQuestionDto.Title!,
          explanation: createQuestionDto.Explanation!,
          questionType: createQuestionDto.QuestionType,
          domainId: createQuestionDto.DomainId)
      {
        AnswerOptions = [.. createQuestionDto.AnswerOptionsDtos.Select(o => new AnswerOption(
              text: o.Text,
              isCorrect: o.IsCorrect,
              domainId: createQuestionDto.DomainId)
              )]
      };

      var rowsAffected = await _questionRepository.AddAsync(newQuestion);

      if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("{RowsAffected} questions are created", rowsAffected);
      }
      return Result.Success();
    }

    // Update Question
    public async Task<Result> UpdateAsync(UpdateQuestionRequest request)
    {
      var validationResult = await _validatorResolver.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
        return Result.Failure(Errors.ValidationFailed);
      }

      var question = await _questionRepository.GetByIdAsync(request.Id);

      question.UpdateQuestion(
          title: request.Title!,
          explanation: request.Explanation!,
          questionType: request.QuestionType);


      var rowsAffected = await _questionRepository.UpdateAsync(question);
      if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("{RowsAffected} number of rows changed while updating 1 question", rowsAffected);
      }

      return Result.Success();

    }

    // Delete Question By Id
    public async Task DeleteQuestionAsync(Guid questionId) {
      await _questionRepository.DeleteQuestionById(questionId);
    }


  }
}
