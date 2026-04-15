
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class ExamAttemptService : IExamAttemptService
    {
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IUserExamResponseRepository _userExamResponseRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IProgressService _progressService;

        public ExamAttemptService(
            IExamAttemptRepository examAttemptRepository,
            IExamRepository examRepository,
            IQuestionRepository questionRepository,
            IProgressService progressService,
            IUserExamResponseRepository userExamResponseRepository
            )
        {
            _examAttemptRepository = examAttemptRepository;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _progressService = progressService;
            _userExamResponseRepository = userExamResponseRepository;
        }

        public async Task<ExamAttemptDto> GetByIdAsync(Guid examAttemptId)
        {

            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);
            if (examAttempt == null)
            {
                throw new Exception("Exam Attempt not found");
            }

            var examAttemptDto = new ExamAttemptDto
            {
                ExamTitle = examAttempt.ExamTitle,
                Score = examAttempt.Score,
                Status = examAttempt.Status
            };
            return examAttemptDto;
        }

        public async Task<ExamAttemptDto> GetByUserId(Guid userId)
        {
            var examAttempt = await _examAttemptRepository.GetByUserId(userId);

            var examAttemptDto = new ExamAttemptDto
            {

                Score = examAttempt.Score
            };
            return examAttemptDto;
        }

        public async Task<Guid> StartAttemptAsync(Guid userId, Guid examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);

            if (exam == null)
            {
                throw new RecordNotFoundException("Exam not found");
            }

            var examAttempt = new ExamAttempt
            {
                UserId = userId,
                ExamId = examId,
                StartedAt = DateTime.UtcNow,
                ExamTitle = exam.Title,
                Score = 0,
                UserExamResponses = new List<UserExamResponse>(),
                Status = AttemptStatus.InProgress,
                TotalQuesitons = exam.NumberOfQuestions
            };
            return await _examAttemptRepository.AddAsync(examAttempt);
        }

        public async Task FinishAttemptAsync(Guid examAttemptId)
        {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);
            if (examAttempt == null)
            {
                throw new RecordNotFoundException("ExamAttempt not found");
            }
            examAttempt.Status = AttemptStatus.Completed;
            examAttempt.SubmittedAt = DateTime.UtcNow;
            await CalculateScore(examAttempt);
            await _progressService.UpdateDomainPerformance(examAttempt);
        }

        public async Task<Guid> SaveResponse(Guid examAttemptId, Guid questionId, Guid domainId, Guid selectedOptionId)
        {
            var exists = await _examAttemptRepository.ExistsAsync(examAttemptId);

            if (!exists)
            {
                throw new RecordNotFoundException($"ExamAttempt {examAttemptId} not found");
            }
            var newResponse = new UserExamResponse
            {
                SelectedOptionId = selectedOptionId,
                QuestionId = questionId,
                DomainId = domainId,
                ExamAttemptId = examAttemptId
            };

            await _userExamResponseRepository.AddAsync(newResponse);
            return newResponse.Id;
        }

        public async Task CalculateScore(ExamAttempt examAttempt)
        {
            int correctCount = 0;
            var userExamResponses = examAttempt.UserExamResponses;

            var questions = await _examRepository.GetQuestionsByExamIdAsync(examAttempt.ExamId);

            foreach (var question in questions)
            {
                var userAnswer = userExamResponses.FirstOrDefault(u => u.QuestionId == question.Id);

                if (userAnswer == null)
                {
                    continue;
                }

                var isCorrect = question.AnswerOptions.Any(o => o.Id == userAnswer.SelectedOptionId && o.IsCorrect);

                if (isCorrect)
                {
                    correctCount++;
                    userAnswer.IsCorrect = true;
                }

            }

            examAttempt.CorrectCount = correctCount;
            examAttempt.Score = (int)Math.Round((double)correctCount / questions.Count() * 100);

            await _examAttemptRepository.UpdateAsync(examAttempt);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _examAttemptRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserExamResponseDto>> GetResponsesAsync(Guid examAttemptId)
        {
            var userExamResponses = await _examAttemptRepository.GetResponsesAsync(examAttemptId);
            var userExamResponseDtos = userExamResponses.Select(
                r => new UserExamResponseDto
                {
                    QuestionId = r.QuestionId,
                    SelectedOptionId = r.SelectedOptionId
                }
            );
            return userExamResponseDtos;

        }

        public async Task StartAttempt(Guid examId, Guid userId)
        {
            ExamAttempt newAttempt = new ExamAttempt
            {
                UserId = userId,
                ExamId = examId,
                Score = 0,
                StartedAt = DateTime.UtcNow,
                Status = AttemptStatus.InProgress
            };

            await _examAttemptRepository.AddAsync(newAttempt);

        }

    }
}

