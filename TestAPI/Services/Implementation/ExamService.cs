using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.Entities;
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

        public ExamService(IExamRepository examRepository, IQuestionRepository questionRepository, IExamAttemptRepository examAttemptRepository, IQuestionService questionService)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _examAttemptRepository = examAttemptRepository;
            _questionService = questionService;
        }

 
        public async Task CompileExam(CompileExamDto compileExamDto) {

            var exam = await _examRepository.GetByIdAsync(compileExamDto.ExamId);

            var questions = await _questionRepository.GetByIdsAsync(compileExamDto.QuestionIds);





            exam.Questions.AddRange(questions);

            await _examRepository.UpdateAsync(exam);


        }


        public async Task DeleteRangeAsync(ICollection<int> examIds) {
            await _examRepository.DeleteRangeAsync(examIds);
        }

        // Get Summary
        public async Task<IEnumerable<ExamSummaryDto>> GetSummaryAsync()
        {
            var exams = await _examRepository.GetAllAsync();
            if (exams == null)
            {
                throw new Exception("No exams found");
            }
            var examSummaryDtos = new List<ExamSummaryDto>();

            foreach (var exam in exams)
            {
                var examSummaryDto = new ExamSummaryDto
                {
                    Id = exam.Id,
                    Title = exam.Title,
                    CategoryId = exam.CategoryId ?? 0,

                    DurationInMinutes = exam.DurationInMinutes,
                    NumberOfQuestions = exam.NumberOfQuestions
                };
                examSummaryDtos.Add(examSummaryDto);
            }

            return examSummaryDtos;
        }

        public async Task<ExamDetailsDto> GetDetailsByIdAsync(int examId)
        {

            var questions = await _examRepository.GetQuestionsByExamIdAsync(examId);
            if(!questions.Any())
            {
                return null;
            }

            var examDetailsDto = new ExamDetailsDto
            {
                QuestionDtos = questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Title = q.Text,
                    AnswerOptionsDtos = q.AnswerOptions.Select(o => new AnswerOptionDto
                    {
                        Id = o.Id,
                        Text = o.Text,
                    }).ToList()
                }
                ).ToList()

            };

            if(examDetailsDto.QuestionDtos == null)
            {
                throw new Exception("Question DTOs are empty");
            }


            return examDetailsDto;
        }



        public async Task<ExamSummaryDto> GetByIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);

            if (exam == null)
            {
                throw new Exception("Exam not found");
            }
            var examSummaryDto = new ExamSummaryDto
            {
                Id = exam.Id,
                Title = exam.Title,
                DurationInMinutes = exam.DurationInMinutes,
                NumberOfQuestions = exam.NumberOfQuestions
            };
            return examSummaryDto;
        }

        public async Task CreateExam(CreateExamDto createExamDto)
        {

            if(createExamDto.createDomainDtos == null ) {
                throw new Exception("Domain Dtos not found");
            }
            var newExam = new Exam
            {
                Title = createExamDto.Title,
                DurationInMinutes = createExamDto.DurationInMinutes,
                PassScore = createExamDto.PassScore,
                Context = createExamDto.Context
            };

            newExam.Domains = createExamDto.createDomainDtos.Select(
                d => new Domain {
                    Title = d.Title,
                    Description = d.Description,
                    Weight = d.Weight}
            ).ToList();  
            
            await _examRepository.AddAsync(newExam);

        }

        //public async Task CompileExam(CompileExamDto compileExamDto)
        //{

        //    if (compileExamDto.QuestionIds == null)
        //    {
        //        throw new Exception("Question Ids are not defined");
        //    }
        //    if(compileExamDto.DomainDtos == null)
        //    {
        //        throw new Exception("Domain Dtos are not defined");
        //    }

        //    var newExam = new Exam
        //    {
        //        Title = compileExamDto.Title,
        //        CategoryId = compileExamDto.CategoryId,
        //        Context = compileExamDto.Context,
        //        DurationInMinutes = compileExamDto.DurationInMinutes
        //    };
            
        //    newExam.Questions.Add()



    
        //}

        public async Task AddQuestionsToExamAsync(int examId, ICollection<int> questionIds)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            var questions = await _questionRepository.GetByIdsAsync(questionIds);

        }




        public async Task<Exam?> GetExamAsync(int examId)
        {

            var exam = await _examRepository.GetByIdAsync(examId);
            return exam;
        }


        public async Task DeleteAsync(int examId)
        {

            await _examRepository.DeleteAsync(examId);
        }


        public async Task<int> CalculateScore(int examAttemptId)
        {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

            foreach (var answerOption in examAttempt.UserExamResponses)
            {
                if (answerOption.IsCorrect)
                {
                    examAttempt.Score += 10;
                }

            }
            return examAttempt.Score;
        }

    }
}
