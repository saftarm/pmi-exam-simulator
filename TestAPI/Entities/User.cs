namespace TestAPI.Entities
{
    public class User
    {

        public int Id { get; set; }

        public string? FirstName {get; set;}

        public string? UserName {get; set;}

        public string? DisplayName { get; set; }

        public string? Email {get;set; }

        public string? PasswordHash {get;set;}

        public ICollection<ExamAttempt>? ExamAttempts {get;set;}



        


    }
}
