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

        public async Task<IEnumerable<QuestionDto>> GetAllAsync() {
            var questions = await _questionRepository.GetAllAsync();
            var questionDtos = new List<QuestionDto>();

            foreach(var question in questions) {

                var questionDto = new QuestionDto {
                    Text = question.Text,
                    AnswerOptionsDtos =  question.AnswerOptions.Select
                    (o => new AnswerOptionDto {
                        Text = o.Text
                    }).ToList()
                };
                questionDtos.Add(questionDto);
        }
        return questionDtos;

        }

        public async Task<QuestionDto> GetByIdAsync(int questionId)
        {

            var question = await _questionRepository.GetByIdAsync(questionId);

            if(question == null) {
                throw new Exception("Question not found");
            }
            var questionDto = new QuestionDto {
                Text = question.Text,
                AnswerOptionsDtos = question.AnswerOptions.Select(
                    o => new AnswerOptionDto {
                        Text = o.Text,
                    }  
                ).ToList()
            };
           return questionDto;
        }


        public async Task<int> CreateAsync(CreateQuestionDto createQuestionDto)
        {
            
           return await _questionRepository.AddAsync(createQuestionDto);
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
