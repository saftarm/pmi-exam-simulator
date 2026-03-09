



using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.DTO;


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
            .Include(r => r.UserExamResponses)
            .FirstOrDefaultAsync(a => a.Id == examAttemptId); 
            
            
            return examAttempt;
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

        public async Task<int> UpdateAsync (int examAttemptId, UpdatedExamAttemptDto updatedExamAttemptDto) {
            
            var examAttempt = await _context.ExamAttempts.FindAsync(examAttemptId);

            examAttempt.sco


            return 
        }

        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsAsync() {
            return await _context.AnswerOptions.ToListAsync();
        }


        

    }
}


