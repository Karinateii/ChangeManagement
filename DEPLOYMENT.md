# Deployment Guide

## Pre-Deployment Checklist

- [ ] All sensitive data removed from source code
- [ ] Connection strings configured via environment variables or Key Vault
- [ ] Admin credentials configured in production secrets
- [ ] Email settings configured and tested
- [ ] HTTPS certificate installed and configured
- [ ] Database backup strategy in place
- [ ] Logging configured and tested
- [ ] Error pages tested in production mode
- [ ] All NuGet packages updated to latest stable versions
- [ ] Security headers validated
- [ ] Performance testing completed

## Deployment Options

### Option 1: Azure App Service (Recommended)

#### Prerequisites
- Azure subscription
- Azure CLI installed or use Azure Portal

#### Steps

1. **Create Azure SQL Database**
```bash
# Create resource group
az group create --name ChangeManagementRG --location eastus

# Create SQL Server
az sql server create \
  --name changemanagement-sql \
  --resource-group ChangeManagementRG \
  --location eastus \
  --admin-user sqladmin \
  --admin-password 'YourSecureP@ssw0rd123!'

# Create database
az sql db create \
  --resource-group ChangeManagementRG \
  --server changemanagement-sql \
  --name ChangeManagementDB \
  --service-objective S0

# Configure firewall (allow Azure services)
az sql server firewall-rule create \
  --resource-group ChangeManagementRG \
  --server changemanagement-sql \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

2. **Create App Service**
```bash
# Create App Service Plan
az appservice plan create \
  --name ChangeManagementPlan \
  --resource-group ChangeManagementRG \
  --location eastus \
  --sku B1 \
  --is-linux

# Create Web App
az webapp create \
  --resource-group ChangeManagementRG \
  --plan ChangeManagementPlan \
  --name changemanagement-app \
  --runtime "DOTNET|6.0"
```

3. **Configure Application Settings**
```bash
# Set connection string
az webapp config connection-string set \
  --resource-group ChangeManagementRG \
  --name changemanagement-app \
  --connection-string-type SQLAzure \
  --settings DefaultConnection="Server=tcp:changemanagement-sql.database.windows.net,1433;Database=ChangeManagementDB;User ID=sqladmin;Password=YourSecureP@ssw0rd123!;Encrypt=True;TrustServerCertificate=False;"

# Set application settings
az webapp config appsettings set \
  --resource-group ChangeManagementRG \
  --name changemanagement-app \
  --settings \
    AdminUser__Email="admin@yourdomain.com" \
    AdminUser__Password="YourSecureP@ssw0rd123!" \
    AdminUser__Name="System Administrator" \
    EmailSettings__SmtpServer="smtp.gmail.com" \
    EmailSettings__SmtpPort="587" \
    EmailSettings__SenderEmail="your-email@gmail.com" \
    EmailSettings__Username="your-email@gmail.com" \
    EmailSettings__Password="your-app-password"
```

4. **Deploy Application**

**Option A: Using Visual Studio**
- Right-click on ChangeManagement project
- Select "Publish"
- Choose "Azure"
- Select your App Service
- Click "Publish"

**Option B: Using Azure CLI**
```bash
# Publish the application locally first
cd ChangeManagement
dotnet publish -c Release -o ./publish

# Create zip file
cd publish
zip -r ../app.zip .

# Deploy to Azure
cd ..
az webapp deployment source config-zip \
  --resource-group ChangeManagementRG \
  --name changemanagement-app \
  --src app.zip
```

**Option C: Using GitHub Actions** (see GitHub Actions section below)

5. **Run Migrations**
```bash
# Option 1: Automatic (already configured in Program.cs)
# Migrations run automatically on application startup

# Option 2: Manual from local machine
# Update connection string to Azure SQL in appsettings.json temporarily
dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement
```

6. **Configure Custom Domain (Optional)**
```bash
az webapp config hostname add \
  --webapp-name changemanagement-app \
  --resource-group ChangeManagementRG \
  --hostname www.yourdomain.com
