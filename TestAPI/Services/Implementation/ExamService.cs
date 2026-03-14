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





        public ExamService(IExamRepository examRepository, IQuestionRepository questionRepository, IExamAttemptRepository examAttemptRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _examAttemptRepository = examAttemptRepository;

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

        public async Task CompileExam(CompileExamDto compileExamDto)
        {

            if (compileExamDto.QuestionIds == null)
            {
                throw new ArgumentException("Question Ids are not defined");
            }


            var numberOfQuestions = compileExamDto.QuestionIds.Count;

            var newExam = new Exam

            {
                Title = compileExamDto.Title,
                NumberOfQuestions = numberOfQuestions,
                DurationInMinutes = compileExamDto.DurationInMinutes,
                CategoryId = compileExamDto.CategoryId

            };


            for (int i = 0; i < numberOfQuestions; i++)
            {
                await _examRepository.AddQuestionToExamAsync(compileExamDto.QuestionIds[i], newExam);
            }

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
