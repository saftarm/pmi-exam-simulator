using Microsoft.AspNetCore.Http.HttpResults;
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

        private static QuestionDto MapToQuestionDto(Question question) {

            if(question.AnswerOptions == null ){
                throw new Exception("Answer Options not found");
            }
            return new QuestionDto {
                Id = question.Id,
                Title = question.Text,
                AnswerOptionsDtos = question.AnswerOptions.Select(o => 
                new AnswerOptionDto {
                    Id = o.Id,
                    Text = o.Text!
                }).ToList()
            }; 
        }

        public async Task<IEnumerable<QuestionDto>> GetAllAsync() {
            var questions = await _questionRepository.GetAllAsync();


            return questions.Select(MapToQuestionDto).ToList();
        }

        public async Task<QuestionDto> GetByIdAsync(int questionId)
        {

            var question = await _questionRepository.GetByIdAsync(questionId);

            var questionDto = MapToQuestionDto(question);

            if(question == null) {
                throw new KeyNotFoundException("Question not found");
            }
            // var questionDto = new QuestionDto {
            //     Id = question.Id,
            //     Text = question.Text,
            //     AnswerOptionsDtos = question.AnswerOptions!.Select(
            //         o => new AnswerOptionDto {
            //             Id = o.Id,
            //             Text = o.Text!,
            //         }  
            //     ).ToList()
            // };
           return questionDto;
        }   

        public async Task<int> CreateAsync(CreateQuestionDto createQuestionDto)
        {
            var newQuestion = new Question {
                Text = createQuestionDto.Text,
                AnswerOptions = createQuestionDto.AnswerOptionsDtos.Select(
                    o => new AnswerOption{
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }
                ).ToList()
            };

            
           return await _questionRepository.AddAsync(newQuestion);
        }


        public async Task DeleteQuestion(int questionId)
        {
            await _questionRepository.DeleteAsync(questionId);
            
        }

        public async Task<UpdateQuestionDto> UpdateAsync(int questionId, UpdateQuestionDto questionDto)
        {
                return await _questionRepository.UpdateAsync(questionId, questionDto);
           }



    }
}
