using System;
using api.Enums;

namespace api.Models.DTOs;

public class AuthResponse
{
    public required string Token { get; set; }
    public DateTime Expires { get; set; }
    public UserRole Role { get; set; }
}
