
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

        public ExamAttemptService (
            IExamAttemptRepository examAttemptRepository,
            IExamRepository examRepository) 
            {
            _examAttemptRepository = examAttemptRepository;
            _examRepository = examRepository;
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
                Score = 0,
                UserExamResponses = new List<UserExamResponse>(),
                Status = ExamStatus.InProgress
            };
            return await _examAttemptRepository.AddAsync(examAttempt);

        }


         public async Task FinishAttemptAsync(int examAttemptId) {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);
            examAttempt.Status = ExamStatus.Completed;
            await CalculateScore(examAttempt);

         }

        public async Task<int> SaveResponse (int examAttemptId, int questionId, int selectedOptionId) {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);


            var newResponse = new UserExamResponse {
                SelectedOptionId = selectedOptionId,
                QuestionId = questionId,
                ExamAttemptId = examAttemptId
            };
            if(examAttempt.UserExamResponses == null) {
                examAttempt.UserExamResponses = new List<UserExamResponse>();
            }
            examAttempt.UserExamResponses.Add(newResponse);
            await _examAttemptRepository.Update(examAttempt);
            return examAttempt.Id;

        }

        public async Task CalculateScore(ExamAttempt examAttempt) 
        {
            var score = 0;
            var answerOptions = await _examAttemptRepository.GetAnswerOptionsByExamId(examAttempt.ExamId);
            foreach( var response in examAttempt.UserExamResponses!) {
                var currentOption = answerOptions.FirstOrDefault(o => o.Id == response.SelectedOptionId);
                if(currentOption.IsCorrect){
                    score += 10;
                }
            }
            examAttempt.Score += score;

            await _examAttemptRepository.Update(examAttempt);
            
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

