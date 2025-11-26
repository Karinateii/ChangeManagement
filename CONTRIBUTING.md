# Contributing to Change Management System

First off, thank you for considering contributing to the Change Management System! It's people like you that make this project better.

## Code of Conduct

By participating in this project, you are expected to uphold our Code of Conduct:
- Be respectful and inclusive
- Welcome newcomers and help them learn
- Focus on what is best for the community
- Show empathy towards other community members

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to avoid duplicates. When you create a bug report, include as many details as possible:

**Bug Report Template:**
```markdown
**Describe the bug**
A clear and concise description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '....'
3. Scroll down to '....'
4. See error

**Expected behavior**
A clear description of what you expected to happen.

**Screenshots**
If applicable, add screenshots to help explain your problem.

**Environment:**
 - OS: [e.g. Windows 10]
 - Browser: [e.g. chrome, safari]
 - .NET Version: [e.g. 6.0]

**Additional context**
Add any other context about the problem here.
```

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion:

- Use a clear and descriptive title
- Provide a detailed description of the suggested enhancement
- Explain why this enhancement would be useful
- List any similar features in other applications if applicable

### Pull Requests

1. **Fork the repository** and create your branch from `master`
2. **Make your changes** following the coding standards
3. **Test your changes** thoroughly
4. **Update documentation** if needed
5. **Write good commit messages**
6. **Submit a pull request**

## Development Setup

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Setup Steps

1. **Clone your fork**
   ```bash
   git clone https://github.com/YOUR-USERNAME/ChangeManagement.git
   cd ChangeManagement
   ```

2. **Configure User Secrets**
   ```bash
   cd ChangeManagement
   dotnet user-secrets init
   dotnet user-secrets set "AdminUser:Email" "admin@example.com"
   dotnet user-secrets set "AdminUser:Password" "Admin@123456"
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(LocalDb)\\MSSQLLocalDb;Database=CHANGE_DEV;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

3. **Restore packages and apply migrations**
   ```bash
   cd ..
   dotnet restore
   dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement
   ```

4. **Run the application**
   ```bash
   dotnet run --project ChangeManagement
   ```

## Coding Standards

### C# Style Guide

- **Naming Conventions:**
  - PascalCase for classes, methods, and properties
  - camelCase for local variables and parameters
  - Prefix interfaces with `I` (e.g., `IRepository`)
  - Use descriptive names that explain intent

- **File Organization:**
  - One class per file
  - Namespace matches folder structure
  - Group related functionality together

- **Code Structure:**
  ```csharp
  // Good
  public class RequestRepository : Repository<Request>, IRequestRepository
  {
      private readonly ApplicationDbContext _db;
      
      public RequestRepository(ApplicationDbContext db) : base(db)
      {
          _db = db;
      }
      
      public async Task<Request> GetByIdAsync(int id)
      {
          return await _db.Requests.FindAsync(id);
      }
  }
  ```

- **Async/Await:**
  - Use async methods for database operations
  - Suffix async methods with `Async`
  - Always await async calls

- **Comments:**
  - Write self-documenting code
  - Comment complex logic
  - Use XML comments for public APIs

### Frontend Standards

- **HTML/Razor:**
  - Use semantic HTML5 elements
  - Keep views clean and simple
  - Use partial views for reusable components

- **CSS:**
  - Use Bootstrap classes when possible
  - Custom CSS goes in site.css
  - Use meaningful class names

- **JavaScript:**
  - Use jQuery sparingly
  - Prefer vanilla JavaScript when possible
  - Use `const` and `let`, avoid `var`
  - Format with consistent indentation

## Testing

### Unit Tests
- Write unit tests for business logic
- Use descriptive test names: `MethodName_Scenario_ExpectedResult`
- Follow AAA pattern: Arrange, Act, Assert

```csharp
[Fact]
public async Task GetById_ExistingId_ReturnsRequest()
{
    // Arrange
    var repository = new RequestRepository(_context);
    
    // Act
    var result = await repository.GetByIdAsync(1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result.Id);
}
```

### Integration Tests
- Test controller actions
- Test database operations
- Test authorization policies

## Database Changes

### Creating Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project Change.DataAccess --startup-project ChangeManagement

# Update database
dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement

# Remove last migration (if not applied)
dotnet ef migrations remove --project Change.DataAccess --startup-project ChangeManagement
```

### Migration Best Practices
- Use descriptive migration names
- Test migrations both up and down
- Don't delete old migrations that have been deployed
- Always review generated migration code

## Security Guidelines

### Never Commit Secrets
- Use User Secrets for development
- Use environment variables for production
- Never commit `appsettings.Production.json` with real credentials

### Input Validation
- Always validate user input on the server
- Use data annotations on models
- Sanitize HTML input if accepting rich text

### Authorization
- Use policy-based authorization
- Check permissions in controllers
- Never trust client-side checks alone

## Commit Message Guidelines

### Format
```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation only
- **style**: Code style changes (formatting, etc.)
- **refactor**: Code refactoring
- **test**: Adding tests
- **chore**: Maintenance tasks

### Examples
```
feat(request): add priority filter to request list

Added dropdown filter to allow users to filter requests by priority level.
Implements issue #123.

Closes #123
```

```
fix(security): prevent XSS in request description

Escaped HTML in request description display to prevent XSS attacks.
Added validation to reject script tags in descriptions.
```

## Pull Request Process

1. **Update your fork** with the latest from master
   ```bash
   git checkout master
   git pull upstream master
   git push origin master
   ```

2. **Create a feature branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```

3. **Make your changes** and commit
   ```bash
   git add .
   git commit -m "feat: add amazing feature"
   ```

4. **Push to your fork**
   ```bash
   git push origin feature/amazing-feature
   ```

5. **Create Pull Request** on GitHub
   - Use a clear title
   - Describe what changes you made and why
   - Reference any related issues
   - Add screenshots if applicable

6. **Address review comments**
   - Make requested changes
   - Push updates to the same branch
   - Respond to reviewer comments

## Review Process

- All pull requests require review before merging
- Reviewers will check:
  - Code quality and style
  - Test coverage
  - Security implications
  - Documentation updates
  - Breaking changes

## Release Process

1. Version numbers follow [Semantic Versioning](https://semver.org/)
2. CHANGELOG.md is updated for each release
3. Releases are tagged in Git
4. Release notes are published on GitHub

## Questions?

Feel free to:
- Open an issue with your question
- Contact the maintainer
- Join our discussions

## Recognition

Contributors will be recognized in:
- README.md
- Release notes
- Contributors page

Thank you for contributing! 🎉
