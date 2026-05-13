# Expense Tracker

## Overview
Expense Tracker is a robust application designed to manage and analyze your expenses. It provides features such as expense tracking, subscription management, analytics, and authentication.

## Project Structure

### Solution
- **ExpenseTracker.sln**: The main solution file containing the ExpenseTracker and UnitTests projects.

### Source Code
- **src/**: Contains the main application code.
  - **API/**: Includes controllers for handling API requests.
  - **Application/**: Contains DTOs, jobs, mappers, and services.
  - **Domain/**: Defines models, exceptions, and domain services.
  - **Infrastructure/**: Handles persistence, security, and other infrastructure concerns.

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

### Configuration
- **appsettings.json**: Default application settings.
  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*"
  }
  ```

- **appsettings.Development.json**: Development-specific settings.
  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    }
  }
  ```

## How to Run

### Prerequisites
- .NET 8.0 SDK
- Docker
- PostgreSQL

### Steps
1. Clone the repository.
2. Build the Docker image:
   ```bash
   docker-compose up --build
   ```
3. Access the API at `http://localhost:8080`.

## Testing

### Unit Tests
- Located in the `tests/UnitTests/` directory.
- Run tests using the following command:
  ```bash
  dotnet test
  ```

## License
This project is licensed under the MIT License.