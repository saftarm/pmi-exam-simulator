using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{ 
    public class ProgressService : IProgressService
    {
        private readonly IExamAttemptRepository _examAttemptRepository;

        public ProgressService( IExamAttemptRepository examAttemptRepository)
        {
            
            _examAttemptRepository = examAttemptRepository;
        }

        // private static IEnumerable<ExamAttemptDto> MapToExamAttemptDtos(ICollection<ExamAttempt> attempts) {
        //     return attempts.Select(
        //         a => new ExamAttemptDto {
        //             ExamTitle = a.ExamTitle,
        //             CorrectAnswersCount = a.c
        //         }
        //     )
        // }

        // public async Task<IEnumerable<ExamAttemptDto>> GetHistoryByUserId (int userId) {

           
        //     var attempts = await  _examAttemptRepository.GetAllAsync();

            



            

        // }

        










    }
}
