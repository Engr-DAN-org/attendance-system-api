
using api.Models.Entities;

namespace api.Interfaces.Service;

public interface IEmailService
{
    Task SendOTPEmailAsync(string toEmail, string body);

    Task SendAttendanceConfirmationEmailAsync(User student);
    Task SendRegistrationCredentialsAsync(User user, string password);

    Task SendClassClassStartedEmailAsync(List<User> students, string SubjectCode, DateTime dateTime);
    Task SendClassClassCanceledEmailAsync(List<User> students, string SubjectCode, DateTime dateTime);
    Task SendAbsentFromClassEmailAsync(List<User> students, string SubjectCode, DateTime dateTime);

}
