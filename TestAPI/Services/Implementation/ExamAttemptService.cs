
using System.Security.Claims;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;
using System.Security.Claims;
using TestAPI.Enums;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TestAPI.Data;
using NuGet.Common;


namespace TestAPI.Services.Implementation
{
    public class ExamAttemptService : IExamAttemptService
    {
        private readonly IExamAttemptRepository _examAttemptRepository;

        public ExamAttemptService (
            IExamAttemptRepository examAttemptRepository) 
            {
            _examAttemptRepository = examAttemptRepository;
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
            var examAttempt = new ExamAttempt {
                UserId = userId,
                ExamId = examId,  
                StartedAt = DateTime.UtcNow,
                Score = 0,
                Status = ExamStatus.InProgress

            };
            return await _examAttemptRepository.AddAsync(examAttempt);

        }


        public async Task FinishAttemptAsync(int examAttemptId, IEnumerable<UserExamResponse> userExamResponses)
        {

            var score = CalculateScore(userExamResponses);


            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

            examAttempt.Score  = await CalculateScore(examAttempt.UserExamResponses);
            examAttempt.Status = ExamStatus.Completed;
            examAttempt.UserExamResponses.Select(r => r.)


        }


        public async Task<int> CalculateScore (IEnumerable<UserExamResponse> userExamResponses) {
            var score = 0;
            var answerOptions = await _examAttemptRepository.GetAnswerOptionsAsync();

            foreach(var option in userExamResponses){
                var currentOption = answerOptions.FirstOrDefault(o => o.Id == option.Id)
                if(currentOption.IsCorrect){
                    option.IsCorrect = true;
                }
            }

            foreach(var answer in answerOptions) {
                if(answer.IsCorrect) {
                    score += 10;
                }
            }
            return score;
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

        // public async Task<ExamAttempt> FinishAttempt(int examAttemptId) {

        //     var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

        //     if(examAttempt == null) {
        //         throw new Exception("ExamRepository could not find examAttempt");
        //     }
        //     await CalculateScore(examAttempt);

        //     examAttempt.Status = ExamStatus.Completed;
        //     examAttempt.SubmittedAt = DateTime.UtcNow;

        //     await _context.SaveChangesAsync();

        //     return examAttempt;


        // }

        // private async Task<int> CalculateScore( ExamAttempt examAttempt){

        //     if(examAttempt == null) {
        //         throw new Exception("Attempt not found");
        //     }
        //     foreach(var selectedOption in examAttempt.UserExamResponses )
        //     {
        //         var answerOption = await _answerOptionRepository.GetByIdAsync(selectedOption.SelectedOptionId);
        //         if(answerOption.IsCorrect) {
        //             examAttempt.Score += 10;
        //             selectedOption.IsCorrect = true;
        //         }
        //     }
            
        //     return examAttempt.Score;


        // }
        
        


       


       

    }
}

