using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthResponseType
    {
        Error,
        InvalidCredentials,
        OTPSent,
        InvalidOTP,
        ExpiredOTP,
        Success,
    }

    public static class AuthResponseMessages
    {
        public static readonly Dictionary<AuthResponseType, string> Messages = new()
        {
            { AuthResponseType.Error, "An error occurred. Please try again later." },
            { AuthResponseType.InvalidCredentials, "Invalid login credentials." },
            { AuthResponseType.InvalidOTP, "The OTP entered is invalid." },
            { AuthResponseType.ExpiredOTP, "The OTP has expired. Redirecting to the Login Page." },
            { AuthResponseType.OTPSent, "An OTP has been sent to your email." },
            { AuthResponseType.Success, "Logged in successfully." }
        };

        public static string GetMessage(AuthResponseType type)
        {
            return Messages.TryGetValue(type, out var message) ? message : "An error occurred. Please try again later.";
        }
    }

    public static class AuthResponseStatus
    {
        public static readonly Dictionary<AuthResponseType, int> Status = new()
        {
            { AuthResponseType.Error, 500 },               // Internal Server Error
            { AuthResponseType.InvalidCredentials, 401 },  // Unauthorized
            { AuthResponseType.InvalidOTP, 400 },          // Bad Request (wrong OTP)
            { AuthResponseType.ExpiredOTP, 410 },          // Gone (OTP is no longer valid)
            { AuthResponseType.OTPSent, 200 },             // OK
            { AuthResponseType.Success, 200 },             // OK
        };

        public static int GetStatus(AuthResponseType type)
        {
            return Status.TryGetValue(type, out int status) ? status : 500;
        }
    }
}