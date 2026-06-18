namespace TestAPI.Exceptions
{
    public class CorruptedFileException : Exception
    {
        public CorruptedFileException(string message) : base(message)
        {

        }
    }
}
