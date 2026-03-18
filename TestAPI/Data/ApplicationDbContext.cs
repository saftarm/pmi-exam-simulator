using Microsoft.EntityFrameworkCore;
using TestAPI.Entities;
using TestAPI.Models;

namespace TestAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<ExamAttempt> ExamAttempts { get; set; }

        public DbSet<UserExamResponse> UserExamResponses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Domain> Domains { get; set; }

    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Question>()
            .HasKey(q => q.Id);


            modelBuilder.Entity<AnswerOption>().
            HasOne(o => o.Question)
            .WithMany(q => q.AnswerOptions)
            .HasForeignKey(o => o.QuestionId)
            .HasConstraintName("FK_AnswerOptions_Exams_ExamId")
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<AnswerOption>()
            .HasOne(e => e.Exam)
            .WithMany(o => o.AnswerOptions)
            .HasForeignKey(e => e.ExamId);

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


            modelBuilder.Entity<Exam>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Exams)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

            


            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Questions)
                .WithMany(q => q.Exams)
                .UsingEntity(j => j.ToTable("ExamQuestion"));

            modelBuilder.Entity<Domain>()
                .HasOne(d => d.Exam)
                .WithMany(e => e.Domains)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Question>()
            .HasOne(q => q.Domain)
            .WithMany(d => d.Questions)
            .HasForeignKey(q => q.DomainId)
            .OnDelete(DeleteBehavior.Cascade);









        }











    }
}
