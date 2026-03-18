using Spentir.Repositories.Interfaces;
using Spentir.Services.Interfaces;
using Spentir.Models;
using Spentir.Security;
using Spentir.Helpers;
using Spentir.DTOs;

namespace Spentir.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        /// <summary>
        /// Creates a new user account after validating that the email is not already registered.
        /// The password is securely hashed before storing the user information in the repository.
        /// </summary>
        /// <param name="dto">The data transfer object containing user registration details.</param>
        /// <returns>
        /// A <see cref="UserTokenDto"/> containing the generated bearer authentication token for the newly created user.
        /// </returns>
        /// <exception cref="DomainException">Thrown when the email is already registered.</exception>

        public async Task<UserTokenDto> CreateUser(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null) throw new DomainException("Email is already registered");

            var hashPassword = PasswordHasher.HashPassword(dto.Password);
            var user = new User(dto.Email, hashPassword, dto.FirstName, dto.LastName, dto.Currency);

            await _userRepository.AddAsync(user);
            var bearerToken = TokenGenerator.GenerateBearerToken(user);

            return new UserTokenDto
            {
                Token = "Bearer " + bearerToken
            };
        }

        /// <summary>
        /// Authenticates a user by verifying the provided password against the stored hashed password.
        /// If authentication is successful, a bearer token is generated and returned.
        /// </summary>
        /// <param name="dto">The data transfer object containing login credentials.</param>
        /// <returns>
        /// A <see cref="UserTokenDto"/> containing the generated bearer authentication token.
        /// </returns>
        /// <exception cref="KeyNotFoundException">Thrown when the user with the specified email is not found.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the password is invalid.</exception>

        public async Task<UserTokenDto> LoginUser(LoginUserDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email) ?? throw new KeyNotFoundException($"User with email: {dto.Email} not found.");

            var isValidPassword = PasswordHasher.VerifyPassword(dto.Password, user.Password);

            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid email or password");

            var bearerToken = TokenGenerator.GenerateBearerToken(user);

            return new UserTokenDto
            {
                Token = "Bearer " + bearerToken
            };
        }

    }
}