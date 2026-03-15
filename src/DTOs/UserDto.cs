namespace Spentir.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Currency { get; set; } = "EUR";
    }

    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = "John";
        public string LastName { get; set; } = "Doe";
        public string Currency { get; set; } = "EUR";
    }

    public class LoginUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserTokenDto
    {
        public string Token { get; set; } = string.Empty;
    }
}