# Configuration Guide

## Initial Setup

### 1. Configure User Secrets (Recommended for Development)

User Secrets keep sensitive data out of your source code. Run these commands from the `ChangeManagement` project directory:

```bash
# Initialize user secrets
dotnet user-secrets init

# Set admin user credentials
dotnet user-secrets set "AdminUser:Email" "admin@yourdomain.com"
dotnet user-secrets set "AdminUser:Password" "YourSecureP@ssw0rd123!"
dotnet user-secrets set "AdminUser:Name" "System Administrator"
dotnet user-secrets set "AdminUser:PhoneNumber" "123-456-7890"
dotnet user-secrets set "AdminUser:StreetAddress" "123 Main St"
dotnet user-secrets set "AdminUser:City" "Your City"
dotnet user-secrets set "AdminUser:State" "Your State"
dotnet user-secrets set "AdminUser:PostalCode" "12345"

# Set database connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(LocalDb)\\MSSQLLocalDb;Database=CHANGE;Trusted_Connection=True;TrustServerCertificate=True;"

# Set email configuration (optional but recommended)
dotnet user-secrets set "EmailSettings:SmtpServer" "smtp.gmail.com"
dotnet user-secrets set "EmailSettings:SmtpPort" "587"
dotnet user-secrets set "EmailSettings:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:SenderName" "Change Management System"
dotnet user-secrets set "EmailSettings:Username" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:Password" "your-app-specific-password"
```

### 2. Gmail Configuration (If Using Gmail for Emails)

To use Gmail for sending emails:

1. Enable 2-Factor Authentication on your Google account
2. Generate an App Password:
   - Go to Google Account Settings
   - Security → 2-Step Verification → App passwords
   - Select "Mail" and your device
   - Copy the 16-character password
3. Use this App Password in the `EmailSettings:Password` secret

### 3. Database Configuration

#### Option A: SQL Server LocalDB (Development - Default)
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(LocalDb)\\MSSQLLocalDb;Database=CHANGE;Trusted_Connection=True;TrustServerCertificate=True;"
```

#### Option B: SQL Server Express
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost\\SQLEXPRESS;Database=CHANGE;Trusted_Connection=True;TrustServerCertificate=True;"
```

#### Option C: Full SQL Server with Credentials
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=your-server;Database=CHANGE;User Id=your-user;Password=your-password;TrustServerCertificate=True;"
```

#### Option D: Azure SQL Database
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=tcp:your-server.database.windows.net,1433;Database=CHANGE;User ID=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=False;"
```

### 4. Apply Database Migrations

```bash
# From the solution root directory
dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement

# Or if EF tools are not installed globally:
dotnet tool install --global dotnet-ef
dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement
```

### 5. Run the Application

```bash
cd ChangeManagement
dotnet run
```

The application will be available at:
- HTTPS: `https://localhost:7XXX` (check console output)
- HTTP: `http://localhost:5XXX`

## Production Configuration

### Environment Variables (Recommended for Production)

Instead of User Secrets, use environment variables:

**Linux/macOS:**
```bash
export AdminUser__Email="admin@yourdomain.com"
export AdminUser__Password="YourSecureP@ssw0rd123!"
export ConnectionStrings__DefaultConnection="your-connection-string"
export EmailSettings__SmtpServer="smtp.gmail.com"
export EmailSettings__SmtpPort="587"
export EmailSettings__SenderEmail="your-email@gmail.com"
export EmailSettings__Username="your-email@gmail.com"
export EmailSettings__Password="your-app-password"
```

**Windows (PowerShell):**
```powershell
$env:AdminUser__Email = "admin@yourdomain.com"
$env:AdminUser__Password = "YourSecureP@ssw0rd123!"
$env:ConnectionStrings__DefaultConnection = "your-connection-string"
$env:EmailSettings__SmtpServer = "smtp.gmail.com"
$env:EmailSettings__SmtpPort = "587"
$env:EmailSettings__SenderEmail = "your-email@gmail.com"
$env:EmailSettings__Username = "your-email@gmail.com"
$env:EmailSettings__Password = "your-app-password"
```

