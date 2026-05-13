# Expense Tracker
![CI](https://github.com/tamarques13/expense-tracker/actions/workflows/dotnet-ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-blue.svg)

## Overview
Expense Tracker is a robust application designed to manage and analyze your expenses. It provides features such as expense tracking, subscription management, analytics, and authentication.

## Project Structure

## Features
- **JWT Authentication**: Secure user authentication and authorization.
- **Refresh Tokens**: Extend user sessions securely with refresh token support.
- **Hangfire Integration**: Background job processing for tasks like checking subscriptions renewal day.
- **PostegreSQL Server Support**: Robust database integration for managing expenses, subscriptions and analytics.
- **AI Integration**: Support system to extract data from receipt images. 
- **Security Utilities**: Includes password hashing and token generation utilities.
- **Modular Architecture**: Organized into controllers, services, repositories and DTOs for maintainability.

### Solution
- **ExpenseTracker.sln**: The main solution file containing the ExpenseTracker and UnitTests projects.

### Source Code
- **src/**: Contains the main application code.
  - **Infrastructure/Persistence/**: Contains the `AppDbContext` for database interactions.
  - **Infrastructure/Security/**: Security utilities like `PasswordHasher` and `TokenGenerater`.
  - **Infrastructure/Migrations/**: Entity Framework migrations for database schema.
  - **Application/DTOs/**: Data Transfer Objects for API requests and responses.
  - **Application/Services/**: Business logic layer with interfaces and implementations, including `ExpenseService`, `SubscriptionService`, and `ReceiptLlmService`.
  - **Application/Services/Auth/Tokens/**: Handles token related logic, such as `AuthToken`.
  - **Application/Jobs/**: Background jobs managed by Hangfire.
  - **Domain/Models/**: Entity models like `Expense`, `Subscription`, `User`, and `RefreshToken`.
  - **API/Middleware/**: Custom middleware like `ErrorHandlingMiddleware`.
  - **API/Controllers/**: Handles API endpoints (e.g., `AuthController`, `ExpenseController`, `AiController`).

### Tests
- **tests/**: Contains unit tests for the application.

## How to Run

### Prerequisites
- .NET SDK 8.0 or later
- PostegreSQL Server
- Docker
- Git Bash (for generating secret keys)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/tamarques13/expense-tracker.git
   cd expense-tracker
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Configure environment variables in a `.env` file:
   ```env
   DB_CONNECTION_STRING=your-database-connection-string
   POSTGRES_DB=your-postgres-db
   POSTGRES_USER=your-postgres-user
   POSTGRES_PASSWORD=your-postgres-password
   SECRET_KEY=your-secret-key (see below for generating one)
   ISSUER=your-jwt-issuer
   AUDIENCE=your-jwt-audience
   OPENAI_API_KEY=your-openai-key
   ```
   - `DB_CONNECTION_STRING`: Connection string for your SQL Server database.
   - `POSTGRES_DB`: Value used as the name of Database
   - `POSTGRES_USER`: Value used as teh name of the User to connect to Database
   - `POSTGRES_PASSWORD`: Value used as teh name of the Password to connect to Database
   - `SECRET_KEY`: A secure key for JWT signing (see the "Generate Environment Secret Key" section below).
   - `ISSUER`: The issuer of the JWT (e.g., your API name).
   - `AUDIENCE`: The audience for the JWT (e.g., your client application).
   - `OPENAI_API_KEY`: A secure key to connect to OpenAI

4. Apply migrations to the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```

## Usage
- Use tools like Postman or Swagger to test the API endpoints.
- Example endpoints:
  - `POST /api/v1/auth/login`: Authenticate and retrieve a JWT.
  - `GET /api/v1/expenses`: Fetch all expenses for a user.
  - `POST /api/v1/expenses`: Create a new expense for a user.
  - `GET /api/v1/subscriptions`: Fetch all subscriptions for a user.
  - `POST /api/v1/subscriptions`: Create a new subscription for a user.
  - `GET /api/v1/analytics`: Retrieve analytics data for expenses and subscriptions.

## Testing

### Unit Tests
- Located in the `tests/UnitTests/` directory.
- Run tests using the following command:
  ```bash
  dotnet test
  ```

### Continuous Integration
This project uses **GitHub Actions** to automatically build and test the application.

The CI pipeline runs on:
- Pull requests  

The workflow performs:
- Restore dependencies  
- Build the project  
- Run unit tests  

## Generate Environment Secret Key (Git Bash)

Copy and run the command below in Git Bash to generate a secure secret key:

```
openssl rand -base64 64
```
The output will be a random base64 string (64 characters).

## License
This project is licensed under the MIT License.

## Database Schema
The database schema includes the following entities:

- **Users**:
  - Fields: `Id`, `Email`, `Password`, `FirstName`, `LastName`, `Currency`.
  - Relationships: One-to-Many with `Expenses` and `Subscriptions`.
- **Expenses**:
  - Fields: `Id`, `Category`, `Amount`, `CreatedAt`, `UserId`.
  - Relationships: Many-to-One with `Users`.
- **Subscriptions**:
  - Fields: `Id`, `Name`, `Category`, `Amount`, `RenewDay`, `ExpireDay`, `IsActive`, `UserId`, `LastGenerated`.
  - Relationships: Many-to-One with `Users`.
- **RefreshTokens**:
  - Fields: `Id`, `UserId`, `Token`, `ExpireDate`, `CreatedAt`, `RevokedAt`, `ReplacedByToken`, `IsRevoked`, `IpAddress`.
  - Relationships: Many-to-One with `Users`.

## API Documentation (Swagger)
Swagger is integrated into the project:

- **Setup**:
  - Swagger is configured in `Program.cs` with security definitions for JWT.
  - Swagger UI is accessible at `/swagger` in development mode.
- **Usage**:
  - Provides API documentation for all endpoints.
  - Includes JWT authentication details for secured endpoints.

## Advanced Features

- **Refresh Tokens**:
  - Tokens are generated during login and stored in the database.
  - Tokens are validated for expiration and reuse detection.
  - Token rotation is implemented to enhance security.
- **AI Integration**:
  - Extracts text from receipts images in the right format to create expenses.
- **Hangfire Jobs**:
  - Jobs are scheduled to check for subscriptions renewal day, to create expenses in the correct day.

## Contributing Guidelines

- **Code Style**:
  - Follow C# conventions and use meaningful variable names.
- **Pull Requests**:
  - Create a new branch for each feature or bug fix.
  - Ensure all tests pass before submitting a pull request.
- **Testing**:
  - Write unit tests for new features.
  - Run `dotnet test` to verify changes.
