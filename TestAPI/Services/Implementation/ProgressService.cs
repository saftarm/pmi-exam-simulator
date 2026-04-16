using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class ProgressService : IProgressService
    {
        private readonly ApplicationDbContext _context;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IExamRepository _examRepository;

        public ProgressService(
            IExamAttemptRepository examAttemptRepository,
            ApplicationDbContext context,
            IExamRepository examRepository)
        {

            _context = context;
            _examAttemptRepository = examAttemptRepository;
            _examRepository = examRepository;

        }


        public async Task<ExamProgressSummaryDto> GetExamProgressSummaryAsync(Guid userId, Guid examId, CancellationToken ct)
        {
            var examAttempts = await _examAttemptRepository.GetAttemptsByExamAndUserIdAsync(userId, examId, ct);

            if (examAttempts == null)
            {
                throw new RecordNotFoundException("No attempts found");
            }
            var latestScore = examAttempts.MaxBy(a => a.CreatedAt)!.Score;
            var averageScore = (int)examAttempts.Average(a => a.Score);
            var highestScore = examAttempts.Max(a => a.Score);
            var lastAttemptDate = examAttempts.Max(a => a.CreatedAt);
            var totalAttempts = examAttempts.Count();
            var totalCorrectAnswered = examAttempts.Sum(a => a.CorrectCount);
            var totalQuestionsAnswered = examAttempts.Sum(a => a.TotalQuesitons);


            var progressSummary = new ExamProgressSummaryDto
            {
                LatestScore = latestScore,
                AverageScore = averageScore,
                HighestScore = highestScore,
                LastAttemptDate = lastAttemptDate,
                TotalAttempts = totalAttempts,
                TotalCorrectAnswered = totalCorrectAnswered,
                TotalQuestionAnswered = totalCorrectAnswered
            };

            return progressSummary;

        }

        // public async Task UpdateDomainPerformance(ExamAttempt examAttempt)
        // {
        //     var performanceUpdates = new List<DomainPerformance>();
        //     var userResponses = examAttempt.UserExamResponses;

        //     if(userResponses == null) {
        //         throw new Exception("No responses found");
        //     }
        //     var userId = examAttempt.UserId;
        //     var exam = examAttempt.Exam;

        //     if (exam == null)
        //     {
        //         throw new RecordNotFoundException("Exam is not found");
        //     }
        //     var domainIds = exam.Domains.Select(d => d.Id).ToList();

        //     var existingRecords = await _context.DomainPerformances.
        //     Where(dp => dp.UserId == userId
        //     && dp.ExamId == exam.Id
        //     && domainIds.Contains(dp.DomainId)).ToListAsync();

        //     Console.WriteLine(existingRecords.Count());

        //     var questions = exam.Questions;

        //     foreach (var domainId in domainIds)
        //     {
        //         var correctPerDomain = 0;
        //         var domainQuestions = questions.Where(q => q.DomainId == domainId).ToList();
        //         var totalPerDomain = domainQuestions.Count();

        //         foreach (var question in domainQuestions)
        //         {
        //             var response = userResponses.FirstOrDefault(r => r.QuestionId == question.Id);

        //             if(response == null) {
        //                 throw new Exception("Response not found");
        //             }
        //             if (response.IsCorrect)
        //             {
        //                 correctPerDomain++;
        //             }
        //         }
        //         var existing = existingRecords.FirstOrDefault(dp => dp.DomainId == domainId);

        //         if (existing == null)
        //         {
        //             _context.DomainPerformances.Add(new DomainPerformance
        //             {
        //                 UserId = userId,
        //                 ExamId = exam.Id,
        //                 DomainId = domainId,
        //                 TotalAnswered = totalPerDomain,
        //                 TotalCorrect = correctPerDomain,
        //                 LastUpdated = DateTime.UtcNow
        //             });
        //         }
        //         else
        //         {
        //             existing.TotalAnswered = totalPerDomain;
        //             existing.TotalCorrect = correctPerDomain;
        //             existing.LastUpdated = DateTime.UtcNow;
        //         }
        //     }

        //     await _context.SaveChangesAsync();
        // }

        public async Task UpdateDomainPerformance(ExamAttempt examAttempt)
        {
            var hashMap = new Dictionary<Guid, (int correctCount, int totalCount)>();

            foreach (var response in examAttempt.UserExamResponses)
            {
                hashMap.TryGetValue(response.DomainId, out var stats);
  
                    hashMap[response.DomainId] = ( 
                        stats.correctCount + (response.IsCorrect ? 1 : 0),
                        stats.totalCount + 1 );       
            }
            
            var existingRecords = await _context.DomainPerformances
                .Where(dp => dp.UserId == examAttempt.UserId 
                && dp.ExamId == examAttempt.ExamId).ToListAsync();
            
            if(existingRecords.Any()) {
                foreach(var (domainId, stats) in hashMap) {
                    var existing = existingRecords.FirstOrDefault(r => r.DomainId == domainId);
                    existing!.TotalCorrect += stats.correctCount;
                    existing.TotalAnswered += stats.totalCount;
                }
            }
            else {
                var newRecords = new List<DomainPerformance>();
                foreach(var (domainId, stats) in hashMap) {
                    var newRecord = new DomainPerformance {
                        UserId = examAttempt.UserId,
                        ExamId = examAttempt.ExamId,
                        DomainId = domainId,
                        LastUpdated =DateTime.UtcNow,
                        TotalAnswered = stats.totalCount,
                        TotalCorrect = stats.correctCount
                    };
                    newRecords.Add(newRecord);
                }
                if(newRecords.Any()){
                    await _context.DomainPerformances.AddRangeAsync(newRecords);
                    await _context.SaveChangesAsync();
                }

            }

        }
    }
}
