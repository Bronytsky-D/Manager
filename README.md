# Task Manager API

A modern, secure web API for task management built with .NET 8. This project demonstrates clean architecture principles, JWT authentication, and containerized deployment suitable for portfolio and learning purposes.

## 🚀 Features

- **User Authentication & Authorization** - JWT-based security
- **CRUD Operations** - Complete task management functionality
- **Database Integration** - PostgreSQL with Entity Framework Core
- **Clean Architecture** - Separation of concerns with multiple layers
- **Containerization** - Docker support for easy deployment
- **API Documentation** - Interactive Swagger UI
- **CI/CD Pipeline** - GitHub Actions integration

## 🏗️ Architecture

The project follows Clean Architecture principles with the following layers:

```
Manager/
├── Manager.API/              # Controllers, API configuration, entry point
├── Manager.Application/      # Business logic, services, use cases
├── Manager.Domain/          # Entities, enums, value objects, interfaces
├── Manager.Infrastructure/  # EF Core, repositories, data access, migrations
├── Manager.Common/          # DTOs, shared utilities, cross-cutting concerns
├── docker-compose.yml       # Container orchestration
└── README.md               # This file
```

## 📋 Prerequisites

Before running the application, ensure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for containerized setup)
- [Git](https://git-scm.com/downloads)
- PostgreSQL (if running locally without Docker)

## 🚀 Quick Start

### Option 1: Docker (Recommended)

1. **Clone the repository**
   ```bash
   git clone https://github.com/Bronytsky-D/Manager.git
   cd Manager
   ```

2. **Build and run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

3. **Access the application**
   - API Base URL: `http://localhost:5000`
   - Swagger Documentation: `http://localhost:5000/swagger`

### Option 2: Local Development

1. **Clone and navigate to project**
   ```bash
   git clone https://github.com/Bronytsky-D/Manager.git
   cd Manager
   ```

2. **Configure database connection**
   
   Update `appsettings.Development.json` or set environment variables (see Configuration section).

3. **Apply database migrations**
   ```bash
   cd Manager.Infrastructure.PostgreSQL
   dotnet ef database update --project Manager.Infrastructure.PostgreSQL --startup-project ../Manager
   ```

4. **Run the application**
   ```bash
   cd ../Manager.API
   dotnet run
   ```

## ⚙️ Configuration

### Application Settings

Configure the following settings in `appsettings.json` or via environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=manager;Username=postgres;Password=yourpassword"
  },
  "JWTSetting": {
    "SecretKey": "your-secret-key-must-be-at-least-32-characters-long",
    "Issuer": "task-manager-api",
    "Audience": "task-manager-users",
    "ExpiresInMinutes": 60
  },
   "Serilog": {
    "Using": [
      "Serilog.Sinks.Console"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithProcessId"
    ]
  },
  "AllowedHosts": "*"
  }
}
```

### Environment Variables (Production)

For production deployments, use environment variables instead of storing secrets in configuration files:

- `ConnectionStrings__DefaultConnection` 
- `JWTSetting__SecretKey`
- `JWTSetting__Issuer`
- `JWTSetting__Audience`
- `JWTSetting__ExpiresInMinutes`

## 🗄️ Database Management

### Migrations

The project uses Entity Framework Core for database management. Migration files are stored in `Manager.Infrastructure.PostgreSQL/Migrations/`.

**Add a new migration:**
```bash
dotnet ef migrations add <MigrationName> --project Manager.Infrastructure.PostgreSQL --startup-project Manager
```

**Apply migrations:**
```bash
dotnet ef database update --project Manager.Infrastructure --startup-project Manager
```

## 🔐 Authentication & Authorization

The API uses JWT Bearer tokens for authentication. All task-related endpoints require authentication.

### Authentication Endpoints

| Method | Endpoint | Description | Authentication Required |
|--------|----------|-------------|------------------------|
| `POST` | `/users/register` | Register new user | No |
| `POST` | `/users/login` | Login user | No |

### Example: User Registration
```bash
curl -X POST http://localhost:5000/users/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "johndoe",
    "email": "john@example.com",
    "password": "SecurePassword123!"
  }'
```

### Example: User Login
```bash
curl -X POST http://localhost:5000/users/login \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "johndoe",
    "password": "SecurePassword123!"
  }'
```

The login endpoint returns a JWT token in the response body. Use this token in the Authorization header for subsequent requests.

## 📝 API Endpoints

### Task Management

All task endpoints require authentication via JWT Bearer token.

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/tasks` | Get all user tasks (with filtering & pagination) |
| `GET` | `/tasks/{id}` | Get specific task by ID |
| `POST` | `/tasks` | Create new task |
| `PUT` | `/tasks/{id}` | Update existing task |
| `DELETE` | `/tasks/{id}` | Delete task |

### Example: Get User Tasks
```bash
curl -H "Authorization: Bearer <your-jwt-token>" \
  "http://localhost:5000/tasks?page=1&pageSize=10&status=pending"
```

### Example: Create Task
```bash
curl -X POST http://localhost:5000/tasks \
  -H "Authorization: Bearer <your-jwt-token>" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Complete project documentation",
    "description": "Update README and add API documentation",
    "dueDate": "2024-12-31T23:59:59Z",
    "priority": "High"
  }'
```

For complete API documentation with request/response schemas, visit the Swagger UI at `http://localhost:5000/swagger`.

## 🧪 Testing

Run the test suite:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test Manager.Tests
```

## 🔄 CI/CD Pipeline

The project includes GitHub Actions workflow for:

- **Build & Test**: Runs on pull requests and pushes
- **Docker Build**: Optional Docker image building and publishing
- **Code Quality**: Static analysis and security scanning

Expected workflow jobs:
- `build` - Restore dependencies and build solution
- `test` - Run unit and integration tests  
- `docker-build` - Build and push Docker image (optional)

## 🛡️ Security Considerations

### Development
- JWT secret key must be at least 32 characters for HMAC-SHA256
- Passwords are hashed using bcrypt algorithm
- User ownership validation for all task operations
- CORS configuration for cross-origin requests

### Production Checklist
- [ ] Use HTTPS/TLS encryption
- [ ] Store secrets in secure key management system
- [ ] Implement rate limiting
- [ ] Enable security headers
- [ ] Set up monitoring and logging
- [ ] Configure firewall rules
- [ ] Regular security updates

## 🐛 Troubleshooting

### Common Issues

**401 Unauthorized on protected endpoints:**
- Verify JWT token is valid and not expired
- Check token signature, Issuer, and Audience settings
- Ensure Authorization header format: `Bearer <token>`

**Database connection errors:**
- Verify PostgreSQL is running
- Check connection string parameters (host, port, credentials)
- Ensure PostgreSQL accepts TCP connections
- Validate database and user permissions

**Docker issues:**
- Ensure Docker Desktop is running
- Check port 5000 is not already in use
- Clear Docker cache: `docker system prune`

### Logs

Check application logs for detailed error information:
```bash
# Docker logs
docker-compose logs api

# Local development
dotnet run --verbosity diagnostic
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request


## 📞 Support

If you have questions or need support:
- Create an [Issue](https://github.com/Bronytsky-D/Manager/issues)

---

⭐ **If you find this project helpful, please consider giving it a star!** ⭐
