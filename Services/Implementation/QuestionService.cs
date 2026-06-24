using TestAPI.DTO;
using TestAPI.DTO.AnswerOption;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Models;
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
        private readonly IDomainRepository _domainRepository;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(
            ILogger<QuestionService> logger,
            IQuestionRepository questionRepository,
            IDomainRepository domainRepository,
            IValidatorResolver validatorResolver,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _questionRepository = questionRepository;
            _domainRepository = domainRepository;
            _validatorResolver = validatorResolver;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> DeleteRangeAsync(IEnumerable<Guid> questionIds)
        {
            var ids = questionIds.ToList();
            if (ids.Count == 0)
            {
                return Result.Success();
            }

            var rowsAffected = await _questionRepository.DeleteRangeAsync(ids);
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning("{RowsAffected} questions have been deleted", rowsAffected);
            }
            return Result.Success();
        }

        public async Task<Result<QuestionAdminDto>> GetByIdAsync(Guid questionId, CancellationToken ct)
        {
            var question = await _questionRepository.GetByIdWithOptionsAsync(questionId, ct);

            if (question == null)
            {
                return Result<QuestionAdminDto>.Failure(Errors.QuestionNotFound);
            }

            return Result<QuestionAdminDto>.Success(MapToAdminDto(question));
        }

        public async Task<Result<PagedList<QuestionListItemDto>>> GetPagedAsync(
            QuestionQueryParameters query,
            CancellationToken ct)
        {
            var page = await _questionRepository.GetPagedAsync(query, ct);
            return Result<PagedList<QuestionListItemDto>>.Success(page);
        }

        public async Task<Result> CreateQuestionAsync(CreateQuestionDto createQuestionDto)
        {
            var validationResult = await _validatorResolver.ValidateAsync(createQuestionDto);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToError());
            }

            var domain = await _domainRepository.GetByIdAsync(createQuestionDto.DomainId);
            if (domain == null)
            {
                return Result.Failure(Errors.DomainNotFound);
            }

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
        {
          AnswerType = createQuestionDto.QuestionType == QuestionType.MultipleChoice
              ? AnswerType.MultipleChoice
              : AnswerType.SingleChoice,
        })]
            };

            await _questionRepository.AddAsync(newQuestion);
            await _unitOfWork.SaveChangesAsync();

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Question created with id {QuestionId}", newQuestion.Id);
            }

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(UpdateQuestionRequest request, CancellationToken ct = default)
        {
            var validationResult = await _validatorResolver.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToError());
            }

            var question = await _questionRepository.GetByIdWithOptionsAsync(request.Id, ct);
            if (question == null)
            {
                return Result.Failure(Errors.QuestionNotFound);
            }

            question.UpdateQuestion(
                title: request.Title!,
                explanation: request.Explanation!,
                questionType: request.QuestionType);

            SyncAnswerOptions(question, request.AnswerOptionsDtos);

            await _questionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync(ct);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Question {QuestionId} updated", request.Id);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteQuestionAsync(Guid questionId, CancellationToken ct = default)
        {
            if (!await _questionRepository.ExistsAsync(questionId, ct))
            {
                return Result.Failure(Errors.QuestionNotFound);
            }

            await _questionRepository.DeleteQuestionById(questionId);
            return Result.Success();
        }

        private static void SyncAnswerOptions(Question question, ICollection<UpdateAnswerOptionDto> dtos)
        {
            question.AnswerOptions ??= [];

            var incomingIds = dtos.Where(d => d.Id.HasValue).Select(d => d.Id!.Value).ToHashSet();
            var toRemove = question.AnswerOptions.Where(o => !incomingIds.Contains(o.Id)).ToList();
            foreach (var option in toRemove)
            {
                question.AnswerOptions.Remove(option);
            }

            foreach (var dto in dtos)
            {
                if (dto.Id.HasValue)
                {
                    var existing = question.AnswerOptions.FirstOrDefault(o => o.Id == dto.Id.Value);
                    if (existing != null)
                    {
                        existing.Text = dto.Text;
                        existing.IsCorrect = dto.IsCorrect;
                    }
                }
                else
                {
                    question.AnswerOptions.Add(new AnswerOption(dto.Text, dto.IsCorrect, question.DomainId));
                }
            }
        }

        private static QuestionAdminDto MapToAdminDto(Question question) => new()
        {
            Id = question.Id,
            Title = question.Title,
            Explanation = question.Explanation,
            QuestionType = question.QuestionType,
            DomainId = question.DomainId,
            DomainTitle = question.Domain?.Title ?? string.Empty,
            ExamTitle = question.Domain?.Exam?.Title ?? string.Empty,
            AnswerOptions = question.AnswerOptions?.Select(o => new AnswerOptionDto
            {
                Id = o.Id,
                Text = o.Text ?? string.Empty,
                IsCorrect = o.IsCorrect
            })
        };
    }
}
