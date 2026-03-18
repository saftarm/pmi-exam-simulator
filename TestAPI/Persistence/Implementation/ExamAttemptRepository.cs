



using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.DTO;
using Microsoft.AspNetCore.Mvc;


namespace TestAPI.Persistence.Implementation
{
    public class ExamAttemptRepository : IExamAttemptRepository
    {

        private readonly ApplicationDbContext _context;

        public ExamAttemptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExamAttempt> GetByIdAsync(int examAttemptId) {
            var examAttempt = await _context.ExamAttempts
            .Include(ea => ea.UserExamResponses)
            .FirstOrDefaultAsync(ea => ea.Id == examAttemptId); 
            
            return examAttempt;
        }

        public async Task<IEnumerable<ExamAttempt>> GetAllAsync() {
            return await _context.ExamAttempts.ToListAsync();
        }

        public async Task<ExamAttempt> GetByUserId(int userId) {
            var examAttempt = await _context.ExamAttempts.FirstOrDefaultAsync(e => e.UserId == userId);

            if(examAttempt == null) {
                throw new Exception ("Attempt not found");
            }
            return examAttempt;
        }

          public async Task<int> AddAsync(ExamAttempt examAttempt) {

            await _context.ExamAttempts.AddAsync(examAttempt);
            await _context.SaveChangesAsync();
            return examAttempt.Id;
          }

        public async Task<int> UpdateAsync (ExamAttempt updatedExamAttempt) {
            _context.ExamAttempts.Update(updatedExamAttempt);
            await _context.SaveChangesAsync();
            return updatedExamAttempt.Id;
        }
         public async Task DeleteAsync(int examAttemptId) {
            var examAttempt = await _context.ExamAttempts.FindAsync(examAttemptId);
            _context.ExamAttempts.Remove(examAttempt);
            await _context.SaveChangesAsync();

         }

    

        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsAsync() {
            return await _context.AnswerOptions.ToListAsync();
        }


        public async Task<AnswerOption> GetAnswerOptionAsync(int selectedOptionId) {
            return await _context.AnswerOptions.FirstOrDefaultAsync(o => o.Id == selectedOptionId);
        }

        public async Task<IEnumerable<UserExamResponse>> GetResponsesAsync(int examAttemptId) {
            var examAttempt = await _context.ExamAttempts
            .Include(r => r.UserExamResponses)
            .FirstOrDefaultAsync(a => a.Id == examAttemptId);

            if(examAttempt.UserExamResponses == null) {
                throw new Exception("Responses not found");
            }


            return examAttempt.UserExamResponses.ToList();
        }

    }
}


