namespace TestAPI.Services.Implementation
{
    public class AuthSettings
    {

        public string? Issuer {get;set;}

        public string? Audience {get;set;}
        public TimeSpan Expires { get; set; }
        public string? SecretKey { get; set; }


    }
}
