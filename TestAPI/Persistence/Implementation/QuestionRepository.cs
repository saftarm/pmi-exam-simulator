using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

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
        public async Task<Guid> AddAsync(Question newQuestion)
        {

            await _context.Questions.AddAsync(newQuestion);
            await _context.SaveChangesAsync();
            return newQuestion.Id;
        }

        public async Task AddRangeAsync(IEnumerable<Question> questions)
        {
            _context.Questions.AddRange(questions);
            await _context.SaveChangesAsync();
        }



        // Update
        public async Task<UpdateQuestionDto> UpdateAsync(Guid questionId, UpdateQuestionDto updateQuestionDto)
        {

            var question = await _context.Questions.
            Include(o => o.AnswerOptions).
            FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                throw new Exception("Question not found");
            }

            if (updateQuestionDto.Text == null)
            {
                throw new ArgumentNullException(nameof(updateQuestionDto), "Question lacks Text field");
            }

            _context.AnswerOptions.RemoveRange(question.AnswerOptions);

            question.AnswerOptions = updateQuestionDto.AnswerOptionsDtos.Select(
                o => new AnswerOption
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }
            ).ToList();
            question.Title = updateQuestionDto.Text;


            await _context.SaveChangesAsync();

            return updateQuestionDto;

        }
        // Delete
        public async Task DeleteAsync(Guid questionId)
        {
            var question = _context.Questions.Find(questionId)!;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        // Get By Id
        public async Task<Question> GetByIdAsync(Guid questionId)
        {

            var question = await _context.Questions
            .Include(o => o.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == questionId);


            if (question == null)
            {
                throw new Exception("Question Not Found");
            }

            return question;
        }

        public async Task<ICollection<Question>> GetByIdsAsync(ICollection<Guid> questionIds)
        {
            return await _context.Questions.Where(q => questionIds.Contains(q.Id))
                .Include(q => q.AnswerOptions)
                .ToListAsync();

        }

        // Get all
        public async Task<IEnumerable<Question>> GetAllAsync()
        {


            var questions = await _context.Questions
            .Include(o => o.AnswerOptions)
            .ToListAsync();

            return questions;

        }


        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsByQuestionID(Guid questionId)
        {
            return await _context.AnswerOptions.Where(o => o.QuestionId == questionId).ToListAsync();
        }



        public async Task DeleteRangeAsync(IEnumerable<Guid> questionIds)
        {
            await _context.Questions.Where(q => questionIds.Contains(q.Id)).ExecuteDeleteAsync();

        }

        public async Task<IEnumerable<Question>> GetFixedAmountOfRandomQuestionsByDomainId(Guid domainId, int numberOfQuestions)
        {
            return await _context.Questions
            .Where(q => q.DomainId == domainId)
            .OrderBy(q => EF.Functions.Random())
            .Take(numberOfQuestions)
            .ToListAsync();

        }

    }

}