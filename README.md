# ToDoList

A feature-rich ASP.NET Core Todo application built using .NET 8, Entity Framework Core, and Identity for authentication. This project implements a modern RESTful API with robust CRUD operations, caching, JWT authentication, and comprehensive testing.

## Features

### Core Functionality
- ✅ Complete CRUD operations (Create, Read, Update, Delete)
- ✅ Toggle todo item completion status
- ✅ Save/load todos to/from files
- ✅ Validation using FluentValidation
- ✅ Caching with Memory Cache
- ✅ Swagger API documentation with annotations

### Authentication
- ✅ User registration and login
- ✅ JWT token authentication
- ✅ Identity framework integration
- 🔜 Social login (planned feature)

### Architecture
- ✅ Clean architecture with separation of concerns
- ✅ Repository pattern
- ✅ Dependency injection
- ✅ AutoMapper for object mapping
- ✅ Entity Framework Core with SQL Server
- ✅ Comprehensive error handling
- ✅ Unit and integration testing support

### Deployment
- ✅ Docker support
- ✅ GitHub Actions CI/CD pipeline for Azure deployment

## Technical Stack

- **Backend**: ASP.NET Core 8
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: JWT (JSON Web Tokens)
- **Documentation**: Swagger with annotations
- **Validation**: FluentValidation
- **Testing**: xUnit
- **Caching**: Memory Cache
- **Serialization**: System.Text.Json
- **CI/CD**: GitHub Actions

## Project Structure

```
ToDoList/
├── ToDoList/                  # Main project
│   ├── Controllers/           # API endpoints
│   ├── Models/                # Domain models
│   ├── DTOs/                  # Data transfer objects
│   ├── Services/              # Business logic
│   ├── Repositories/          # Data access
│   ├── Validators/            # Input validation
│   ├── Migrations/            # Database migrations
│   ├── Mappings/              # AutoMapper profiles
│   ├── Data/                  # Database context and configuration
│   ├── Exceptions/            # Custom exceptions
│   └── Helpers/               # Utility classes
└── ToDoList.Tests/            # Test project
    ├── Controllers/           # Controller tests
    ├── Models/                # Model tests
    ├── Integration/           # Integration tests
    ├── Services/              # Service tests
    └── Repository/            # Repository tests
```

## API Endpoints

The API includes the following endpoints:

### Authentication
- `POST /api/Auth/register` - Register a new user
- `POST /api/Auth/login` - Login with user credentials

### Todo Operations
- `GET /api/Todo` - Get all todo items
- `GET /api/Todo/{id}` - Get a specific todo item
- `POST /api/Todo` - Create a new todo item
- `PUT /api/Todo/{id}` - Update a todo item
- `DELETE /api/Todo/{id}` - Delete a todo item
- `PATCH /api/Todo/{id}/toggle` - Toggle completion status
- `POST /api/Todo/save` - Save todos to a file
- `POST /api/Todo/load` - Load todos from a file

## Setting Up the Project

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or later / VS Code

### Database Setup
1. Update the connection string in `appsettings.json` if needed
2. Run migrations to create the database:
   ```
   dotnet ef database update
   ```

### Run the Application
```bash
# Clone the repository
git clone https://github.com/yourusername/ToDoList.git

# Navigate to the project directory
cd ToDoList

# Restore packages
dotnet restore

# Run the application
dotnet run --project ToDoList
```

The API will be available at:
- HTTP: http://localhost:5058
- HTTPS: https://localhost:7273
- Swagger UI: http://localhost:5058/swagger

## Deploying to Azure

This project includes a GitHub Actions workflow for automatic deployment to Azure. To use it, you need to:

1. Create an Azure Virtual Machine
2. Set up the following GitHub secrets:
   - `AZURE_VM_IP` - The IP address of your Azure VM
   - `AZURE_VM_USERNAME` - Your VM username
   - `SSH_PRIVATE_KEY` - Your SSH private key
   - `PROJECT_NAME` - The name of your project
   - `APP_PORT` - The port your application will run on

The workflow will automatically:
- Build and test the application
- Publish the application
- Deploy to your Azure VM
- Configure Nginx as a reverse proxy
- Set up a systemd service for your application

## Testing

Run the tests with:

```bash
dotnet test
```

## Docker Support

Build and run with Docker:

```bash
# Build the Docker image
docker build -t todolist .

# Run the container
docker run -p 8080:8080 todolist
```

## License

This project is available as open source under the terms of the MIT License.

## To Do

- Add social login (Google, Microsoft, etc.)
- Implement user-specific todo items
- Add more comprehensive test coverage
- Enhance UI/frontend components
