using System;
using System.Net;
using api.Enums;

namespace api.Models.DTOs;

public class LoginDTO
{
    public required string EmailOrIdNo { get; set; } // Accepts Email or ID Number
    public required string Password { get; set; }
}
public class TwoFactorPromptDTO
{
    public bool Success { get; set; } = true;
    public DateTime Expiry { get; set; } = DateTime.Now.AddMinutes(2);
    public string? Email { get; set; }
}
public class TwoFactorRequestDTO
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}

public class LogoutDTO
{
    public required string Token { get; set; } // Frontend will send the token for invalidation
}

public class AuthResponseDTO
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string Message { get; set; } = "Logged In Successfully.";
    public string? Token { get; set; }
    public DateTime? Expiry { get; set; }
    public string? Role { get; set; }
}


