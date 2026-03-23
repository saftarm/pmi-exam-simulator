using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Packaging;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;
using TestAPI.Exceptions;
using System.Diagnostics;



namespace TestAPI.Services.Implementation
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionService _questionService;

        private readonly IDomainRepository _domainRepository;

        public ExamService(IExamRepository examRepository,
         IQuestionRepository questionRepository,
         IExamAttemptRepository examAttemptRepository, 
         IQuestionService questionService,
         IDomainRepository domainRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _examAttemptRepository = examAttemptRepository;
            _questionService = questionService;
            _domainRepository = domainRepository;
        }

  
        // public async Task CompileExam(int examId)
        // {
        //     var exam = await _examRepository.GetByIdAsync(examId);

        //     var questions = await _questionRepository.GetByIdsAsync(questionIds);

        //     exam.Questions.AddRange(questions);
        //     await _examRepository.UpdateAsync(exam);
        // }

        public async Task DeleteRangeAsync(ICollection<int> examIds) {
            await _examRepository.DeleteRangeAsync(examIds);
        }


        public async Task<ExamFullDto> GetByIdAsync(int examId) {
                ExamFullDto dto = new ();
                return dto;
        } 
        // Get Summary
        public async Task<IEnumerable<ExamSummaryDto>> GetSummariesAsync(PageParameters pageParameters)
        {
            var examsQuery = _examRepository.GetAllAsync();

            var pagedExams = await PagedList<Exam>.CreateAsync(examsQuery, pageParameters.PageNumber, pageParameters.PageSize);

            var examSummaries = pagedExams.Items.Select(
                pe => new ExamSummaryDto {
                    Id = pe.Id,
                    Title = pe.Title,
                    CategoryTitle = pe.Category.Title,
                    CategoryId = pe.CategoryId,
                    DurationInMinutes = pe.DurationInMinutes,
                    NumberOfQuestions = pe.NumberOfQuestions
                }
            );
           

            return examSummaries;
        }

        public async Task<IEnumerable<ExamDetailsDto>> GetDetailsAsync(PageParameters pageParameters) {
            var examsQuery = _examRepository.GetAllAsync();

            var pagedExams = await PagedList<Exam>.CreateAsync(examsQuery, pageParameters.PageNumber, pageParameters.PageSize);

            var examDetails = pagedExams.Items.Select(
                pe => new ExamDetailsDto {
                    QuestionDtos = pe.Questions.Select(
                        q => new QuestionDto {
                            Title = q.Title,
                            AnswerOptionsDtos = q.AnswerOptions.Select(
                                o => new AnswerOptionDto {
                                    Text = o.Text
                                }
                            ).ToList()
                        }
                        
                    ).ToList()
                }
            ).ToList();
            return examDetails;
        }


        public async Task<ExamSummaryDto> GetSummaryByIdAsync(int id) {
            var exam = await  _examRepository.GetByIdAsync(id);

            if(exam == null) {
                throw new RecordNotFoundException("Exam is not found");
            }
            var examSummaryDto = new ExamSummaryDto {
                Id = exam.Id,
                Title = exam.Title,
                CategoryTitle = exam.Category.Title,
                CategoryId = exam.CategoryId,
                DurationInMinutes = exam.DurationInMinutes,
                NumberOfQuestions = exam.NumberOfQuestions
            };

            return examSummaryDto;
        }

        public async Task<ExamDetailsDto> GetDetailsByIdAsync(int id)
        {

            var examStatus = await _examRepository.GetExamStatusByIdAsync(id);
            if(examStatus == ExamStatus.Draft || examStatus == ExamStatus.Compiled) {
                throw new Exception("Exam is not published yet");
            }
            var questions = await _examRepository.GetQuestionsByExamIdAsync(id);

            var examDetailsDto = new ExamDetailsDto
            {
                QuestionDtos = questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    AnswerOptionsDtos = q.AnswerOptions.Select(o => new AnswerOptionDto
                    {
                        Id = o.Id,
                        Text = o.Text,
                    }).ToList()
                }
                ).ToList()
            };

            if(examDetailsDto.QuestionDtos == null)
            {
                throw new Exception("Question DTOs are empty");
            }


            return examDetailsDto;
        }
 

        public async Task CreateExams(List<CreateExamDto> dto){

            var newExams = dto.Select(
                e => new Exam {
                    CategoryId = e.CategoryId,
                    Title = e.Title,
                    Context = e.Context,
                    DurationInMinutes = e.DurationInMinutes,
                    NumberOfQuestions = e.NumberOfQuestions,
                    PassScore = e.PassScore,
                    Domains = e.CreateDomainDtos.Select(
                        d => new Domain {
                            Title = d.Title,
                            Description = d.Description,
                            Weight = d.Weight
                        }
                    ).ToList()
                }
             
            ).ToList();

        

            await _examRepository.AddAsync(newExams);
        }

        

        public async Task AddQuestionsToExamAsync(int examId, ICollection<int> questionIds)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            var questions = await _questionRepository.GetByIdsAsync(questionIds);

        }
        

        public async Task<IEnumerable<PublishedExamDto>> GetPublishedExamsAsync(PageParameters pageParemeters) {

            var examsQuery = _examRepository.GetAllAsync();
            var pagedExams = await PagedList<Exam>.CreateAsync(examsQuery, pageParemeters.PageNumber, pageParemeters.PageSize);
            if(pagedExams.Items.Any()){
                throw new RecordNotFoundException("No published exams found for the requested page");
            }
            var exams = pagedExams.Items.Select(pe => {
                if(pe.Category == null) {
                    throw new InvalidOperationException(
                        $"Exam with ID {pe.Id} has no associated category." +
                        "Ensure Category is included in query.");}
                

                 return new PublishedExamDto {
                    Id = pe.Id,
                    Title = pe.Title,
                    DurationInMinutes = pe.DurationInMinutes,
                    NumberOfQuestions = pe.NumberOfQuestions,
                    Category = pe.Category.Title

                }; 
            }).ToList(); 

            return exams;



        }



        public async Task<Exam?> GetExamAsync(int examId)
        {

            var exam = await _examRepository.GetByIdAsync(examId);
            return exam;
        }


        public async Task DeleteAsync(int examId)
        {

            await _examRepository.DeleteAsync(examId);
        }


        public async Task<int> CalculateScore(int examAttemptId)
        {
            var examAttempt = await _examAttemptRepository.GetByIdAsync(examAttemptId);

            foreach (var answerOption in examAttempt.UserExamResponses)
            {
                if (answerOption.IsCorrect)
                {
                    examAttempt.Score += 10;
                }

            }
            return examAttempt.Score;
        }

    }
}
