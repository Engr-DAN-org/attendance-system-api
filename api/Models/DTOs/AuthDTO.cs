using System;
using System.Net;
using api.Enums;
using api.Utils;

namespace api.Models.DTOs;

public class LoginDTO
{
    public required string EmailOrIdNo { get; set; } // Accepts Email or ID Number
    public required string Password { get; set; }
}

public class TwoFactorRequestDTO
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}

public class TwoFactorResponseDTO
{
    public AuthResponseType ResponseType { get; set; } = AuthResponseType.OTPSent;
    public string Message => AuthResponseMessages.GetMessage(ResponseType);
    public DateTime Expiry { get; set; } = DateTimeUtils.TwoMinutesAfter();
    public string? Email { get; set; }
}


public class AuthResponseDTO
{
    public AuthResponseType ResponseType { get; set; } = AuthResponseType.Success;
    public string Message => AuthResponseMessages.GetMessage(ResponseType);
    public string? Token { get; set; }
    public DateTime? Expiry { get; set; }
    public string? Role { get; set; }
}


