namespace TestAPI.Entities;

/// <summary>
/// Question presentation type. Multiple-choice scoring is not implemented yet.
/// </summary>
public enum QuestionType
{
    SingleChoice = 1,
    MultipleChoice = 2,
    TrueFalse = 3,
}
