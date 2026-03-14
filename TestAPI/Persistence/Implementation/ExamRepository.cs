using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;


namespace TestAPI.Persistence.Implementation
{ 
    public class ExamRepository : IExamRepository
    {

        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Exam>> GetAllAsync() {
            return await _context.Exams.ToListAsync();
        }
        
        public void Add(Exam exam)
        {
            _context.Exams.Add(exam);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId) 
        {
            var question = await _context.Questions
            .Include(o => o.AnswerOptions)
            .Where(q => q.ExamsQuestions.Any(eq => eq.ExamId == examId))
            .ToListAsync();

            return question;
        }


        public async Task DeleteAsync(int examId) {
            await _context.Exams.Where(e => e.Id == examId).ExecuteDeleteAsync();

            
            await _context.SaveChangesAsync();

        }

        public async Task<Exam?> GetByIdAsync(int id)
        {
            var examFromDb = await _context.Exams.Include(e => e.Questions!)
            .ThenInclude(q => q.AnswerOptions!)
            .FirstOrDefaultAsync(e => e.Id == id);

            return examFromDb;
        }

        public  async Task AddQuestionToExamAsync(int questionId, Exam exam)
        {
            
            if(exam.Questions == null) {
                exam.Questions = new List<Question>();
            }


            var question = await _context.Questions.FindAsync(questionId);

            if(question == null ) 
            {
                throw new ArgumentException($"Question with id{questionId} not found");
            }
        
            exam.Questions.Add(question);

            if(question.Exams == null) {
                question.Exams = new List<Exam>();
            }

            
            if(exam.Questions.Contains(question)){
                exam.Questions.Add(question);
                question.Exams.Add(exam);
                await _context.SaveChangesAsync();
            }


        }


    }
}
