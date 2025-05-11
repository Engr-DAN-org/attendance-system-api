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
                await SendEmailAsync([student.Email], subject, body);
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
                await SendEmailAsync([student.Guardian.Email], subject, body);
            }
        }

        public async Task SendRegistrationCredentialsAsync(User student, string password)
        {
            var DateTime = DateTimeUtils.DateTimeNowFormattedString();
            var appUrl = VariableParser.GetEnvString("VITE_APP_URL");
            var route = $"{appUrl}/sign-in";

            if (student.Email != null)
            {
                var subject = "Student's First Login";
                var body = $@"
                    <p>Hello <strong>{student.FirstName}</strong>,</p>
                    <p>Welcome aboard! 🎉 You're now ready to log in to your account.</p>
                    <p>Your login credentials are as follows:</p>
                    <ul>
                        <li><strong>ID Number:</strong> {student.IdNumber}</li>
                        <li><strong>Password:</strong> {password}</li>
                    </ul>
                    <p>You can log in by clicking the link below:</p>
                    <p>
                        <a href='{route}' style='display:inline-block;padding:10px 20px;background-color:#4CAF50;color:white;text-decoration:none;border-radius:5px;'>
                            Sign In to Your Account
                        </a>
                    </p>
                    <p>We recommend to change your password after successful login.</p>
                    <br>
                    <p>If you did not request this registration, please ignore this message.</p>
                    <br>
                    ";

                await SendEmailAsync([student.Email], subject, body);
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

                await SendEmailAsync([student.Guardian.Email], subject, body);
            }
        }

        public async Task SendClassClassStartedEmailAsync(List<User> students, string SubjectCode, DateTime dateTime)
        {
            var formattedDateTime = DateTimeUtils.ToPhTimeString(dateTime);
            var appUrl = VariableParser.GetEnvString("VITE_APP_URL");

            var subject = "Class Started";

            var body = $@"
                    <p>Dear Student,</p>

                    <p>This is a friendly reminder that your class <strong>{SubjectCode}</strong> has just started on <strong>{formattedDateTime}</strong>.</p>

                    <p>Please open the student app or click the link below to log in and mark your attendance by scanning the QR code provided by your teacher:</p>

                    <p>
                        <a href='{appUrl}/sign-in' style='display:inline-block;padding:10px 20px;background-color:#007bff;color:white;text-decoration:none;border-radius:5px;'>
                            Log in to Web App
                        </a>
                    </p>

                    <p>Make sure to join the class on time and participate actively.</p>

                    <br />
                    <p>Thank you and enjoy your class!</p>
                ";


            var emailAddresses = students
                .Where(r => !string.IsNullOrEmpty(r.Email))
                .Select(r => r.Email!)
                .ToList();

            await SendEmailAsync(emailAddresses, subject, body);
        }

        public async Task SendClassClassCanceledEmailAsync(List<User> students, string SubjectCode, DateTime dateTime)
        {
            var formattedDateTime = DateTimeUtils.ToPhTimeString(dateTime);
            var subject = "Class Cancelation Notification";

            var body = $@"
                <p>Dear Student,</p>

                <p>We would like to inform you that your scheduled class for <strong>{SubjectCode}</strong> on <strong>{formattedDateTime}</strong> has been <strong>CANCELED</strong>.</p>

                <p>Please wait for further updates or rescheduling information from your teacher or the school administration.</p>

                <br />
                <p>Thank you for your understanding.</p>
                ";

            var emailAddresses = students
                .Where(r => !string.IsNullOrEmpty(r.Email))
                .Select(r => r.Email!)
                .ToList();

            await SendEmailAsync(emailAddresses, subject, body);
        }


        public async Task SendAbsentFromClassEmailAsync(List<User> students, string SubjectCode, DateTime dateTime)
        {
            var formattedDateTime = DateTimeUtils.ToPhTimeString(dateTime);
            var subject = "Class Session Ended";

            var body = $@"
            <p>Dear Student,</p>

            <p>We would like to inform you that you were marked as <strong>ABSENT</strong> for the class session of <strong>{SubjectCode}</strong>, which concluded on <strong>{formattedDateTime}</strong>.</p>

            <p>If you believe this record is incorrect, please contact your teacher as soon as possible to clarify the matter.</p>

            <br />
            <p>Thank you for your attention.</p>
            ";

            var emailAddresses = students
                .Where(r => !string.IsNullOrEmpty(r.Email))
                .Select(r => r.Email!)
                .ToList();

            await SendEmailAsync(emailAddresses, subject, body);
        }




        public async Task SendOTPEmailAsync(string toEmail, string body)
        {
            var subject = "2FA Verification";
            await SendEmailAsync([toEmail], subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string subject, string body)
        {
            await SendEmailAsync([toEmail], subject, body);
        }

        private async Task SendEmailAsync(List<string> toEmails, string subject, string body)
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

                foreach (var email in toEmails)
                {
                    mailMessage.To.Add(email);
                }

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