**Windows (Command Prompt):**
```cmd
set AdminUser__Email=admin@yourdomain.com
set AdminUser__Password=YourSecureP@ssw0rd123!
set ConnectionStrings__DefaultConnection=your-connection-string
```

### Azure App Service Configuration

1. Go to Azure Portal → Your App Service
2. Navigate to Configuration → Application settings
3. Add new application settings:
   - Name: `AdminUser:Email`, Value: `admin@yourdomain.com`
   - Name: `AdminUser:Password`, Value: `YourSecureP@ssw0rd123!`
   - Name: `ConnectionStrings:DefaultConnection`, Value: `your-connection-string`
   - etc.

### Azure Key Vault (Most Secure)

```csharp
// In Program.cs (after builder creation):
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

Store secrets in Azure Key Vault:
- `AdminUser--Email`
- `AdminUser--Password`
- `ConnectionStrings--DefaultConnection`
- `EmailSettings--SmtpServer`
- etc.

## Configuration File Structure

### appsettings.json (Base Settings)
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### appsettings.Development.json (Development Only)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "DetailedErrors": true
}
```

### appsettings.Production.json (Production - NOT in source control)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com"
}
```

## Password Requirements

The system enforces the following password policy:
- Minimum 8 characters
- At least 1 uppercase letter (A-Z)
- At least 1 lowercase letter (a-z)
- At least 1 digit (0-9)
- At least 1 special character (!@#$%^&*)
- At least 1 unique character

Example valid passwords:
- `SecureP@ss123`
- `Admin2024!Pass`
- `MyP@ssw0rd99`

## Email Configuration Examples

### Gmail
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": "587",
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Change Management System",
    "Username": "your-email@gmail.com",
    "Password": "your-16-char-app-password"
  }
}
```

### Outlook/Office 365
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.office365.com",
    "SmtpPort": "587",
    "SenderEmail": "your-email@outlook.com",
    "SenderName": "Change Management System",
    "Username": "your-email@outlook.com",
    "Password": "your-password"
  }
}
```

### SendGrid
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.sendgrid.net",
    "SmtpPort": "587",
    "SenderEmail": "noreply@yourdomain.com",
    "SenderName": "Change Management System",
    "Username": "apikey",
    "Password": "your-sendgrid-api-key"
  }
}
```

### AWS SES
```json
{
  "EmailSettings": {
    "SmtpServer": "email-smtp.us-east-1.amazonaws.com",
    "SmtpPort": "587",
    "SenderEmail": "noreply@yourdomain.com",
    "SenderName": "Change Management System",
    "Username": "your-smtp-username",
    "Password": "your-smtp-password"
  }
}
```

## Troubleshooting Configuration

### Issue: Admin user not created
**Solution:** Check logs in `logs/changemanagement-YYYYMMDD.txt` for errors. Ensure:
- AdminUser configuration is present
- Password meets requirements
- Database connection is valid

### Issue: Email not sending
**Solution:**
- Verify SMTP settings are correct
- Check firewall allows outbound on port 587 or 465
- For Gmail, ensure App Password is used (not regular password)
- Check logs for detailed error messages

### Issue: Database connection failed
**Solution:**
- Verify SQL Server is running
- Check connection string format
- Ensure database user has proper permissions
- For Azure SQL, ensure firewall rules allow connection

### Issue: User Secrets not loading
**Solution:**
```bash
# Verify secrets are set
dotnet user-secrets list --project ChangeManagement

# Clear and reset if needed
dotnet user-secrets clear --project ChangeManagement
# Then re-add secrets
```

## Security Checklist

- [ ] Never commit `appsettings.Production.json` to source control
- [ ] Use User Secrets for development
- [ ] Use environment variables or Key Vault for production
- [ ] Change default admin password immediately after first login
- [ ] Use strong, unique passwords (password manager recommended)
- [ ] Enable HTTPS in production
- [ ] Configure proper firewall rules
- [ ] Regular backup of database
- [ ] Keep NuGet packages updated
- [ ] Monitor application logs regularly

## Support

For issues or questions:
1. Check the logs: `logs/changemanagement-YYYYMMDD.txt`
2. Review this configuration guide
3. Check README.md for general information
4. Review SECURITY.md for security-specific guidance
