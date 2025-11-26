# Change Management System

A professional ASP.NET Core 6.0 MVC application for managing change requests with role-based access control, secure authentication, and comprehensive logging.

## Features

### Security Features
- ✅ **Role-Based Authorization**: Admin and Employee roles with policy-based access control
- ✅ **Strong Password Policies**: Enforced password complexity requirements
- ✅ **Anti-CSRF Protection**: ValidateAntiForgeryToken on all POST operations
- ✅ **Security Headers**: XSS, clickjacking, and content-type sniffing protection
- ✅ **Secure Cookies**: HttpOnly, Secure, and SameSite cookie attributes
- ✅ **Input Validation**: Server-side validation with data annotations
- ✅ **SQL Injection Protection**: Entity Framework parameterized queries
- ✅ **Account Lockout**: Automatic lockout after failed login attempts

### Application Features
- **Request Management**: Create, view, edit, and delete change requests
- **Status Tracking**: Pending, Approved, and Not Approved statuses
- **Priority Levels**: Low, Medium, High, and Critical prioritization
- **Admin Approval Workflow**: Admins can approve/reject with reasons
- **Audit Trail**: Submission date, approval date, and submitter tracking
- **Email Notifications**: SMTP-based email sending capability
- **Structured Logging**: Serilog with file and console output

## Technology Stack

- **Framework**: ASP.NET Core 6.0 MVC
- **Database**: SQL Server with Entity Framework Core 6
- **Authentication**: ASP.NET Core Identity
- **Logging**: Serilog
- **Frontend**: Bootstrap 5, jQuery, DataTables

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

The following password requirements are enforced:
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 digit
- At least 1 non-alphanumeric character
- Account lockout after 5 failed attempts (15-minute lockout)

### Authorization Policies

- **AdminOnly**: Requires Admin role
- **EmployeeOnly**: Requires Employee role
- **AdminOrEmployee**: Requires either Admin or Employee role

### Logging

Logs are written to:
- Console output
- `logs/changemanagement-YYYYMMDD.txt` (daily rolling files)

Log levels can be configured in `appsettings.json` under the `Serilog` section.

## Usage

### Default Roles

1. **Admin**: Full access to all features
   - View all requests
   - Approve/reject requests
   - Delete requests
   - Add admin reasons and approval dates

2. **Employee**: Standard user access
   - Create new requests
   - View all requests
   - Cannot approve/reject or delete requests

### Creating the First Admin User

The admin user is automatically created on first run if configured in user secrets or `appsettings.json`. See the Installation section above.

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
