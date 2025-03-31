

using api.Utils;

namespace api.Models.Entities
{
    public class TwoFactorAuth
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string Code { get; set; } = IsInProductionEnv() ? OTPGenerator.GenerateOTP() : "000000";

        public DateTime Expiry { get; set; } = DateTimeUtils.TwoMinutesAfter();

        public bool IsExpired => DateTimeUtils.IsExpired(Expiry);

        public string Message => $"Your OTP is <strong>{Code}</strong>, and will expire in 2 minutes.";

        private static bool IsInProductionEnv()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
        }
    }
}