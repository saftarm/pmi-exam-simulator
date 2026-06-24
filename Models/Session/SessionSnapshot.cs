using TestAPI.Entities;

namespace TestAPI.Models.Session;

public record SessionSnapshot
{
    public Guid SessionId { get; init; }

    public Guid ExamId { get; init; }

    public Guid UserId { get; init; }

    public int TotalQuestions { get; init; }

    public IReadOnlyList<SessionQuestionEntry> Questions { get; init; } = [];
}

public record SessionQuestionEntry
{
    public Guid QuestionId { get; init; }

    public QuestionType QuestionType { get; init; }

    public int OrderIndex { get; init; }
}
