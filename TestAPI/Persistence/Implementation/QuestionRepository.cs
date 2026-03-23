using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.DTO;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace TestAPI.Persistence.Implementation
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add 
        public async Task<int> AddAsync(Question newQuestion)
        {

            await _context.Questions.AddAsync(newQuestion);
            await _context.SaveChangesAsync();
            return newQuestion.Id;
        }

        public async Task AddRangeAsync(ICollection<Question> questions) {
            _context.Questions.AddRange(questions);
            await _context.SaveChangesAsync();
        }



        // Update
        public async Task<UpdateQuestionDto> UpdateAsync(int questionId, UpdateQuestionDto updateQuestionDto)
        {
            
            var question = await _context.Questions.
            Include(o => o.AnswerOptions).
            FirstOrDefaultAsync(q => q.Id == questionId);

            if( question == null) {
                throw new Exception("Question not found");
            }

            _context.AnswerOptions.RemoveRange(question.AnswerOptions);

            question.AnswerOptions = updateQuestionDto.AnswerOptionsDtos.Select( 
                o => new AnswerOption{
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }
            ).ToList();
            question.Title = updateQuestionDto.Text;

            
            await _context.SaveChangesAsync();

            return updateQuestionDto;

           

        }
        // Delete
        public async Task DeleteAsync(int questionId)
        {
            var question = _context.Questions.Find(questionId)!;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        // Get By Id
        public async Task<Question> GetByIdAsync(int questionId) {
            
            var question = await _context.Questions
            .Include(o => o.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == questionId);

            
            if(question == null ) {
                throw new Exception("Question Not Found");
            }
            
            return question;
        }

        public async Task<ICollection<Question>> GetByIdsAsync(ICollection<int> questionIds)
        {
            return await _context.Questions.Where(q => questionIds.Contains(q.Id))
                .Include(q => q.AnswerOptions)
                .ToListAsync();

        }

        // Get all
        public async Task <IEnumerable<Question>> GetAllAsync()
        {


            var questions = await _context.Questions
            .Include(o => o.AnswerOptions)
            .ToListAsync();

            return questions;
            
        }


        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsByQuestionID (int questionId) {
            return await _context.AnswerOptions.Where(o => o.QuestionId == questionId).ToListAsync();
        }



        public async Task DeleteRangeAsync(IEnumerable<int> questionIds) {
            await _context.Questions.Where(q => questionIds.Contains(q.Id)).ExecuteDeleteAsync();
            
        }

    }

}