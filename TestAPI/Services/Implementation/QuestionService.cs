using System.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
    public class QuestionService : IQuestionService
    { 
        private readonly IQuestionRepository _questionRepository;
        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;

        }
        private static QuestionDto MapToQuestionDto(Question question)
        {

            if (question.AnswerOptions == null)
            {
                throw new Exception("Answer Options not found");
            }
            return new QuestionDto
            {
                Id = question.Id,
                Title = question.Title,
                AnswerOptionsDtos = question.AnswerOptions.Select(o =>
                new AnswerOptionDto
                {
                    Id = o.Id,
                    Text = o.Text!

                }).ToList()
            };
        }

        private static ICollection<Question> MapQuestionDtosToQuestions(IEnumerable<CreateQuestionDto> createQuestionDtos)
        {
            var questions = createQuestionDtos.Select(
                q => new Question
                {
                    Title = q.Title,
                    Explanation = q.Explanation,
                    DomainId = q.DomainId,
                    AnswerOptions = q.AnswerOptionsDtos.Select(
                        o => new AnswerOption
                        {
                            Text = o.Text,
                            IsCorrect = o.IsCorrect
                        }
                    ).ToList()

                }
            ).ToList();

            return questions;
        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> questionIds)
        {
            await _questionRepository.DeleteRangeAsync(questionIds);

        }

        public async Task<IEnumerable<QuestionDto>> GetAllAsync()
        {
            var questions = await _questionRepository.GetAllAsync();


            return questions.Select(MapToQuestionDto).ToList();
        }

        public async Task<QuestionDto> GetByIdAsync(Guid questionId)
        {

            var question = await _questionRepository.GetByIdAsync(questionId);

            var questionDto = MapToQuestionDto(question);

            if (question == null)
            {
                throw new KeyNotFoundException("Question not found");
            }

            return questionDto;
        }

        public async Task<Guid> CreateAsync(CreateQuestionDto createQuestionDto)
        {
            var newQuestion = new Question
            {
                Title = createQuestionDto.Title,
                Explanation = createQuestionDto.Explanation,
                DomainId = createQuestionDto.DomainId,
                AnswerOptions = createQuestionDto.AnswerOptionsDtos.Select(
                    o => new AnswerOption
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }
                ).ToList()
            }; 
 

            return await _questionRepository.AddAsync(newQuestion);
        }

        // Create Multiple Questions
        public async Task CreateRangeAsync(List<CreateQuestionDto> dto)
        {

            var questions = MapQuestionDtosToQuestions(dto);

            await _questionRepository.AddRangeAsync(questions);
        }


        // Delete Question
        public async Task DeleteQuestion(Guid questionId)
        {
            await _questionRepository.DeleteAsync(questionId);

        }


        // Update Question
        public async Task<UpdateQuestionDto> UpdateAsync(Guid questionId, UpdateQuestionDto questionDto)
        {
            // validation 
            // mapping dto to entity
            // calling repository service for 
            return await _questionRepository.UpdateAsync(questionId, questionDto);
        }





    }
}
