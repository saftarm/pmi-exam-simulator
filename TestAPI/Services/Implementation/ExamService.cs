using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.DTO.Exam.Requests;
using TestAPI.DTO.Exam.Responses;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionService _questionService;
        private readonly IDomainRepository _domainRepository;
        private readonly IMapperService _mapperService;

        public ExamService(IExamRepository examRepository,
         IQuestionRepository questionRepository,
         IExamAttemptRepository examAttemptRepository,
         IQuestionService questionService,
         IDomainRepository domainRepository,
         IMapperService mapperService
         )
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _examAttemptRepository = examAttemptRepository;
            _questionService = questionService;
            _domainRepository = domainRepository;
            _mapperService = mapperService;
        }



        public async Task CompileExam(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);

            var domainIds = await _examRepository.GetDomainIdsById(id);

            foreach (var domainId in domainIds)
            {
                var weight = exam.Domains.FirstOrDefault(d => d.Id == domainId)!.Weight;

                var numberOfQuestions = (exam.NumberOfQuestions * weight) / 100;

                var decimalPart = numberOfQuestions - (int)numberOfQuestions;

                if (decimalPart >= 0.5)
                {
                    numberOfQuestions += (1 - decimalPart);
                }
                else
                {
                    numberOfQuestions -= decimalPart;
                }

                var questions = await _questionRepository.GetFixedAmountOfRandomQuestionsByDomainId(domainId, numberOfQuestions);


                exam.Questions.AddRange(questions);

            }

            await _examRepository.UpdateAsync(exam);
        }

        public async Task DeleteRangeAsync(ICollection<Guid> examIds)
        {
            await _examRepository.DeleteRangeAsync(examIds);
        }

        public async Task<ExamFullDto> GetByIdAsync(Guid examId)
        {
            ExamFullDto dto = new ExamFullDto();
            return dto;
        }

        // Get Summary
        public async Task<IEnumerable<ExamSummaryDto>> GetSummariesAsync(PageParameters pageParameters)
        {
            var examsQuery = _examRepository.GetAllAsync();
            var pagedExams = await PagedList<Exam>.CreateAsync(examsQuery, pageParameters.PageNumber, pageParameters.PageSize);
            var examSummaries = pagedExams.Items.Select(
                pe => new ExamSummaryDto
                {
                    Id = pe.Id,
                    Title = pe.Title,
                    CategoryTitle = pe.Category.Title,
                    CategoryId = pe.CategoryId,
                    DurationInMinutes = pe.DurationInMinutes,
                    NumberOfQuestions = pe.NumberOfQuestions
                }
            );
            return examSummaries;
        }

        public async Task<ExamSummaryDto> UpdateAsync(Guid id, UpdateExamRequest request)
        {
            var exam = await _examRepository.GetByIdAsync(id);

            if (exam == null)
            {
                throw new RecordNotFoundException("Exam not found");
            }

            exam.NumberOfQuestions = request.NumberOfQuestions;
            exam.DurationInMinutes = request.DurationInMinutes;


            await _examRepository.UpdateAsync(exam);

            return new ExamSummaryDto
            {
                Title = exam.Title,
                CategoryTitle = exam.CategoryTitle
            };
        }
        public async Task<IEnumerable<ExamDetailsDto>> GetDetailsAsync(PageParameters pageParameters)
        {
            var examsQuery = _examRepository.GetAllAsync();

            var pagedExams = await PagedList<Exam>.CreateAsync(examsQuery, pageParameters.PageNumber, pageParameters.PageSize);

            var examDetails = pagedExams.Items.Select(
                pe => new ExamDetailsDto
                {
                    QuestionDtos = pe.Questions.Select(
                        q => new QuestionDto
                        {
                            Id = q.Id,
                            Title = q.Title,
                            AnswerOptionsDtos = q.AnswerOptions.Select(
                                o => new AnswerOptionDto
                                {
                                    Text = o.Text
                                }
                            ).ToList()
                        }

                    ).ToList()
                }
            ).ToList();
            return examDetails;
        }


        public async Task<ExamSummaryDto> GetSummaryByIdAsync(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);

            if (exam == null)
            {
                throw new RecordNotFoundException("Exam is not found");
            }
            var examSummaryDto = new ExamSummaryDto
            {
                Id = exam.Id,
                Title = exam.Title,
                CategoryTitle = exam.Category.Title,
                CategoryId = exam.CategoryId,
                DurationInMinutes = exam.DurationInMinutes,
                NumberOfQuestions = exam.NumberOfQuestions
            };

            return examSummaryDto;
        }

        public async Task<ExamDetailsDto> GetDetailsByIdAsync(Guid id)
        {

            var examStatus = await _examRepository.GetExamStatusByIdAsync(id);
            if (examStatus == ExamStatus.Draft || examStatus == ExamStatus.Compiled)
            {
                throw new Exception("Exam is not published yet");
            }
            var questions = await _examRepository.GetQuestionsByExamIdAsync(id);

            var examDetailsDto = new ExamDetailsDto
            {
                QuestionDtos = questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    AnswerOptionsDtos = q.AnswerOptions.Select(o => new AnswerOptionDto
                    {
                        Id = o.Id,
                        Text = o.Text,
                    }).ToList()
                }
                ).ToList()
            };

            if (examDetailsDto.QuestionDtos == null)
            {
                throw new Exception("Question DTOs are empty");
            }


            return examDetailsDto;
        }

        public async Task<IEnumerable<CreateExamResponse>> CreateExams(List<CreateExamDto> dto)
        {

            var newExams = _mapperService.MapCreateExamDtosToExam(dto);
            await _examRepository.AddAsync(newExams);
            var createdExams = _mapperService.MapNewExamsToCreateExamResponse(newExams);
            return createdExams;

        }

        public async Task AddQuestionsToExamAsync(Guid examId, ICollection<Guid> questionIds)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            var questions = await _questionRepository.GetByIdsAsync(questionIds);

        }

        public async Task<Exam?> GetExamAsync(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);

            if (exam == null)
            {
                throw new RecordNotFoundException($"Exam not found by id = {id}");
            }
            return exam;
        }


        public async Task DeleteAsync(Guid examId)
        {
            await _examRepository.DeleteAsync(examId);
        }


        public async Task<int> CalculateScore(Guid examAttemptId)
        {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

            if(examAttempt == null) {
                throw new RecordNotFoundException("Exam attempt nout found by Id");
            }

            foreach (var answerOption in examAttempt.UserExamResponses)
            {
                if (answerOption.IsCorrect)
                {
                    examAttempt.Score += 10;
                }
            }
            return examAttempt.Score;
        }

        public async Task PublihExam(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);

            exam.Status = ExamStatus.Published;

            await _examRepository.UpdateAsync(exam);
        }
        public async Task<IEnumerable<ExamSummaryDto>> GetPublishedExamSummariesByCategoryId(Guid categoryId, PageParameters pageParameters, CancellationToken ct)
        {
            var exams = await _examRepository.GetPublishedExamsByCategoryIdAsync(categoryId, pageParameters, ct);

            if (exams == null)
            {
                throw new RecordNotFoundException($"No Exams found by CategoryId = {categoryId} or no Exams were published");
            }

            return _mapperService.MapExamsToExamSummaryDtos(exams);
        }


    }
}
