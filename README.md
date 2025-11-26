# Change Management System

A web application for managing change requests built with ASP.NET Core 6.0 MVC, featuring role-based access control and secure authentication.

## Features

### Security
- Role-based authorization (Admin and Employee roles)
- Strong password policies with account lockout
- CSRF protection on all forms
- Security headers (XSS, clickjacking, CSP protection)
- Secure cookie configuration
- Input validation throughout
- Entity Framework parameterized queries

### Application Features
- Create and manage change requests
- Track status (Pending, Approved, Rejected)
- Priority levels (Low, Medium, High, Critical)
- Admin approval workflow with reasons
- Audit trail with dates and submitter info
- Email notifications via SMTP
- Structured logging with Serilog

## Technology Stack

- ASP.NET Core 6.0 MVC
- SQL Server with Entity Framework Core 6
- ASP.NET Core Identity
- Serilog for logging
- Bootstrap 5, jQuery, DataTables

## Project Structure

```
ChangeManagement/
├── ChangeManagement/          # Main web application
│   ├── Controllers/           # MVC Controllers
│   ├── Views/                 # Razor views
│   ├── wwwroot/              # Static files (CSS, JS)
│   └── Program.cs            # Application entry point
├── Change.DataAccess/         # Data access layer
│   ├── Data/                 # DbContext
│   ├── Migrations/           # EF migrations
│   └── Repository/           # Repository pattern
├── Change.Models/             # Domain models
│   └── Models/               # Entity classes
└── Change.Utility/            # Shared utilities
    ├── EmailSender.cs        # Email service
    └── SD.cs                 # Static constants
```

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Karinateii/ChangeManagement.git
   cd ChangeManagement
   ```

2. **Configure User Secrets (Development)**
   ```bash
   cd ChangeManagement
   dotnet user-secrets init
   dotnet user-secrets set "AdminUser:Email" "admin@example.com"
   dotnet user-secrets set "AdminUser:Password" "SecureP@ssw0rd123!"
   dotnet user-secrets set "AdminUser:Name" "System Administrator"
   ```

3. **Configure Email Settings (Optional)**
   ```bash
   dotnet user-secrets set "EmailSettings:SmtpServer" "smtp.gmail.com"
   dotnet user-secrets set "EmailSettings:SmtpPort" "587"
   dotnet user-secrets set "EmailSettings:SenderEmail" "your-email@gmail.com"
   dotnet user-secrets set "EmailSettings:Username" "your-email@gmail.com"
   dotnet user-secrets set "EmailSettings:Password" "your-app-password"
   ```

4. **Update Database Connection String**
   
   Edit `appsettings.json` or use user secrets:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your-Connection-String"
   ```

5. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

6. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement
   ```

7. **Run the Application**
   ```bash
   dotnet run --project ChangeManagement
   ```

   Navigate to `https://localhost:7XXX` (check console output for exact port)

## Configuration

### Password Policy

Password requirements:
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 digit
- At least 1 special character
- Account lockout: 5 failed attempts, 15-minute lockout

### Authorization Policies

- **AdminOnly**: Admin role required
- **EmployeeOnly**: Employee role required
- **AdminOrEmployee**: Either role accepted

### Logging

Logs are written to:
- Console output
- `logs/changemanagement-YYYYMMDD.txt` (daily rolling files)

Log levels can be configured in `appsettings.json` under the `Serilog` section.

## Usage

### Roles

1. **Admin**: Full access
   - View all requests
   - Approve/reject requests
   - Delete requests
   - Add approval reasons and dates

2. **Employee**: Standard access
   - Create new requests
   - View all requests
   - Cannot approve/reject or delete

### Admin User Setup

The admin user is created automatically on first run using credentials from user secrets or appsettings.json.

### Email Notifications

To enable email notifications:
1. Configure SMTP settings in user secrets (recommended) or `appsettings.json`
2. For Gmail, use an App Password instead of your regular password
3. The EmailSender service will log warnings if email settings are not configured

## Security Best Practices Implemented

1. **Never store sensitive data in source control**
   - Use User Secrets for development
   - Use Azure Key Vault or environment variables for production

2. **Connection strings and credentials**
   - Stored in User Secrets (development)
   - Should use managed identities or secure vaults in production

3. **HTTPS enforcement**
   - Always enabled with HSTS headers in production

4. **Security headers**
   - X-Content-Type-Options: nosniff
   - X-Frame-Options: DENY
   - X-XSS-Protection: 1; mode=block
   - Content-Security-Policy configured
   - Referrer-Policy: strict-origin-when-cross-origin

5. **Input validation**
   - Server-side validation on all inputs
   - Data annotations on models
   - Maximum length constraints

## Database Schema

### Request Table
- `Id` (PK): Unique identifier
- `Title`: Request title (5-200 chars, required)
- `Description`: Detailed description (10-2000 chars, required)
- `Priority`: Low, Medium, High, or Critical (required)
- `Status`: Pending, Approved, or Not Approved
- `Date`: Submission timestamp (required)
- `SubmittedBy`: User who created the request (required)
- `AdminReason`: Admin's approval/rejection reason (max 1000 chars)
- `AdminApprovalDate`: Date when admin made decision

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in user secrets or appsettings.json
- Ensure database exists or migrations are applied

### Migration Errors
```bash
# Remove last migration
dotnet ef migrations remove --project Change.DataAccess --startup-project ChangeManagement

# Add new migration
dotnet ef migrations add MigrationName --project Change.DataAccess --startup-project ChangeManagement
```

### Email Not Sending
- Check SMTP settings configuration
- For Gmail, ensure "Less secure app access" is enabled or use App Passwords
- Check logs for detailed error messages

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is for educational and professional development purposes.

## Acknowledgments

- ASP.NET Core Team for the excellent framework
- Serilog contributors for structured logging
- Bootstrap and jQuery communities
