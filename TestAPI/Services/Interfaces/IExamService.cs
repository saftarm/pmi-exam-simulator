using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface IExamService
    {

        public Task CompileExam(CompileExamDto compileExamDto);
        public Task<IEnumerable<ExamSummaryDto>> GetSummaryAsync();
        public  Task<ExamDetailsDto> GetDetailsByIdAsync(int examId);        
        public Task<ExamDto> GetByIdAsync(int examId);
        // public Task<CreateExamDto> CreateAsync(CreateExamDto createExamDto);
        // public Task<UpdateExamDto> UpdateExamDto(int id, UpdateExamDto updateExamDto);

        public Task DeleteAsync(int examId);

        public Task<int> CalculateScore(int examAttemptId);

    }
}
