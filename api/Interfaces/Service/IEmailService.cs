
using api.Models.Entities;

namespace api.Interfaces.Service;

public interface IEmailService
{
    Task SendOTPEmailAsync(string toEmail, string body);

    Task SendAttendanceConfirmationEmailAsync(User student);
}
