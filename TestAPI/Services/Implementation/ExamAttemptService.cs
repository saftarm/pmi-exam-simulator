
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
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;


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


        // public async Task<int> FinishAttemptAsync(int examAttemptId, IEnumerable<UserExamResponseDto> userExamResponseDtos)
        // {
            
        //     var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

        //     var incomingResponses = new List<UserExamResponse>();

        //      var answerOptions = await _examAttemptRepository.GetAnswerOptionsAsync();
            


        //     examAttempt.UserExamResponses.Add()

        //     foreach(var incoming in incomingResponses) {

        //         var option = answerOptions.FirstOrDefault(o => o.Id == incoming.SelectedOptionId);
        //         if(option.IsCorrect) {
        //             var newUserExamResponse = new UserExamResponse {
        //                 ExamAttemptId =,
        //                 QuestionId,
        //                 SelectedOptionId,
        //                 IsCorrect,
        //             }
        //         }
        //     }

        //     examAttempt.Score  = await CalculateScore(examAttempt.UserExamResponses);
        //     examAttempt.Status = ExamStatus.Completed;

        //     return await _examAttemptRepository.Update(examAttempt);


        // }


        public async Task<int> CalculateScore (IEnumerable<UserExamResponse> userExamResponses) {
            var score = 0;
            var answerOptions = await _examAttemptRepository.GetAnswerOptionsAsync();

            foreach(var option in userExamResponses){
                var currentOption = answerOptions.FirstOrDefault(o => o.Id == option.SelectedOptionId);
                if(currentOption.IsCorrect){
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

