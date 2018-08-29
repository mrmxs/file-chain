namespace API.Models.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string EthereumWallet { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public static UserDto Stub()
        {
            return new UserDto
            {
                Id = 125,
                EthereumWallet = "xxxxx",
                FirstName = "John",
                LastName = "Doe"
            };
        }
    }
}