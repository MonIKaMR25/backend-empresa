# Copilot Instructions for backend-empresa

## Repository Overview
This is a backend application for a business/company management system built with .NET.

## Technology Stack
- **Framework**: .NET
- **Language**: C#
- **Project Type**: Backend/API

## Coding Standards

### General Guidelines
- Follow C# naming conventions (PascalCase for classes and methods, camelCase for local variables)
- Use meaningful and descriptive names for variables, methods, and classes
- Keep methods focused and single-purpose
- Add XML documentation comments for public APIs
- Use async/await for I/O operations

### Code Organization
- Organize code into logical namespaces
- Keep related functionality together
- Use dependency injection for managing dependencies
- Follow SOLID principles

### Error Handling
- Use try-catch blocks appropriately
- Log exceptions with sufficient context
- Throw specific exceptions, not generic ones
- Validate input parameters

## Testing Guidelines
- Write unit tests for business logic
- Use meaningful test names that describe what is being tested
- Follow Arrange-Act-Assert pattern
- Mock external dependencies in unit tests
- Aim for high code coverage on critical paths

## Build and Development

### Build Process
- Use standard .NET CLI commands (`dotnet build`, `dotnet run`, `dotnet test`)
- Ensure the project builds without warnings
- Run tests before committing changes

### Dependencies
- Keep NuGet packages up to date
- Document any new dependencies added
- Avoid adding unnecessary dependencies

## Git Workflow
- Write clear, descriptive commit messages
- Keep commits focused and atomic
- Follow conventional commit format when possible
- Create feature branches for new work

## API Development
- Use RESTful conventions for API endpoints
- Document API endpoints with proper XML comments or Swagger/OpenAPI
- Validate input data
- Return appropriate HTTP status codes
- Use DTOs for API request/response models

## Security Best Practices
- Never commit sensitive information (API keys, passwords, connection strings)
- Use configuration providers for sensitive data
- Validate and sanitize all user input
- Implement proper authentication and authorization
- Follow OWASP security guidelines

## Performance Considerations
- Use async/await for I/O-bound operations
- Implement proper caching strategies where appropriate
- Optimize database queries
- Use pagination for large data sets
- Profile and monitor application performance

## Additional Notes
- This is an evolving project - update these instructions as the project grows
- When in doubt, follow established .NET and C# best practices
- Consistency with existing code patterns is important
