

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

        public string Message => $@"
                                <p>Your One-Time Password (OTP) is: <strong>{Code}</strong></p>
                                <p>This code will expire in <strong>2 minutes</strong>. Please do not share it with anyone.</p>
                                <p>If you did not request this, please ignore this message.</p>
                                ";
        private static bool IsInProductionEnv()
        {
            return VariableParser.GetEnvString("ASPNETCORE_ENVIRONMENT") == "Production";
        }
    }
}