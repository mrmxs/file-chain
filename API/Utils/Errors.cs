using API.Models;

namespace API.Utils
{
    public static class Errors
    {
        public static ErrorDto WRONG_CREDENTIALS = new ErrorDto("Wrong credentials");
        public static ErrorDto FILE_DOES_NOT_EXISTS = new ErrorDto("File doesn't exists");
        public static ErrorDto INSUFFICIENT_PRIVILEGES = new ErrorDto("Insufficient privileges");
        public static ErrorDto INDEX_DOES_NOT_EXISTS = new ErrorDto("Index doesn't exists");
        public static ErrorDto LOGIN_ALREADY_EXISTS = new ErrorDto("Login already exists");
        public static ErrorDto REQUIRED_FIELDS_ARE_MISSING = new ErrorDto("Required fields are missing");
    }
}