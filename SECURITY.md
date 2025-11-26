# Security Documentation

## Security Enhancements Summary

This document outlines the security improvements made to the Change Management System.

## Authentication & Authorization

### Identity Configuration
- **Password Requirements**:
  - Minimum 8 characters
  - Requires uppercase letters
  - Requires lowercase letters
  - Requires digits
  - Requires non-alphanumeric characters
  - Minimum of 1 unique character

- **Account Lockout**:
  - Lockout duration: 15 minutes
  - Max failed attempts: 5
  - Enabled for all users including new registrations

- **Cookie Security**:
  - `HttpOnly`: true (prevents JavaScript access)
  - `SecurePolicy`: Always (requires HTTPS)
  - `SameSite`: Strict (CSRF protection)
  - Sliding expiration: 24 hours

### Authorization Policies
Policy-based authorization instead of role strings:
- `AdminOnly`: Admin role required
- `EmployeeOnly`: Employee role required
- `AdminOrEmployee`: Either role accepted

## CSRF Protection

- Anti-forgery tokens on all POST, PUT, DELETE actions
- `[ValidateAntiForgeryToken]` attribute on all state-changing operations
- Custom header name: `X-CSRF-TOKEN`
- Secure cookie policy enforced

## Security Headers

The following security headers are automatically added to all responses:

```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Referrer-Policy: strict-origin-when-cross-origin
Content-Security-Policy: default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' cdn.jsdelivr.net ajax.aspnetcdn.com; style-src 'self' 'unsafe-inline' cdn.jsdelivr.net; font-src 'self' cdn.jsdelivr.net; img-src 'self' data:;
```

### Header Explanations

1. **X-Content-Type-Options: nosniff**
   - Prevents MIME type sniffing
   - Forces browsers to respect declared content types

2. **X-Frame-Options: DENY**
   - Prevents clickjacking attacks
   - Disallows embedding in iframes

3. **X-XSS-Protection: 1; mode=block**
   - Enables XSS filter in older browsers
   - Blocks pages when XSS detected

4. **Content-Security-Policy**
   - Restricts resource loading
   - Mitigates XSS and injection attacks
   - Allows CDN resources for Bootstrap/jQuery

5. **Referrer-Policy**
   - Controls referrer information leakage
   - Sends origin only on cross-origin requests

## Input Validation

### Model Validation
All models include comprehensive data annotations:

```csharp
[Required(ErrorMessage = "Title is required.")]
[StringLength(200, MinimumLength = 5)]
public string Title { get; set; }

[Required(ErrorMessage = "Description is required.")]
[StringLength(2000, MinimumLength = 10)]
public string Description { get; set; }

[RegularExpression("^(Low|Medium|High|Critical)$")]
public string Priority { get; set; }
```

### View-Level Validation
- Client-side validation with jQuery Validation
- `maxlength` attributes on inputs
- `required` attributes on mandatory fields
- Type-specific inputs (email, datetime-local)

## SQL Injection Prevention

- **Entity Framework Core**: All database queries use parameterized commands
- **Repository Pattern**: No raw SQL queries
- **LINQ expressions**: Type-safe query construction

Example:
```csharp
// Safe - parameterized automatically
var request = _unitOfWork.Request.Get(u => u.Id == id);
```

## Sensitive Data Protection

### Configuration Secrets
Never store sensitive data in source control:

**Development**: Use User Secrets
```bash
dotnet user-secrets set "AdminUser:Email" "admin@example.com"
dotnet user-secrets set "AdminUser:Password" "SecurePassword123!"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;"
```

**Production**: Use environment-specific configurations
- Azure Key Vault
- AWS Secrets Manager
- Environment variables
- Managed identities

### Excluded from Source Control
- `appsettings.Production.json` with real credentials
- User Secrets storage
- Connection strings with real servers
- Email credentials

## Logging & Monitoring

### Structured Logging (Serilog)
- **Information**: Successful operations, user actions
- **Warning**: Non-critical issues, missing configurations
- **Error**: Exceptions with full stack traces
- **Critical**: Application startup failures

### What's Logged
✅ User authentication events
✅ Request creation/updates
✅ Admin approval actions
✅ Database operations
✅ Email sending attempts
✅ Configuration issues

