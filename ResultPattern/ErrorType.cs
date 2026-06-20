namespace TestAPI.ResultPattern
{
    public enum ErrorType
    {
        InternalServerError,
        NotFound,
        Unauthorized,
        Forbidden,
        Conflict,
        UnprocessableEntity,
        Validation
    }
}
