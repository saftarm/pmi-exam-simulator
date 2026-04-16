using Microsoft.EntityFrameworkCore;
using TestAPI.Entities;

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
        public DbSet<DomainPerformance> DomainPerformances { get; set; }

        public DbSet<RefreshToken> RefreshTokens {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

            modelBuilder.Entity<AnswerOption>().
            HasOne(o => o.Question)
            .WithMany(q => q.AnswerOptions)
            .HasForeignKey(o => o.QuestionId)
            .HasConstraintName("FK_AnswerOptions_Exams_ExamId")
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamAttempt>()
            .HasOne<User>()
            .WithMany(u => u.ExamAttempts)
            .HasForeignKey(ea => ea.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamResponse>()
            .HasOne(r => r.ExamAttempt)
            .WithMany(e => e.UserExamResponses)
            .HasForeignKey(r => r.ExamAttemptId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamResponse>()
            .HasOne(r => r.Question)
            .WithMany(q => q.UserExamResponses)
            .HasForeignKey(r => r.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserExamResponse>()
           .HasOne(r => r.SelectedOption)
           .WithMany()
           .HasForeignKey(r => r.SelectedOptionId)
           .OnDelete(DeleteBehavior.Restrict);

           modelBuilder.Entity<UserExamResponse>()
           .HasOne(r => r.Domain)
           .WithMany(d => d.UserExamResponses)
           .HasForeignKey(r => r.DomainId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamAttempt>()
            .Property(e => e.Status)
            .HasConversion<string>();

            modelBuilder.Entity<Exam>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Exams)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
            .HasOne(q => q.Exam)
            .WithMany(e => e.Questions)
            .HasForeignKey(q => q.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domain>()
                .HasOne(d => d.Exam)
                .WithMany(e => e.Domains)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Question>()
            .HasOne(q => q.Domain)
            .WithMany(d => d.Questions)
            .HasForeignKey(q => q.DomainId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DomainPerformance>()
            .HasOne<User>()
            .WithMany(u => u.DomainPerfomances)
            .HasForeignKey(dp => dp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DomainPerformance>()
            .HasOne(dp => dp.Domain)
            .WithMany(d => d.DomainPerformances)
            .HasForeignKey(dp => dp.DomainId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DomainPerformance>()
            .HasOne(dp => dp.Exam)
            .WithMany(d => d.DomainPerfomances)
            .HasForeignKey(dp => dp.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DomainPerformance>()
            .HasIndex(dp => new { dp.UserId, dp.DomainId, dp.ExamId })
            .IsUnique();

            modelBuilder.Entity<RefreshToken>()
            .HasOne<User>()
            .WithOne(u => u.RefreshToken)
            .HasForeignKey<RefreshToken>(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
