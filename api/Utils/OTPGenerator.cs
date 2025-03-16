// Purpose: Contains the OTPGenerator class which is used to generate a random 6 digit OTP.
// The OTP is generated using the GenerateOTP method which returns a random 6 digit number.
namespace api.Utils
{
    public class OTPGenerator
    {
        public static string GenerateOTP()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString();
        }
    }
}