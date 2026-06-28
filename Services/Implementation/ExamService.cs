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
using TestAPI.Validation;

namespace TestAPI.Services.Implementation
{
    public class ExamService : IExamService
    {
        private readonly ILogger<ExamService> _logger;
        private readonly IExamRepository _examRepository;
        private readonly IDomainRepository _domainRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUnitOfWork _unitOfWork;

        public ExamService(
            ILogger<ExamService> logger,
            IExamRepository examRepository,
            IDomainRepository domainRepository,
            ICategoryRepository categoryRepository,
            IQuestionRepository questionRepository,
            IExamAttemptRepository examAttemptRepository,
            IValidatorResolver validatorResolver,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _examRepository = examRepository;
            _domainRepository = domainRepository;
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _examAttemptRepository = examAttemptRepository;
            _validatorResolver = validatorResolver;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<QuestionSnapshotDto>>> CompileExam(Guid examId, CancellationToken ct = default)
        {
            var examWithDomains = await _examRepository.QueryExamsWithDomainsById(examId);
            if (examWithDomains is null)
            {
                _logger.LogWarning("Exam with ID: {ID} not found", examId);
                return Result<IReadOnlyList<QuestionSnapshotDto>>.Failure(Errors.RecordNotFoundById);
            }

            if (examWithDomains.Domains is null || !examWithDomains.Domains.Any())
            {
                _logger.LogWarning("Exam with ID: {ID} has no domains", examId);
                return Result<IReadOnlyList<QuestionSnapshotDto>>.Failure(Errors.RangeOfRecordsNotFound);
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
                return Result<IReadOnlyList<QuestionSnapshotDto>>.Failure(Errors.RangeOfRecordsNotFound);
            }

            var compiledQuestionDtos = examQuestions.Select(e => new QuestionSnapshotDto
            {
                Id = e.Id,
                Title = e.Title,
                DomainId = e.DomainId,
                QuestionType = e.QuestionType,
                AnswerOptionsDtos = [.. e.AnswerOptions!.Select(o => new AnswerOptionDto
        {
          Id = o.Id,
          Text = o.Text!
        })]
            }).ToList();

            _logger.LogDebug("Compiled {Count} questions for exam {ExamId}", compiledQuestionDtos.Count, examId);

            return Result<IReadOnlyList<QuestionSnapshotDto>>.Success(compiledQuestionDtos);
        }

        public async Task<Result> CreateExamAsync(CreateExamDto dto, CancellationToken ct)
        {
            var validationResult = await _validatorResolver.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToError());
            }

            var submittedDomainTitles = dto.CreateDomainDtos.Select(d => d.Title).AsEnumerable();
            var domainExistsByTitle = await _domainRepository.AnyExistsByTitleAsync(submittedDomainTitles);

            if(domainExistsByTitle) {
                return Result.Failure(new Error("CONFLICT", "Domain already exists with given title", ErrorType.Conflict));
            }
            
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
            await _examRepository.AddAsync(newExam);

            var rowsAffected = await _unitOfWork.SaveChangesAsync(ct);
            if (rowsAffected == 0)
            {
                return Result.Failure(Errors.SessionPersistenceFailed);
            }
            return Result.Success();
        }

        public async Task<Result> ArchiveAsync(Guid examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return Result.Failure(Errors.ExamNotFound);
            exam.ChangeStatus(ExamStatus.Archived);
            await _examRepository.UpdateAsync(exam);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<ExamSummaryDto>>> GetAllExamsAsync(CancellationToken ct = default)
        {
            var summaries = await _examRepository.GetAllExamSummariesAsync(ct);
            return Result<IReadOnlyList<ExamSummaryDto>>.Success(summaries);
        }

        public async Task<Result<ExamDetailsDto>> GetDetailsByIdAsync(Guid examId)
        {
            var examInDb = await _examRepository.GetByIdAsync(examId);
            if (examInDb == null)
            {
                _logger.LogWarning("Exam {ExamId} not found", examId);
                return Result<ExamDetailsDto>.Failure(Errors.ExamNotFound);
            }

            var attemptCounts = await _examAttemptRepository.GetCompletedAttemptCountsByExamAsync();
            var attemptCount = attemptCounts.GetValueOrDefault(examId);

            var examDetailsDto = new ExamDetailsDto
            {
                Id = examInDb.Id,
                Title = examInDb.Title,
                Context = examInDb.Context,
                NumberOfQuestions = examInDb.NumberOfQuestions,
                DurationInMinutes = examInDb.DurationInMinutes,
                AttemptCount = attemptCount,
                IsMostPopular = false,
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
                return Result<IEnumerable<ExamDetailsDto>>.Success([]);
            }

            var attemptCounts = await _examAttemptRepository.GetCompletedAttemptCountsByExamAsync();
            var maxAttempts = attemptCounts.Values.DefaultIfEmpty(0).Max();

            var examsDetailsDtos = paginatedExams.Select(
                e =>
                {
                    var count = attemptCounts.GetValueOrDefault(e.Id);
                    return new ExamDetailsDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Context = e.Context,
                        DurationInMinutes = e.DurationInMinutes,
                        NumberOfQuestions = e.NumberOfQuestions,
                        AttemptCount = count,
                        IsMostPopular = maxAttempts > 0 && count == maxAttempts,
                    };
                });
            return Result<IEnumerable<ExamDetailsDto>>.Success(examsDetailsDtos);
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateExamRequest request)
        {
            var exam = await _examRepository.GetByIdAsync(id);

            if (exam == null) return Result.Failure(Errors.ExamNotFound);

            exam.UpdateExamDetails(request);
            await _examRepository.UpdateAsync(exam);
            await _unitOfWork.SaveChangesAsync();

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
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<ExamOverviewStatsDto>>> GetExamOverviewStatsAsync(CancellationToken ct = default)
        {
            var stats = await _examAttemptRepository.GetOverviewStatsAsync(ct);
            return Result<IReadOnlyList<ExamOverviewStatsDto>>.Success(stats);
        }
    }
}
