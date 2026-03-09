using System.Security;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;


namespace TestAPI.Services.Interfaces
{
    public interface IQuestionService
    {

        public  Task<int> CreateAsync(CreateQuestionDto questionDto);

        public Task<IEnumerable<QuestionDto>> GetAllAsync();

        public Task<QuestionDto> GetByIdAsync(int questionId);


        public Task DeleteQuestion(int questionId);

        public Task<UpdateQuestionDto> UpdateAsync(int questionId, UpdateQuestionDto questionDto);
    
   

    }
}
