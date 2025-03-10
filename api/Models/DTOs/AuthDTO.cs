using System;

namespace api.Models.DTOs;

public class LoginDTO
{
    public required string Identifier { get; set; } // Accepts Email or ID Number
    public required string Password { get; set; }
}

public class Validate2FARequestDTO
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
    public required string Token { get; set; }
    public DateTime Expiry { get; set; }
}
