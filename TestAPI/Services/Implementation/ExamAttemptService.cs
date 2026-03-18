
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;
using TestAPI.Enums;


namespace TestAPI.Services.Implementation
{
    public class ExamAttemptService : IExamAttemptService
    {
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IExamRepository _examRepository;

        private readonly IQuestionRepository _questionRepository;

        public ExamAttemptService (
            IExamAttemptRepository examAttemptRepository,
            IExamRepository examRepository,
            IQuestionRepository questionRepository
            ) 
            {
            _examAttemptRepository = examAttemptRepository;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
           }

        public async Task<ExamAttemptDto> GetByIdAsync(int examAttemptId) {

            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);
            if(examAttempt == null) {
                throw new Exception("Exam Attempt not found");
            }

            var examAttemptDto = new ExamAttemptDto {
                ExamTitle = examAttempt.ExamTitle,
                Score = examAttempt.Score,
                Status = examAttempt.Status
            };
            return examAttemptDto;
        }
        
        public async Task<ExamAttemptDto> GetByUserId (int userId) {
            var examAttempt = await _examAttemptRepository.GetByUserId(userId);

            var examAttemptDto = new ExamAttemptDto {

                Score = examAttempt.Score
            };
            return examAttemptDto;
        }

        public async Task<int> StartAttemptAsync(int userId, int examId) 
        {
            var exam = await _examRepository.GetByIdAsync(examId);

            if(exam == null) {
                throw new KeyNotFoundException("Exam not found");
            }

            var examAttempt = new ExamAttempt {
                UserId = userId,
                ExamId = examId,  
                StartedAt = DateTime.UtcNow,
                ExamTitle = exam.Title,
                Score = 0,
                UserExamResponses = new List<UserExamResponse>(),
                Status = ExamStatus.InProgress
            };
            return await _examAttemptRepository.AddAsync(examAttempt);

        }


         public async Task FinishAttemptAsync(int examAttemptId) {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);
            examAttempt.Status = ExamStatus.Completed;
            examAttempt.SubmittedAt = DateTime.UtcNow;
            await CalculateScore(examAttempt);
            
            

         }

        public async Task<int> SaveResponse (int examAttemptId, int questionId, int selectedOptionId) {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

            if(examAttempt == null)
            {
                throw new Exception($"ExamAttempt {examAttemptId} not found");
            }


            var newResponse = new UserExamResponse {
                SelectedOptionId = selectedOptionId,
                QuestionId = questionId,
                ExamAttemptId = examAttemptId
            };
            if(examAttempt.UserExamResponses == null) {
                examAttempt.UserExamResponses = new List<UserExamResponse>();
            }
            examAttempt.UserExamResponses.Add(newResponse);
            await _examAttemptRepository.UpdateAsync(examAttempt);
            return examAttempt.Id;

        }

        public async Task CalculateScore(ExamAttempt examAttempt) 
        {

            int correctCount = 0;
            var userExamResponses = examAttempt.UserExamResponses;

            var questions = await _examRepository.GetQuestionsByExamIdAsync(examAttempt.ExamId);

            foreach (var question in questions) {
                var userAnswer = userExamResponses.FirstOrDefault(u => u.QuestionId == question.Id);

                if(userAnswer == null) {
                    continue;
                }

                var isCorrect = question.AnswerOptions.Any(o => o.Id == userAnswer.SelectedOptionId && o.IsCorrect);

                if(isCorrect) {
                    correctCount ++;
                }

            }

            examAttempt.CorrectCount = correctCount;
            examAttempt.Score = (int)Math.Round((double)correctCount /questions.Count() * 100);

            await _examAttemptRepository.UpdateAsync(examAttempt);
                
            }


            

        public async Task DeleteAsync(int id) {
            await _examAttemptRepository.DeleteAsync(id);
        }
             
            
        

            
        

        public async Task<IEnumerable<UserExamResponseDto>> GetResponsesAsync(int examAttemptId){
            var userExamResponses =  await _examAttemptRepository.GetResponsesAsync(examAttemptId);
            var userExamResponseDtos = userExamResponses.Select(
                r => new UserExamResponseDto {
                    QuestionId = r.QuestionId,
                    SelectedOptionId = r.SelectedOptionId
                }
            );
            return userExamResponseDtos;

        }

        public async Task StartAttempt(int examId, int userId) {
            ExamAttempt newAttempt = new ExamAttempt {
                UserId = userId,
                ExamId = examId,
                Score = 0,
                StartedAt = DateTime.UtcNow,
                Status = ExamStatus.InProgress
            };

            await _examAttemptRepository.AddAsync(newAttempt);

        }

    }
}

