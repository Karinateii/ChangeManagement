# LinkedIn Project Notes

## For LinkedIn Project Section

**Project Name:** Change Management System

**Description:**
Developed a web application for managing organizational change requests using ASP.NET Core 6.0 MVC. The system implements role-based access control where admins can approve or reject requests submitted by employees. Built with security in mind, including CSRF protection, password policies, and secure authentication. The frontend uses Bootstrap 5 for a responsive design that works across devices. Backend utilizes Entity Framework Core with a repository pattern for data access, and Serilog for logging. Key features include request workflow management, status tracking with priority levels, and an admin approval system.

**Skills to tag:** 
ASP.NET Core • C# • Entity Framework Core • SQL Server • Bootstrap • JavaScript • jQuery • Web Security • Repository Pattern • Authentication & Authorization • Responsive Design

**Project URL:** https://github.com/Karinateii/ChangeManagement

---

## If Asked About Technical Details

**Architecture:**
The application follows a layered architecture with separate projects for data access, models, and utilities. I used the repository pattern with Unit of Work to keep the data access logic organized and testable. Most operations are async for better performance.

**Security:**
Implemented several security measures including CSRF tokens on forms, security headers (X-Frame-Options, XSS-Protection, Content Security Policy), password complexity requirements, and account lockout after failed login attempts. No sensitive data is hardcoded - everything uses configuration or user secrets.

**Frontend:**
The UI uses Bootstrap 5 with custom CSS for a professional look. I added DataTables for the request lists with custom rendering for status badges and priority indicators. The design is mobile-responsive and includes hover effects and smooth transitions.

**Challenges:**
One interesting problem was getting DataTables to work properly with the ASP.NET Core JSON serialization. The property names needed to match exactly between the JavaScript and C# models. I also configured Content Security Policy to work with CDN resources while maintaining security.

**What I'd Improve:**
If I had more time, I'd add two-factor authentication, implement real-time notifications with SignalR, and add file attachment capabilities for requests. I'd also expand the logging to create a full audit trail of all changes.

