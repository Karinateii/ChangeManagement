# Quick Start Guide

## 🚀 Get Up and Running in 5 Minutes

### Step 1: Configure Secrets (2 minutes)

```bash
cd ChangeManagement
dotnet user-secrets init
dotnet user-secrets set "AdminUser:Email" "admin@yourcompany.com"
dotnet user-secrets set "AdminUser:Password" "SecureP@ssw0rd123!"
dotnet user-secrets set "AdminUser:Name" "Admin User"
```

### Step 2: Install Dependencies (1 minute)

```bash
dotnet restore
```

### Step 3: Setup Database (1 minute)

```bash
dotnet ef database update --project ../Change.DataAccess --startup-project .
```

### Step 4: Run Application (1 minute)

```bash
dotnet run
```

Navigate to: `https://localhost:7XXX` (check console for exact port)

---

## 📋 First Login

1. Click "Login" or navigate to `/Identity/Account/Login`
2. Login with your configured admin credentials
3. Change password immediately after first login

---

## 🔧 Common Tasks

### Create a New Request (Employee)
1. Login as employee
2. Click "Create New" on Requests page
3. Fill in Title, Description, Priority
4. Click "Create"

### Approve/Reject Request (Admin)
1. Login as admin
2. Click on request in list
3. Change Status dropdown
4. Add Admin Reason
5. Set Admin Approval Date
6. Click "Update Status"

### View Approved Requests
- Navigate to "Approved" in navigation menu
- View all approved requests with details

### View Rejected Requests
- Navigate to "Not Approved" in navigation menu
- View all rejected requests with reasons

---

## 🛠️ Troubleshooting

### Database Connection Failed
```bash
# Check SQL Server is running
# Default connection uses LocalDB
# Update connection string if needed:
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your-Connection-String"
```

### Admin User Not Created
```bash
# Check logs in logs/changemanagement-YYYYMMDD.txt
# Verify secrets are set:
dotnet user-secrets list
# Ensure password meets requirements (8+ chars, uppercase, lowercase, digit, special char)
```

### Email Not Sending
```bash
# Configure email settings (optional):
dotnet user-secrets set "EmailSettings:SmtpServer" "smtp.gmail.com"
dotnet user-secrets set "EmailSettings:SmtpPort" "587"
dotnet user-secrets set "EmailSettings:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:Username" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:Password" "your-app-password"
```

---

## 📚 Need More Help?

- **Full Setup**: See `CONFIGURATION.md`
- **Security Info**: See `SECURITY.md`
- **Deployment**: See `DEPLOYMENT.md`
- **All Changes**: See `IMPROVEMENTS_SUMMARY.md`
- **Project Overview**: See `README.md`

---

## ⚡ Quick Commands Reference

| Task | Command |
|------|---------|
| Restore packages | `dotnet restore` |
| Build project | `dotnet build` |
| Run application | `dotnet run --project ChangeManagement` |
| Update database | `dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement` |
| Add migration | `dotnet ef migrations add MigrationName --project Change.DataAccess --startup-project ChangeManagement` |
| List secrets | `dotnet user-secrets list --project ChangeManagement` |
| Clear secrets | `dotnet user-secrets clear --project ChangeManagement` |
| Publish for deployment | `dotnet publish -c Release -o ./publish` |

---

## 🎯 Key Features

✅ Role-based access (Admin/Employee)  
✅ Secure authentication with lockout  
✅ CSRF protection  
✅ XSS protection  
✅ SQL injection protection  
✅ Security headers  
✅ Input validation  
✅ Email notifications  
✅ Structured logging  
✅ Professional error handling  

---

## 🔐 Default Credentials

**IMPORTANT:** Configure your own admin credentials via user secrets before first run!

After first login, immediately:
1. Change admin password
2. Update admin profile information
3. Create employee accounts

---

## 💡 Pro Tips

- Use **User Secrets** for development (already configured in steps above)
- Use **Environment Variables** for production
- Check **logs folder** for detailed error information
- Enable **email settings** for notifications
- Always use **HTTPS** in production
- Keep **NuGet packages** updated
- Backup **database** regularly

---

**Ready to go!** 🎉

For detailed documentation, see the other .md files in the project root.
