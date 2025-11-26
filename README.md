# Change Management System

[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-6.0-512BD4)](https://docs.microsoft.com/aspnet/core)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.0-512BD4)](https://docs.microsoft.com/ef/core/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A secure, enterprise-grade web application for managing organizational change requests built with ASP.NET Core 6.0 MVC. Features role-based authorization, comprehensive security measures, and a responsive user interface for streamlined change management workflows.

## 📸 Screenshots

![Dashboard](https://via.placeholder.com/800x400/0d6efd/ffffff?text=Change+Management+Dashboard)
*Main dashboard showing all change requests*

![Request Form](https://via.placeholder.com/800x400/198754/ffffff?text=New+Request+Form)
*Create new change request form*

## ✨ Features

### 🔒 Security
- **Role-Based Authorization**: Admin and Employee roles with policy-based access control
- **Strong Password Policies**: Minimum 8 characters with complexity requirements
- **Account Lockout**: 5 failed attempts trigger 15-minute lockout
- **CSRF Protection**: Anti-forgery tokens on all state-changing operations
- **Security Headers**: XSS protection, clickjacking prevention, Content Security Policy
- **Secure Cookies**: HttpOnly, Secure, SameSite strict policies
- **Input Validation**: Comprehensive server-side and client-side validation
- **SQL Injection Prevention**: Entity Framework parameterized queries

### 🎯 Application Features
- **Request Management**: Create, view, update, and delete change requests
- **Status Tracking**: Pending, Approved, Not Approved states
- **Priority Levels**: Low, Medium, High, Critical classifications
- **Admin Workflow**: Approval/rejection with reason documentation
- **Audit Trail**: Complete history with dates and submitter information
- **Email Notifications**: SMTP-based notifications for status changes
- **Structured Logging**: Serilog with daily rolling file logs
- **Responsive Design**: Mobile-friendly Bootstrap 5 interface
- **DataTables Integration**: Sortable, searchable request lists

## 🛠️ Technology Stack

**Backend:**
- ASP.NET Core 6.0 MVC
- C# 10
- Entity Framework Core 6
- ASP.NET Core Identity
- Repository Pattern & Unit of Work

**Database:**
- SQL Server / SQL Server LocalDB
- Entity Framework Migrations

**Frontend:**
- Bootstrap 5.3
- jQuery 3.6
- DataTables 1.13
- Font Awesome Icons
- Responsive Design

**Security & Logging:**
- Serilog (Structured Logging)
- ASP.NET Core Data Protection
- Identity Framework Authentication

**Development Tools:**
- Visual Studio 2022 / VS Code
- .NET CLI
- SQL Server Management Studio

## 📁 Project Structure

```
ChangeManagement/
├── ChangeManagement/              # 🌐 Main web application
│   ├── Controllers/               # MVC Controllers (Home, Request, Approve, etc.)
│   ├── Views/                     # Razor views with responsive layouts
│   ├── wwwroot/                   # Static files (CSS, JS, images)
│   ├── Areas/Identity/            # ASP.NET Core Identity UI
│   ├── Program.cs                 # Application startup & configuration
│   └── appsettings.json           # Configuration (non-sensitive)
│
├── Change.DataAccess/             # 💾 Data access layer
│   ├── Data/                      # DbContext configuration
│   ├── Migrations/                # EF Core migrations
│   └── Repository/                # Repository pattern implementation
│       ├── IRepository/           # Repository interfaces
│       ├── Repository.cs          # Generic repository
│       ├── RequestRepository.cs   # Request-specific repository
│       └── UnitOfWork.cs          # Unit of Work pattern
│
├── Change.Models/                 # 📋 Domain models
│   └── Models/
│       ├── ApplicationUser.cs     # Extended Identity user
│       ├── Request.cs             # Change request entity
│       └── ErrorViewModel.cs      # Error handling model
│
└── Change.Utility/                # 🔧 Shared utilities
    ├── EmailSender.cs             # SMTP email service
    └── SD.cs                      # Static definitions (roles, status)
```

## 🚀 Getting Started

### Prerequisites

- **.NET 6.0 SDK or later** - [Download](https://dotnet.microsoft.com/download/dotnet/6.0)
- **SQL Server 2019+** or **SQL Server LocalDB** - [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
- **Visual Studio 2022** or **VS Code** (optional but recommended)
- **Git** for version control

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/Karinateii/ChangeManagement.git
   cd ChangeManagement
   ```

2. **Configure User Secrets** (Recommended for development)
   
   Navigate to the main project folder:
   ```bash
   cd ChangeManagement
   dotnet user-secrets init
   ```
   
   Set admin credentials:
   ```bash
   dotnet user-secrets set "AdminUser:Email" "admin@example.com"
   dotnet user-secrets set "AdminUser:Password" "Admin@123456"
   dotnet user-secrets set "AdminUser:Name" "System Administrator"
   ```
   
   Configure database connection:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(LocalDb)\\MSSQLLocalDb;Database=CHANGE;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

3. **Configure Email** (Optional - for notifications)
   ```bash
   dotnet user-secrets set "EmailSettings:SmtpServer" "smtp.gmail.com"
   dotnet user-secrets set "EmailSettings:SmtpPort" "587"
   dotnet user-secrets set "EmailSettings:SenderEmail" "your-email@gmail.com"
   dotnet user-secrets set "EmailSettings:Username" "your-email@gmail.com"
   dotnet user-secrets set "EmailSettings:Password" "your-app-password"
   ```
   
   > 💡 **Tip**: For Gmail, use an [App Password](https://support.google.com/accounts/answer/185833) instead of your regular password.

4. **Restore Dependencies**
   ```bash
   cd ..
   dotnet restore
   ```

5. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement
   ```
   
   If `dotnet-ef` is not installed:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

6. **Run the Application**
   ```bash
   dotnet run --project ChangeManagement
   ```
   
   The application will start at:
   - **HTTPS**: `https://localhost:7XXX`
   - **HTTP**: `http://localhost:5XXX`
   
   (Check console output for exact ports)

7. **Login**
   - Use the admin credentials you configured in step 2
   - Default role: Admin (full access)

### 📖 Additional Documentation

- **[Configuration Guide](CONFIGURATION.md)** - Detailed setup instructions
- **[Security Documentation](SECURITY.md)** - Security features and best practices
- **[Deployment Guide](DEPLOYMENT.md)** - Production deployment instructions
- **[Quick Start Guide](QUICKSTART.md)** - Get up and running in 5 minutes

## ⚙️ Configuration

### Password Policy

Password requirements enforced:
- ✅ Minimum 8 characters
- ✅ At least 1 uppercase letter (A-Z)
- ✅ At least 1 lowercase letter (a-z)
- ✅ At least 1 digit (0-9)
- ✅ At least 1 special character (!@#$%^&*)
- ✅ Account lockout: 5 failed attempts = 15-minute lockout

### Authorization Policies

| Policy | Description | Access |
|--------|-------------|--------|
| **AdminOnly** | Administrator access | Full system control |
| **EmployeeOnly** | Standard employee | Create/view requests |
| **AdminOrEmployee** | Any authenticated user | Common features |

### Logging Configuration

Logs are written to:
- **Console output** (all environments)
- **File**: `logs/changemanagement-YYYYMMDD.txt` (daily rolling)

Configure log levels in `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    }
  }
}
```

## 👥 Usage

### User Roles

#### 🔐 Admin
**Full system access including:**
- View all change requests
- Approve or reject requests with reasons
- Delete requests
- Set approval dates and admin notes
- Access to admin dashboard

#### 👤 Employee
**Standard access including:**
- Create new change requests
- View all requests (read-only for others' requests)
- Edit own pending requests
- Track request status

### Creating a Change Request

1. Log in with employee or admin credentials
2. Navigate to **Request** → **Create New Request**
3. Fill in the form:
   - **Title**: Brief description (5-200 characters)
   - **Description**: Detailed explanation (10-2000 characters)
   - **Priority**: Select Low, Medium, High, or Critical
4. Click **Submit**
5. Request is created with "Pending" status

### Admin Approval Process

1. Log in with admin credentials
2. Navigate to **Approve** section
3. Review pending requests
4. Click **Approve** or **Not Approve**
5. Provide approval/rejection reason
6. Confirmation saved with timestamp

### Request Status Flow

```
📝 Pending → ✅ Approved
         → ❌ Not Approved
```

## 🔒 Security Best Practices Implemented

1. ✅ **No Hardcoded Secrets**: User Secrets for development, environment variables for production
2. ✅ **HTTPS Enforcement**: Automatic redirection with HSTS headers
3. ✅ **Security Headers**: X-Content-Type-Options, X-Frame-Options, X-XSS-Protection, CSP
4. ✅ **Input Validation**: Server-side validation with data annotations
5. ✅ **SQL Injection Prevention**: Entity Framework parameterized queries
6. ✅ **XSS Protection**: Razor encoding, Content Security Policy
7. ✅ **CSRF Protection**: Anti-forgery tokens on all forms
8. ✅ **Secure Authentication**: Identity Framework with strong password policies
9. ✅ **Account Lockout**: Brute-force attack prevention
10. ✅ **Secure Cookies**: HttpOnly, Secure, SameSite strict

> 📖 See [SECURITY.md](SECURITY.md) for detailed security documentation

## 🗄️ Database Schema

### Request Table

| Column | Type | Description | Constraints |
|--------|------|-------------|-------------|
| `Id` | int | Primary Key | Auto-increment |
| `Title` | nvarchar(200) | Request title | Required, 5-200 chars |
| `Description` | nvarchar(2000) | Detailed description | Required, 10-2000 chars |
| `Priority` | nvarchar(50) | Priority level | Required, Enum |
| `Status` | nvarchar(50) | Current status | Nullable, Enum |
| `Date` | datetime2 | Submission date/time | Required |
| `SubmittedBy` | nvarchar(256) | Username of submitter | Required |
| `AdminReason` | nvarchar(1000) | Admin decision reason | Nullable, max 1000 chars |
| `AdminApprovalDate` | datetime2 | Decision timestamp | Nullable |

### Enums

**Priority Levels:**
- Low
- Medium
- High
- Critical

**Status Values:**
- Pending (default)
- Approved
- Not Approved

## 🐛 Troubleshooting

### Database Connection Issues
**Problem**: Can't connect to database  
**Solutions**:
- Verify SQL Server is running
- Check connection string in user secrets: `dotnet user-secrets list`
- Ensure database exists or run migrations
- For Azure SQL, check firewall rules

### Migration Errors
```bash
# Remove last migration
dotnet ef migrations remove --project Change.DataAccess --startup-project ChangeManagement

# Add new migration
dotnet ef migrations add MigrationName --project Change.DataAccess --startup-project ChangeManagement

# Update database
dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement
```

### Admin User Not Created
**Problem**: Can't login with admin credentials  
**Solutions**:
- Check `logs/changemanagement-YYYYMMDD.txt` for errors
- Verify AdminUser secrets are set: `dotnet user-secrets list`
- Ensure password meets complexity requirements
- Delete and recreate database if needed

### Email Not Sending
**Problem**: Email notifications not working  
**Solutions**:
- Verify SMTP settings in user secrets
- For Gmail: Use App Password, not regular password
- Check firewall allows outbound SMTP (port 587/465)
- Review logs for detailed error messages
- Email is optional - app works without it

### Port Already in Use
**Problem**: Port 5000/7000 already in use  
**Solution**:
```bash
# Specify different ports
dotnet run --urls "https://localhost:7001;http://localhost:5001"
```

### User Secrets Not Loading
```bash
# List all secrets
dotnet user-secrets list --project ChangeManagement

# Clear all secrets
dotnet user-secrets clear --project ChangeManagement

# Re-initialize
dotnet user-secrets init --project ChangeManagement
```

## 🚀 Deployment

This application can be deployed to:
- ☁️ Azure App Service (Recommended)
- 🐳 Docker Container
- 🖥️ Windows Server / IIS
- 🐧 Linux Server (Ubuntu/Debian) with Nginx

See **[DEPLOYMENT.md](DEPLOYMENT.md)** for detailed deployment instructions.

### Quick Deploy to Azure

```bash
# Login to Azure
az login

# Create resources and deploy
az webapp up --sku B1 --name changemanagement-app --location eastus
```

## 📝 API Endpoints

### Request Management
- `GET /Request` - List all requests
- `GET /Request/Create` - New request form
- `POST /Request/Create` - Submit new request
- `GET /Request/Edit/{id}` - Edit request form
- `POST /Request/Edit/{id}` - Update request
- `POST /Request/Delete/{id}` - Delete request

### Admin Approval
- `GET /Approve` - Admin approval dashboard
- `POST /Approve/ApproveRequest` - Approve request
- `POST /Approve/NotApproveRequest` - Reject request

### Not Approved Requests
- `GET /NotApproved` - View rejected requests

All endpoints require authentication. Admin endpoints require Admin role.

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. **Commit your changes**
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. **Push to the branch**
   ```bash
   git push origin feature/AmazingFeature
   ```
5. **Open a Pull Request**

### Contribution Guidelines
- Follow C# coding conventions
- Add unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR
- Follow security best practices

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Doutimiwei Ebenezer** 
- GitHub: [@Karinateii](https://github.com/Karinateii)
- LinkedIn: [Doutimiwei Ebenezer](https://www.linkedin.com/in/ebenezer-doutimiwei-b929a6208/)
- Email: karinateidoutimiwei@gmail.com

## 🙏 Acknowledgments

- **ASP.NET Core Team** - For the excellent framework and documentation
- **Serilog Contributors** - For structured logging capabilities
- **Bootstrap Team** - For the responsive UI framework
- **jQuery & DataTables** - For enhanced user interactions
- **Stack Overflow Community** - For invaluable development support

## 📚 Learning Resources

This project demonstrates:
- Clean architecture principles
- Repository and Unit of Work patterns
- Identity Framework implementation
- Entity Framework Core best practices
- Security-first development approach
- Responsive web design

Perfect for learning ASP.NET Core MVC development!

## 🔗 Related Projects

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Bootstrap 5](https://getbootstrap.com)

---

<div align="center">

### ⭐ If you found this project helpful, please give it a star!

**Built with ❤️ using ASP.NET Core**

</div>
