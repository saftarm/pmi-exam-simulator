using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{ 
    public class ProgressService : IProgressService
    {

        private readonly ApplicationDbContext _context;

        private readonly IExamAttemptRepository _examAttemptRepository;

        public ProgressService(IExamAttemptRepository examAttemptRepository,
        ApplicationDbContext  context)
        {

            _context = context;
            _examAttemptRepository = examAttemptRepository;

        }
        public async Task UpdateDomainPerformance(ExamAttempt examAttempt) {

            var performanceUpdates = new List<DomainPerformance>();
            
            var userExamResponses = examAttempt.UserExamResponses;

            var userId = examAttempt.UserId;
            var exam = examAttempt.Exam;

            if(exam == null) {
                throw new Exception("Exam is not found");
            }
            var domainIds = exam.Domains.Select(d => d.Id).ToList();

            var existingRecords = await _context.DomainPerformances.
            Where(dp =>dp.UserId == userId 
            && dp.ExamId == exam.Id 
            && domainIds.Contains(dp.DomainId)).ToListAsync();
            
            System.Console.WriteLine(existingRecords.Count());

            var questions = exam.Questions;

            
            foreach(var domainId in domainIds) {

                var correctPerDomain = 0;
                var domainQuestions = questions.Where(q => q.DomainId == domainId);
                var totalPerDomain = domainQuestions.Count();

                foreach(var question in domainQuestions) {
                    var response = userExamResponses.FirstOrDefault(r => r.QuestionId == question.Id);
                    if(response.IsCorrect) {
                        correctPerDomain++;
                    }
                }
                var existing = existingRecords.FirstOrDefault(dp => dp.DomainId == domainId);

                if(existing == null) {
                    _context.DomainPerformances.Add(new DomainPerformance {
                         UserId = userId,
                        ExamId = exam.Id,
                        DomainId = domainId,
                        TotalAnswered = totalPerDomain,
                        TotalCorrect = correctPerDomain,
                        LastUpdated = DateTime.UtcNow
                    });
                }
                else {
                    existing.TotalAnswered = totalPerDomain;
                    existing.TotalCorrect = correctPerDomain;
                    existing.LastUpdated = DateTime.UtcNow;
                }

                
                
            }

            await _context.SaveChangesAsync();
                 
            
        }

        public async Task<UserProgressSummaryDto> GetUserProress() {
           
        }
        
        










    }
}