```

7. **Enable HTTPS (Automatic with Azure)**
Azure App Service automatically provides HTTPS. To enforce it:
```bash
az webapp update \
  --resource-group ChangeManagementRG \
  --name changemanagement-app \
  --https-only true
```

### Option 2: Docker Container

#### Create Dockerfile
```dockerfile
# Already included in project root if needed
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChangeManagement/ChangeManagement.csproj", "ChangeManagement/"]
COPY ["Change.DataAccess/Change.DataAccess.csproj", "Change.DataAccess/"]
COPY ["Change.Models/Change.Models.csproj", "Change.Models/"]
COPY ["Change.Utility/Change.Utility.csproj", "Change.Utility/"]
RUN dotnet restore "ChangeManagement/ChangeManagement.csproj"
COPY . .
WORKDIR "/src/ChangeManagement"
RUN dotnet build "ChangeManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChangeManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChangeManagement.dll"]
```

#### Build and Run Docker Container
```bash
# Build image
docker build -t changemanagement:latest .

# Run container
docker run -d \
  -p 8080:80 \
  -e ConnectionStrings__DefaultConnection="your-connection-string" \
  -e AdminUser__Email="admin@yourdomain.com" \
  -e AdminUser__Password="YourSecureP@ssw0rd123!" \
  --name changemanagement-app \
  changemanagement:latest
```

#### Deploy to Azure Container Instances
```bash
# Create container registry
az acr create \
  --resource-group ChangeManagementRG \
  --name changemanagementacr \
  --sku Basic

# Build and push image to ACR
az acr build \
  --registry changemanagementacr \
  --image changemanagement:latest .

# Deploy to Container Instance
az container create \
  --resource-group ChangeManagementRG \
  --name changemanagement-container \
  --image changemanagementacr.azurecr.io/changemanagement:latest \
  --dns-name-label changemanagement \
  --ports 80 443 \
  --registry-username <username> \
  --registry-password <password> \
  --environment-variables \
    ConnectionStrings__DefaultConnection="your-connection-string" \
    AdminUser__Email="admin@yourdomain.com"
```

### Option 3: Windows Server / IIS

#### Prerequisites
- Windows Server 2016 or later
- IIS installed with ASP.NET Core hosting bundle
- SQL Server (local or remote)

#### Installation Steps

1. **Install .NET 6.0 Runtime**
   - Download from: https://dotnet.microsoft.com/download/dotnet/6.0
   - Install ASP.NET Core 6.0 Runtime & Hosting Bundle

2. **Install IIS Features**
```powershell
Install-WindowsFeature -name Web-Server -IncludeManagementTools
Install-WindowsFeature Web-Asp-Net45
```

3. **Publish Application**
```bash
# On development machine
dotnet publish -c Release -o C:\Publish\ChangeManagement
```

4. **Copy Files to Server**
   - Copy contents of publish folder to `C:\inetpub\wwwroot\ChangeManagement`

5. **Configure IIS**
   - Open IIS Manager
   - Create new Application Pool:
     - Name: ChangeManagementAppPool
     - .NET CLR Version: No Managed Code
     - Managed Pipeline Mode: Integrated
   - Create new Website:
     - Site name: ChangeManagement
     - Application Pool: ChangeManagementAppPool
     - Physical path: C:\inetpub\wwwroot\ChangeManagement
     - Binding: HTTP, Port 80 (or HTTPS, Port 443)

6. **Configure Application Settings**
   - Create `appsettings.Production.json` in deployment folder
   - Or use environment variables in Application Pool settings

7. **Set Permissions**
```powershell
# Grant IIS user access to application folder
icacls "C:\inetpub\wwwroot\ChangeManagement" /grant "IIS AppPool\ChangeManagementAppPool:(OI)(CI)F" /T
```

8. **Configure SSL Certificate** (if using HTTPS)
   - Install certificate in IIS
   - Update site binding to HTTPS

### Option 4: Linux Server (Ubuntu/Debian)

#### Prerequisites
- Ubuntu 20.04 LTS or Debian 11
- Nginx or Apache
- SQL Server or Azure SQL

#### Installation Steps

1. **Install .NET 6.0 Runtime**
```bash
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-6.0
```

2. **Publish and Transfer Application**
```bash
# On development machine
dotnet publish -c Release -o ./publish

