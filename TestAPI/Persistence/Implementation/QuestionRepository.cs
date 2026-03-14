using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.DTO;
using NuGet.Protocol.Core.Types;

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
            question.Text = updateQuestionDto.Text;

            
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

        // Get all
        public async Task <IEnumerable<Question>> GetAllAsync()
        {


            var questions = await _context.Questions
            .Include(o => o.AnswerOptions)
            .ToListAsync();

            return questions;
            
        }

        // public async Task<List<Exam>?> GetExamsByQuestionIdAsync(int questionId){
        //     var question = await _context.Questions.FindAsync(questionId);

        //     if(question == null) {
        //         throw new ArgumentException($"Question with id {questionId} does not exist");
        //     }
        //     if(question.Exams == null) {
        //         throw new ArgumentException($"Question with id {questionId} does not belong to any exam");
        //     }
        //     List<Exam> exams = question.Exams.ToList();

        //     return exams;

        // }



    }

}