namespace API.Models
{
    public class ErrorDto
    {
        public string Error { get; }

        public ErrorDto(string text)
        {
            Error = text;
        }
    }

    public class SuccessDto
    {
        public string Success { get; }

        public SuccessDto(string text)
        {
            Success = text;
        }
    }
}