# Transfer to server
scp -r ./publish user@server:/var/www/changemanagement
```

3. **Create Systemd Service**
```bash
sudo nano /etc/systemd/system/changemanagement.service
```

```ini
[Unit]
Description=Change Management ASP.NET Core App
After=network.target

[Service]
WorkingDirectory=/var/www/changemanagement
ExecStart=/usr/bin/dotnet /var/www/changemanagement/ChangeManagement.dll
Restart=always
RestartSec=10
SyslogIdentifier=changemanagement
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ConnectionStrings__DefaultConnection=your-connection-string
Environment=AdminUser__Email=admin@yourdomain.com
Environment=AdminUser__Password=YourSecureP@ssw0rd123!

[Install]
WantedBy=multi-user.target
```

4. **Start Service**
```bash
sudo systemctl enable changemanagement
sudo systemctl start changemanagement
sudo systemctl status changemanagement
```

5. **Configure Nginx**
```bash
sudo nano /etc/nginx/sites-available/changemanagement
```

```nginx
server {
    listen 80;
    server_name yourdomain.com www.yourdomain.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

```bash
sudo ln -s /etc/nginx/sites-available/changemanagement /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

6. **Configure SSL with Let's Encrypt**
```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d yourdomain.com -d www.yourdomain.com
```

## GitHub Actions CI/CD

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]
  workflow_dispatch:

env:
  AZURE_WEBAPP_NAME: changemanagement-app
  DOTNET_VERSION: '6.0.x'

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: ${{ env.AZURE_WEBAPP_NAME }}
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/myapp
```

## Post-Deployment Steps

1. **Verify Application**
   - Access application URL
   - Test login with admin credentials
   - Create a test request
   - Verify email functionality

2. **Database Verification**
   - Confirm migrations applied
   - Check admin user exists
   - Verify roles created

3. **Security Verification**
   - Confirm HTTPS working
   - Test security headers
   - Verify authentication/authorization

4. **Monitoring Setup**
   - Configure Application Insights (Azure)
   - Set up log monitoring
   - Configure alerts for errors

5. **Backup Configuration**
   - Set up automated database backups
   - Configure file system backups
   - Test restore procedures

## Troubleshooting

### Issue: Migrations not applying
```bash
# Manual migration from connection to production DB
dotnet ef database update --project Change.DataAccess --startup-project ChangeManagement --connection "your-production-connection-string"
```

### Issue: 500 Internal Server Error
- Check application logs
- Verify connection string is correct
- Ensure all environment variables are set
- Check file permissions (Linux/IIS)

### Issue: Database connection timeout
- Verify firewall rules
- Check SQL Server is accepting remote connections
- Verify credentials are correct

## Rollback Procedure

1. **Azure App Service**
```bash
# Swap deployment slots
az webapp deployment slot swap \
  --resource-group ChangeManagementRG \
  --name changemanagement-app \
  --slot staging \
  --target-slot production
```

2. **IIS/Linux**
   - Stop application
   - Restore previous version files
   - Restart application

3. **Database Rollback**
   - Restore from backup
   - Or migrate to specific version:
```bash
dotnet ef database update PreviousMigrationName --project Change.DataAccess --startup-project ChangeManagement
```

## Performance Optimization

- Enable response compression
- Configure output caching
- Use CDN for static assets
- Optimize database indexes
- Enable application-level caching

## Support

For deployment issues:
1. Check application logs: `logs/changemanagement-YYYYMMDD.txt`
2. Review Azure App Service logs (if using Azure)
3. Check system event logs (Windows/Linux)
4. Verify all configuration settings
