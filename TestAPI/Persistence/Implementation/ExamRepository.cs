using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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


        public IQueryable<Exam> GetAllAsync()
        {
            var examsQuery = _context.Exams
            .Include(e => e.Category)
            .Include(e => e.Questions)
            .AsQueryable();
            return examsQuery; 
        }

        public async Task AddAsync(IEnumerable<Exam> exams)
        {
            await _context.Exams.AddRangeAsync(exams);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId)
        {

            var questions = await _context.Questions
            .Where(q => q.ExamId == examId)
            .Include(q => q.AnswerOptions)
            .ToListAsync();
            // if (questions.Any())
            // {
            //     throw new KeyNotFoundException("Questions not found");
            // }
            return questions;
        }

     



        public async Task UpdateAsync(Exam exam) {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(int examId)
        {
            await _context.Exams.Where(e => e.Id == examId).ExecuteDeleteAsync();


            await _context.SaveChangesAsync();

        }


        public async Task DeleteRangeAsync(IEnumerable<int> examIds) {
            await _context.Exams.Where(q => examIds.Contains(q.Id)).ExecuteDeleteAsync();
            
        }

        public async Task<Exam> GetByIdAsync(int id)
        {
            var examFromDb = await _context.Exams
            .Include( e=> e.Category)
            .Include(e => e.Questions!)
            .ThenInclude(q => q.AnswerOptions!)
            .FirstOrDefaultAsync(e => e.Id == id);

            return examFromDb;
        }


        public async Task<IEnumerable<Exam>> GetAllById(ICollection<int> examIds)
        {
            return await _context.Exams.Where(e => examIds.Contains(e.Id)).ToListAsync() ;

        }

        // public async Task AddQuestionToExamAsync(int questionId, Exam exam)
        // {

        //     if (exam.Questions == null)
        //     {
        //         exam.Questions = new List<Question>();
        //     }


        //     var question = await _context.Questions.FindAsync(questionId);

        //     if (question == null)
        //     {
        //         throw new ArgumentException($"Question with id{questionId} not found");
        //     }

        //     exam.Questions.Add(question);

        //     if (question.Exams == null)
        //     {
        //         question.Exams = new List<Exam>();
        //     } 


        //     if (exam.Questions.Contains(question))
        //     {
        //         exam.Questions.Add(question);
        //         question.Exams.Add(exam);
        //         await _context.SaveChangesAsync();
        //     }


        // }


        public async Task AddQuestionsToExamAsync(int examId, ICollection<Question> questions)
        {
            var exam = await _context.Exams.FindAsync(examId);

            foreach (var question in questions)
            {
                exam.Questions.Add(question);
            }

        }


        public async Task<ExamStatus> GetExamStatusByIdAsync(int id) {
            var examStatus = await _context.Exams
            .Where(e => e.Id == id) 
            .Select(e => e.Status) 
            .FirstOrDefaultAsync();

            return examStatus; 
            
        }
    }
}
