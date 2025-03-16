using System;

namespace api.Models.DTOs;

public class EmailSettings
{
    public required string Server { get; set; }
    public required int Port { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
