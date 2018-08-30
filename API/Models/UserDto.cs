namespace API.Models
{
    public class UserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Info { get; set; }
        public bool IsAdmin { get; set; }
        
        public static UserDto Stub()
        {
            return new UserDto
            {
                Login = "johndoe",
                Password= "",
                FirstName = "John",
                LastName = "Doe",
                Info = "xxxxx",
                IsAdmin = false
            };
        }
    }
}