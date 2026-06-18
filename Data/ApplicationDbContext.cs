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

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>()
      .HasIndex(u => u.UserName)
      .IsUnique();


      // Exam - Category 
      modelBuilder.Entity<Exam>()
      .HasOne(e => e.Category)
      .WithMany(c => c.Exams)
      .HasForeignKey(e => e.CategoryId)
      .OnDelete(DeleteBehavior.Restrict);

      // -------------------------------------------------------------------

      // Domain - Exam
      modelBuilder.Entity<Domain>()
          .HasOne(d => d.Exam)
          .WithMany(e => e.Domains)
          .HasForeignKey(d => d.ExamId)
          .OnDelete(DeleteBehavior.Cascade);


      // -------------------------------------------------------------------

      // Question - Exam
      modelBuilder.Entity<Question>()
      .HasOne(q => q.Exam)
      .WithMany(e => e.Questions)
      .HasForeignKey(q => q.ExamId)
      .OnDelete(DeleteBehavior.Restrict);

      // Question - Domain
      modelBuilder.Entity<Question>()
      .HasOne(q => q.Domain)
      .WithMany(d => d.Questions)
      .HasForeignKey(q => q.DomainId)
      .OnDelete(DeleteBehavior.NoAction);

      // -------------------------------------------------------------------
      // AnswerOption - Questions (Every questions has its own AnswerOptions)
      modelBuilder.Entity<AnswerOption>().
      HasOne(o => o.Question)
      .WithMany(q => q.AnswerOptions)
      .HasForeignKey(o => o.QuestionId)
      .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<AnswerOption>()
        .HasOne(o => o.Domain)
        .WithMany(d => d.AnswerOptions)
        .HasForeignKey(o => o.DomainId)
        .OnDelete(DeleteBehavior.NoAction);
         


      // -------------------------------------------------------------------


      // UserExamResponse - ExamAttempt (Every response linked to the ExamAttempt)
      modelBuilder.Entity<UserExamResponse>()
      .HasOne(r => r.ExamAttempt)
      .WithMany(e => e.UserExamResponses)
      .HasForeignKey(r => r.ExamAttemptId)
      .OnDelete(DeleteBehavior.Cascade);

      // UserExamResponse - Question  (Every response linked to the questions)
      modelBuilder.Entity<UserExamResponse>()
      .HasOne(r => r.Question)
      .WithMany(q => q.UserExamResponses)
      .HasForeignKey(r => r.QuestionId)
      .OnDelete(DeleteBehavior.NoAction);

      // UserExamResponse - SelectedOptionId (Every response linked to the answer option)

      modelBuilder.Entity<UserExamResponse>()
     .HasOne(r => r.SelectedOption)
     .WithMany()
     .HasForeignKey(r => r.SelectedOptionId)
     .OnDelete(DeleteBehavior.Restrict);


      // UserExamResponse - Domain (Every response linked to the domain)
      modelBuilder.Entity<UserExamResponse>()
      .HasOne(r => r.Domain)
      .WithMany(d => d.UserExamResponses)
      .HasForeignKey(r => r.DomainId)
      .OnDelete(DeleteBehavior.NoAction);

      // -------------------------------------------------------------------

      modelBuilder.Entity<ExamAttempt>()
        .Property(a => a.PercentageScore)
        .HasColumnType("decimal(5,2)")
        .HasComputedColumnSql(
            "CASE WHEN \"TotalQuestions\" = 0 THEN 0.0 ELSE (\"CorrectCount\"::numeric / \"TotalQuestions\"::numeric) * 100 END",
            stored: true
            );


      // ExamAttempt - User (ExamAttempt belongs to the specific user)
      modelBuilder.Entity<ExamAttempt>()
      .HasOne<User>()
      .WithMany(u => u.ExamAttempts)
      .HasForeignKey(ea => ea.UserId)
      .OnDelete(DeleteBehavior.Cascade);

      // -------------------------------------------------------------------
      //
      //
      // DomainPerformance - User (Every user has its own DomainPerformance)
      modelBuilder.Entity<DomainPerformance>()
      .HasOne<User>()
      .WithMany(u => u.DomainPerfomances)
      .HasForeignKey(dp => dp.UserId)
      .OnDelete(DeleteBehavior.Restrict);

      // DomainPerformance - Domain (Every DomainPerformance linked to the specific Domain)
      modelBuilder.Entity<DomainPerformance>()
      .HasOne(dp => dp.Domain)
      .WithMany(d => d.DomainPerformances)
      .HasForeignKey(dp => dp.DomainId)
      .OnDelete(DeleteBehavior.Cascade);

      // DomainPerformance - Exam (Every DomainPerformance linked to the specific Exam )
      modelBuilder.Entity<DomainPerformance>()
      .HasOne(dp => dp.Exam)
      .WithMany(d => d.DomainPerfomances)
      .HasForeignKey(dp => dp.ExamId)
      .OnDelete(DeleteBehavior.Cascade);

      // DomainPerformance table 3 foreight key index 
      modelBuilder.Entity<DomainPerformance>()
      .HasIndex(dp => new { dp.UserId, dp.DomainId, dp.ExamId })
      .IsUnique();





      // -------------------------------------------------------------------

      modelBuilder.Entity<RefreshToken>()
      .HasOne<User>()
      .WithMany(u => u.RefreshTokens)
      .HasForeignKey(rt => rt.UserId)
      .OnDelete(DeleteBehavior.Cascade);

    }

  }
}
