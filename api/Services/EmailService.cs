using System.Net;
using System.Net.Mail;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;
using api.Utils;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public class EmailService(IOptions<EmailSettings> smtpSettings) : IEmailService
    {
        private readonly EmailSettings _smtpSettings = smtpSettings.Value;

        public async Task SendAttendanceConfirmationEmailAsync(User student)
        {
            var DateTime = DateTimeUtils.DateTimeNowFormattedString();
            if (student.Email != null)
            {
                var subject = "Student's Attendance Confirmation";
                var body = $"Hello, <strong>{student.FirstName}</strong>, <br>You have join the class at <strong>{DateTime}</strong>.";
                await SendEmailAsync(student.Email, subject, body);
            }
            if (student.Guardian?.Email != null)
            {
                var subject = "Guardian Confirmation to Student's Attendance";
                var body = $"Hello, <strong>{student.Guardian.FirstName}</strong>, <br>Your student has join the class at <strong>{DateTime}</strong>.";
                await SendEmailAsync(student.Guardian.Email, subject, body);
            }
        }

        public async Task SendOTPEmailAsync(string toEmail, string body)
        {
            var subject = "2FA Verification";
            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string subject, string body)
        {
            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port);
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
