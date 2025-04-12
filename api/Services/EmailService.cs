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
                var subject = "Class Attendance Confirmation";
                var body = $@"
                            <p>Hello <strong>{student.FirstName}</strong>,</p>
                            <p>This is to confirm that you have successfully joined the class on <strong>{DateTime}</strong>.</p>
                            <p>Keep up the great attendance!</p>
                            <br>
                        ";
                await SendEmailAsync(student.Email, subject, body);
            }
            if (student.Guardian?.Email != null)
            {
                var subject = "Student Attendance Notification";
                var body = $@"
                            <p>Hello <strong>{student.Guardian.FirstName}</strong>,</p>
                            <p>This is to inform you that <strong>{student.FirstName}</strong> has successfully joined the class on <strong>{DateTime}</strong>.</p>
                            <p>Thank you for staying involved in their academic journey.</p>
                            <br>
                            ";
                await SendEmailAsync(student.Guardian.Email, subject, body);
            }
        }

        public async Task SendEmailConfirmationAsync(User student)
        {
            var DateTime = DateTimeUtils.DateTimeNowFormattedString();
            var appUrl = VariableParser.GetEnvString("VITE_APP_URL");
            var route = $"{appUrl}/verify-email/{student.Id}";
            if (student.Email != null)
            {
                var subject = "Student's First Login";
                var body = $@"
                        <p>Hello <strong>{student.FirstName}</strong>,</p>
                        <p>Welcome aboard! 🎉 You're just one step away from completing your account setup.</p>
                        <p>Please complete your registration by clicking the link below:</p>
                        <p>
                            <a href='{route}' style='display:inline-block;padding:10px 20px;background-color:#4CAF50;color:white;text-decoration:none;border-radius:5px;'>
                                Complete Registration
                            </a>
                        </p>
                        <p>This link will expire on <strong>{DateTime}</strong>.</p>
                        <p>If you did not request this registration, please ignore this message.</p>
                        <br>
                        ";

                await SendEmailAsync(student.Email, subject, body);
            }
            if (student.Guardian?.Email != null)
            {
                var subject = "Guardian Confirmation to Student's Registration";
                var body = $@"
                        <p>Hello <strong>{student.Guardian.FirstName}</strong>,</p>
                        <p>We’re happy to inform you that you’ve been successfully registered as <strong>{student.FirstName}</strong>’s guardian.</p>
                        <p>From now on, you will receive notifications whenever <strong>{student.FirstName}</strong> logs their daily attendance.</p>
                        <p>If you have any questions or concerns, feel free to reach out to our support team.</p>
                        <br>
                    ";

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
