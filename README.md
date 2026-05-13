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
  - **Application/Services/**: Business logic layer with interfaces and implementations, including `AuthService`, `ReservationService`, and `AdminReservationService`.
  - **Application/Services/Auth/Tokens/**: Handles token related logic, such as `AuthToken`.
  - **Application/Services/Reservations/Capacity/**: Manages reservation capacity logic.
  - **Application/Jobs/**: Background jobs managed by Hangfire.
  - **Domain/Models/**: Entity models like `Reservation`, `Resource`, `User`, and `RefreshToken`.
  - **API/Middleware/**: Custom middleware like `ErrorHandlingMiddleware`.
  - **API/Controllers/**: Handles API endpoints (e.g., `AuthController`, `ReservationController`, `ResourceController`).

### Tests
- **tests/**: Contains unit tests for the application.

## Key Files

### Docker
- **Dockerfile**: Builds the application into a Docker image.
  ```dockerfile
  FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
  WORKDIR /src
  COPY src/*.csproj ./
  RUN dotnet restore ./ExpenseTracker.csproj
  COPY . .
  RUN dotnet publish -c Release -o /app/publish

  FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
  WORKDIR /app
  COPY --from=build /app/publish .
  EXPOSE 8080
  ENTRYPOINT ["dotnet", "ExpenseTracker.dll"]
  ```

- **docker-compose.yml**: Defines services for the application and database.
  ```yaml
  version: "3.9"

  services:
    api:
      image: expense-tracker-api:latest
      build:
        context: .
        dockerfile: Dockerfile
      container_name: expense-tracker-api
      ports:
        - "8080:8080"
      environment:
        DB_CONNECTION_STRING: Host=host.docker.internal;Port=5432;Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Database=${POSTGRES_DB}
        SECRET_KEY: ${SECRET_KEY}
        ISSUER: ${ISSUER}
        AUDIENCE: ${AUDIENCE}
        OPENAI_API_KEY: ${OPENAI_API_KEY}
        ASPNETCORE_URLS: "http://0.0.0.0:8080"
        ASPNETCORE_ENVIRONMENT: "Production"
      depends_on:
        db: 
          condition: service_healthy

    db:
      image: postgres:16
      container_name: expense-tracker-postgres
      environment:
        POSTGRES_USER: ${POSTGRES_USER}
        POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
        POSTGRES_DB: ${POSTGRES_DB}
      ports:
        - "5432:5432"
      volumes:
        - pg_data:/var/lib/postgresql/data
      healthcheck:
        test: ["CMD-SHELL", "pg_isready -U ${POSTGRES_USER}"]
        interval: 5s
        timeout: 3s
        retries: 5

  volumes:
    pg_data:
  ```
  
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
   
## Testing

### Unit Tests
- Located in the `tests/UnitTests/` directory.
- Run tests using the following command:
  ```bash
  dotnet test
  ```

## License
This project is licensed under the MIT License.