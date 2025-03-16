

using api.Utils;

namespace api.Models.Entities
{
    public class TwoFactorAuth
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string Code { get; set; } = OTPGenerator.GenerateOTP();
        public DateTime Expiry { get; set; } = DateTimeUtils.TwoMinutesAfter();

        public bool IsExpired => DateTimeUtils.IsExpired(Expiry);

        public string Message => $"Your OTP is <strong>{Code}</strong>, and will expire in 2 minutes.";
    }
}