### What's NOT Logged
❌ Passwords
❌ Session tokens
❌ Personal identifiable information (PII)
❌ Credit card numbers
❌ API keys

### Log Files
- Location: `logs/changemanagement-YYYYMMDD.txt`
- Rotation: Daily
- Retention: Manual cleanup required
- Format: JSON-structured for easy parsing

## Error Handling

### Production Error Pages
- Custom error pages (no stack traces)
- Generic error messages to users
- Detailed errors logged server-side

### Development Mode
- Developer exception page enabled
- Detailed stack traces
- Database error page

## HTTPS Enforcement

### Configuration
```csharp
app.UseHttpsRedirection();  // Redirects HTTP to HTTPS
app.UseHsts();              // HTTP Strict Transport Security
```

### HSTS Settings
- Default: 30 days
- Includes subdomains
- Preload eligible

## Database Security

### Connection String Security
- Use integrated security or managed identities
- Never commit connection strings to source control
- Use encrypted connections: `TrustServerCertificate=True` (development only)

### Migrations
- Applied automatically on startup
- Logged with errors
- Transactional execution

### Entity Framework Best Practices
- No SQL injection vulnerabilities
- Parameterized queries only
- Tracked entities for updates
- Soft delete possible (not implemented)

## Email Security

### SMTP Configuration
- TLS/SSL encryption enforced
- Credentials stored in secrets
- Timeouts configured
- Error handling without exposing credentials

### Email Content
- HTML sanitization (if accepting user content)
- No inline credentials
- Proper error logging

## API Endpoint Security

### API Controllers
- Authorization required on all endpoints
- Input validation on all parameters
- Proper HTTP status codes
- Error messages don't expose internal details

### Rate Limiting
Currently not implemented. Consider adding:
- AspNetCoreRateLimit package
- Per-user or per-IP limits
- Configurable thresholds

## Session Management

### Cookie Settings
```csharp
options.Cookie.HttpOnly = true;          // JavaScript can't access
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // HTTPS only
options.Cookie.SameSite = SameSiteMode.Strict;           // CSRF protection
options.ExpireTimeSpan = TimeSpan.FromHours(24);         // 24-hour sessions
options.SlidingExpiration = true;                        // Extends on activity
```

## Admin User Creation

### Secure Initialization
- No hardcoded credentials
- Configuration-based user creation
- Validated password requirements
- Email confirmation optional
- Logged creation attempts

## Recommended Production Hardening

1. **Enable Email Confirmation**
   ```csharp
   options.SignIn.RequireConfirmedEmail = true;
   ```

2. **Add Rate Limiting**
   - Install: `AspNetCoreRateLimit`
   - Configure per-endpoint limits

3. **Implement CAPTCHA**
   - Add reCAPTCHA on login/registration
   - Prevents brute force attacks

4. **Add 2FA Support**
   - Already supported by Identity
   - Configure authenticator app support

5. **Database Encryption**
   - Enable Transparent Data Encryption (TDE)
   - Encrypt backups

6. **Regular Security Updates**
   - Keep NuGet packages updated
   - Monitor CVE databases
   - Subscribe to security bulletins

7. **Penetration Testing**
   - Regular security audits
   - Automated scanning (OWASP ZAP, etc.)
   - Manual testing of critical paths

8. **Content Security Policy Tuning**
   - Remove `unsafe-inline` and `unsafe-eval` if possible
   - Implement nonce-based CSP
   - Report violations to monitoring service

## Security Checklist

- [x] Password policies enforced
- [x] Account lockout configured
- [x] CSRF protection on all forms
- [x] Security headers implemented
- [x] Input validation on all fields
- [x] SQL injection protection (EF Core)
- [x] XSS protection (encoded output)
- [x] HTTPS enforcement
- [x] Secure cookie settings
- [x] Structured logging
- [x] Error handling without info leakage
- [x] Authorization policies
- [x] No hardcoded secrets
- [ ] Rate limiting (recommended)
- [ ] CAPTCHA on login (recommended)
- [ ] 2FA support (recommended)
- [ ] Email confirmation (optional)

## Reporting Security Issues

If you discover a security vulnerability:
1. **DO NOT** open a public issue
2. Email: security@yourdomain.com
3. Include detailed reproduction steps
4. Allow time for patching before disclosure

## References

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [NIST Security Guidelines](https://www.nist.gov/cyberframework)
