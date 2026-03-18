using System.Security;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;


namespace TestAPI.Services.Interfaces
{
    public interface IQuestionService
    {

        public  Task<int> CreateAsync(CreateQuestionDto createQuestionDto);
        
        public Task CreateRangeAsync(CreateQuestionsDto createQuestionsDto); 

        public Task<IEnumerable<QuestionDto>> GetAllAsync();

        public Task<QuestionDto> GetByIdAsync(int questionId);

        public Task DeleteRangeAsync(IEnumerable<int> questionIds);


        public Task DeleteQuestion(int questionId);

        public Task<UpdateQuestionDto> UpdateAsync(int questionId, UpdateQuestionDto questionDto);
    
   

    }
}
