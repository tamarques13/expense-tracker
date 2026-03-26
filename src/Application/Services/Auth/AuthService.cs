using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Auth.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Infrastructure.Security;
using Spentir.Domain.Exceptions;
using Spentir.Application.DTOs;

namespace Spentir.Application.Services.Auth
{
    /// <summary>
    /// Application layer service responsible for user registration and authentication.
    /// Handles email uniqueness validation, secure password hashing, credential verification
    /// and bearer token generation. Acts as the main entry point for user account creation
    /// and login operations.
    /// </summary>

    public class AuthService(IAuthToken authToken, IAuthRepository authRepository, IRefreshTokenRepository refreshTokenRepository) : IAuthService
    {
        private readonly IAuthToken _authToken = authToken;
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

        /// <summary>
        /// Creates a new user account after validating that the email is not already registered.
        /// The password is securely hashed before storing the user information in the repository.
        /// </summary>
        /// <param name="dto">The data transfer object containing user registration details.</param>
        /// <returns>
        /// A <see cref="UserTokenDto"/> containing the generated bearer authentication token for the newly created user.
        /// </returns>
        /// <exception cref="DomainException">Thrown when the email is already registered.</exception>

        public async Task<UserTokenDto> CreateUserAsync(CreateUserDto dto, string ipAddress)
        {
            var existingUser = await _authRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null) throw new DomainException("Email is already registered");

            var hashPassword = PasswordHasher.HashPassword(dto.Password);

            var user = new User(dto.Email, hashPassword, dto.FirstName, dto.LastName, dto.Currency);
            var refreshToken = new RefreshToken(user.Id, Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(7), ipAddress);

            await _authRepository.AddAsync(user);
            await _refreshTokenRepository.AddAsync(refreshToken);

            var bearerToken = TokenGenerator.GenerateBearerToken(user);

            return new UserTokenDto
            {
                AccessToken = "Bearer " + bearerToken,
                RefreshToken = refreshToken.Token
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

        public async Task<UserTokenDto> LoginUserAsync(LoginUserDto dto, string ipAddress)
        {
            var user = await _authRepository.GetByEmailAsync(dto.Email) ?? throw new KeyNotFoundException($"User with email: {dto.Email} not found.");

            var isValidPassword = PasswordHasher.VerifyPassword(dto.Password, user.Password);

            if (!isValidPassword) throw new UnauthorizedAccessException("Invalid email or password");

            var refreshToken = new RefreshToken(user.Id, Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(7), ipAddress);
            var bearerToken = TokenGenerator.GenerateBearerToken(user);

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new UserTokenDto
            {
                AccessToken = "Bearer " + bearerToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<UserTokenDto> RotateRefreshTokenAsync(string Token, string ipAddress)
        {
            var oldToken = await _refreshTokenRepository.GetByTokenAsync(Token) ?? throw new KeyNotFoundException($"Token: {Token} not found.");

            await _authToken.ValidateRefreshTokenAsync(oldToken);

            oldToken.RevokedAt = DateTime.UtcNow;
            oldToken.IsRevoked = true;

            var user = await _authRepository.GetByIdAsync(oldToken.UserId) ?? throw new KeyNotFoundException($"User with Id: {oldToken.UserId} not found.");
            var refreshToken = new RefreshToken(oldToken.UserId, Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(7), ipAddress);

            oldToken.ReplacedByToken = refreshToken.Token;

            await _refreshTokenRepository.UpdateAsync(oldToken);
            await _refreshTokenRepository.AddAsync(refreshToken);

            var bearerToken = TokenGenerator.GenerateBearerToken(user);

            return new UserTokenDto
            {
                AccessToken = "Bearer " + bearerToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task LogOutAsync(Guid userId)
        {
            await _authToken.RevokeAllTokensForUser(userId);
        }
    }
}


