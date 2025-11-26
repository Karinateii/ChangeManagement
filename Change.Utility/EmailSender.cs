using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Change.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPortString = _configuration["EmailSettings:SmtpPort"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];

                // If email settings are not configured, log and return
                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(senderEmail))
                {
                    _logger.LogWarning("Email settings not configured. Email to {Email} with subject '{Subject}' was not sent.", email, subject);
                    return;
                }

                if (!int.TryParse(smtpPortString, out int smtpPort))
                {
                    smtpPort = 587; // Default SMTP port
                }

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(username ?? senderEmail, password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail, senderName ?? "Change Management System"),
                        Subject = subject,
                        Body = htmlMessage,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email sent successfully to {Email} with subject '{Subject}'", email, subject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email} with subject '{Subject}'", email, subject);
                // Don't throw - email failures shouldn't break the application flow
            }
        }
    }
}
