# TNT Services Copilot Instructions

This is a .NET 10.0 ASP.NET Core web application with a Razor Pages UI, API controllers, and a shared models library.

## Build, Test, and Lint Commands

### Build
```bash
dotnet build
```

### Run Tests
Run all tests:
```bash
dotnet test
```

Run a specific test class:
```bash
dotnet test --filter ClassName=AuthorizationControllerTests
```

Run a specific test method:
```bash
dotnet test --filter Name~Authorize_Valid
```

### Database Migrations
Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

Apply migrations to the database:
```bash
dotnet ef database update [-v]
```

Remove the last migration:
```bash
dotnet ef migrations remove
```

## Solution Structure

- **TNT.Services.Service** - Main ASP.NET Core web application (Razor Pages + API controllers)
  - `Controllers/` - API endpoints, each inheriting from ControllerBase with [ApiController] attribute
  - `Data/ApplicationDbContext.cs` - EF Core context inheriting from IdentityDbContext (manages Application, Release, Licensee, Analytic entities)
  - `Models/Entities/` - Domain models (all use Guid or int primary keys named "ID")
  - `Program.cs` - Service configuration including JWT authentication, Entity Framework setup, and database seeding
  - `appsettings.json` - Configuration including JWT settings (Jwt:Key, Jwt:Issuer, Jwt:Audience, Jwt:Subject)

- **TNT.Services.Models** - Shared models library (published as NuGet package)
  - `Dto/` - Data Transfer Objects
  - `Request/` - Request models for API calls
  - `Response/` - Response models and DtoResponse base class
  - `Exceptions/` - Custom exceptions
  - `JWT.cs` - JWT-related models

- **NUnitTests** - NUnit test project
  - `ContextDependentTest.cs` - Base class providing mock DbSet creation and test data (Applications, Releases, Licensees)
  - Test classes inherit from ContextDependentTests and use Moq to mock ApplicationDbContext
  - All tests marked with [ExcludeFromCodeCoverage]

- **Supporting Projects**
  - TNT.Services.Client - Client library
  - TNT.Updater - Update service
  - TestWithConsole - Console application for manual testing

## Key Conventions

### Code Style
- C# 12+ with nullable reference types enabled (`<Nullable>enable</Nullable>`)
- Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- XML documentation comments (`///`) on all public members
- Latest language version (`<LangVersion>latest</LangVersion>`)

### Controllers
- API routes use pattern: `[Route("api/[controller]/[action]")]`
- All inherit from ControllerBase with [ApiController] attribute
- Use dependency injection in constructor for ApplicationDbContext, IConfiguration, and utilities (DateTimeUtil, GuidUtil)
- Return ActionResult with proper HTTP status codes (OkObjectResult, BadRequestResult, NotFoundResult)

### Database & Models
- ApplicationDbContext inherits from IdentityDbContext (includes ASP.NET Identity tables)
- Entity primary keys named "ID" (Guid for most domain entities, int for Release)
- Entities are in TNT.Services.Service.Models.Entities namespace
- DbSets are virtual properties

### Testing
- Base class ContextDependentTests provides:
  - Mock Application, Release, and Licensee collections
  - `GetDbSet<T>(List<T>)` helper to create mockable DbSet from lists
- Tests use Moq for mocking DbContext and IConfiguration
- Constructor injection pattern for test utilities (DateTimeUtil, GuidUtil can be injected or auto-instantiated)

### Configuration
- JWT configuration keys: Jwt:Key, Jwt:Issuer, Jwt:Audience, Jwt:Subject (stored in Setting.cs)
- Connection string: DefaultConnection
- Stored in appsettings.json and User Secrets (for development)

### Database
- SQLite is used (Microsoft.EntityFrameworkCore.Sqlite)
- Database seeding happens at startup via SeedData.InitializeAsync()
- Migrations stored in Migrations/ folder

## Commit Message Guidelines

This project follows [Conventional Commits](https://www.conventionalcommits.org/) format:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- **feat** - A new feature
- **fix** - A bug fix
- **docs** - Documentation changes
- **style** - Code style changes (formatting, missing semicolons, etc.) - not affecting functionality
- **refactor** - Code refactoring without feature or fix
- **test** - Adding or updating tests
- **chore** - Build, dependencies, tooling updates
- **perf** - Performance improvements
- **ci** - CI/CD configuration changes

### Scope (optional)
Use the affected area: `api`, `models`, `db`, `auth`, `tests`, etc.

### Examples
```
feat(api): add new endpoint for license validation
fix(auth): correct JWT token expiration validation
test(controllers): add tests for AuthorizationController
docs: update database migration instructions
chore: upgrade Entity Framework to 10.0.7
refactor(models): simplify application entity structure
```

### Guidelines
- Use lowercase for type, scope, and subject
- Subject line: imperative mood ("add" not "added"), no period at end
- Keep subject under 50 characters
- Body should explain *why*, not *what* (code shows what)
- Reference related issues: `Closes #123` or `Relates to #456`

## Common Tasks

**Adding a new API endpoint:**
1. Create a controller in Controllers/ inheriting from ControllerBase with [ApiController]
2. Use [Route("api/[controller]/[action]")] and inject dependencies
3. Add corresponding DTOs/Request/Response models to TNT.Services.Models
4. Add corresponding NUnit tests inheriting from ContextDependentTests

**Adding a database entity:**
1. Create entity class in TNT.Services.Service.Models.Entities/
2. Add DbSet<T> property to ApplicationDbContext (virtual property)
3. Create and apply EF migration: `dotnet ef migrations add EntityName`
4. Add test data to ContextDependentTests base class

**Modifying request/response models:**
- Edit DTOs in TNT.Services.Models/Dto or Dto/Response
- Regenerate NuGet package: The project has GeneratePackageOnBuild enabled, publish to D:\NugetRepo
