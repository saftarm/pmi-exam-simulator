namespace TestAPI.ResultPattern
{
  public record Error(string StatusCode, string Description,  ErrorType ErrorType)
  {
    public readonly static Error None = new(string.Empty, string.Empty, default);
  }

  public static class Errors {
    public static Error RecordAlreadyExists {get;} = new ("CONFLICT", "Record already exists in the database", ErrorType.Conflict);
    public static Error RangeOfRecordsNotFound{get;} = new ("NOT_FOUND", "Range of records not found", ErrorType.NotFound);
    public static Error RecordNotFoundById {get;} = new ("NOT_FOUND", "Record not found by given Id", ErrorType.NotFound);
    public static Error RedisKeyNotFound {get;} = new ("NOT_FOUND", "Redis key not found", ErrorType.NotFound);
    public static Error ExamNotFound {get;} = new("NOT_FOUND", "Exam not found", ErrorType.NotFound);
    public static Error ExamAlreadyExists {get;} = new ("CONFLICT", "Exam with given title already exists", ErrorType.Conflict);
    public static Error InvalidToken {get;} = new ("UNAUTHORIZED", "User is unauthorized", ErrorType.Unauthorized);
    public static Error QuestionsNotFound {get;} = new ("NOT_FOUND", "Questions of the exam not found", ErrorType.NotFound);
    public static Error ExamCompilationFailed {get;} = new ("INTERNAL_SERVER_ERROR", "Exam compilation failed", ErrorType.InternalServerError);
    public static Error ExamDataCorrupted {get;} = new ("UNPROCESSABLE_ENTITY", "Exam data is corrupted", ErrorType.UnprocessableEntity);
    public static Error ValidationFailed {get;} = new ("UNPROCESSABLE_ENTITY", "Validation failed because of invalid input", ErrorType.Validation);
    public static Error UserNotFoundById {get;} = new Error("NOT_FOUND", "User not found by given Id", ErrorType.NotFound);
    public static Error AccountNotActive {get;} = new ("FORBIDDEN", "Account is not active", ErrorType.Forbidden);
    public static Error CannotSuspendSelf {get;} = new ("FORBIDDEN", "You cannot change your own account status", ErrorType.Forbidden);
    public static Error LastAdminRequired {get;} = new ("FORBIDDEN", "At least one admin account must remain", ErrorType.Forbidden);
    public static Error QuestionNotFound {get;} = new ("NOT_FOUND", "Question not found", ErrorType.NotFound);
    public static Error DomainNotFound {get;} = new ("NOT_FOUND", "Domain not found", ErrorType.NotFound);
    public static Error EmailAlreadyExists {get;} = new ("CONFLICT", "Email is already in use", ErrorType.Conflict);
    public static Error UserNameAlreadyExists {get;} = new ("CONFLICT", "Username is already in use", ErrorType.Conflict);
  }

}
