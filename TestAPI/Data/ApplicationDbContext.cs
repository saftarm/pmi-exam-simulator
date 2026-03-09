using Microsoft.EntityFrameworkCore;
using TestAPI.Models;
using TestAPI.Entities;
using System.Security.Cryptography;

namespace TestAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionCategory> QuestionCategories { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<ExamAttempt> ExamAttempts {get;set;}

        public DbSet<UserExamResponse> UserExamResponses {get;set;}

        public DbSet<ExamQuestion> ExamsQuestions {get;set;}

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     base.OnConfiguring(optionsBuilder);

        //     Database.EnsureCreated();
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Question>()
            .HasKey(q => q.Id);

            // modelBuilder.Entity<Question>()
            // .HasMany(q => q.Exams)
            // .WithMany(e => e.Questions);


            modelBuilder.Entity<AnswerOption>().
            HasOne(o => o.Question)
            .WithMany(q => q.AnswerOptions)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ExamAttempt>()
            .HasOne(ea => ea.User)
            .WithMany(u => u.ExamAttempts)
            .HasForeignKey(ea => ea.UserId) 
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamResponse>()
            .HasOne(r => r.ExamAttempt)
            .WithMany(e => e.UserExamResponses)
            .HasForeignKey(r => r.ExamAttemptId);

            modelBuilder.Entity<ExamAttempt>()
            .Property(e => e.Status)
            .HasConversion<string>();

        


        }
        

     


        





    }
}
