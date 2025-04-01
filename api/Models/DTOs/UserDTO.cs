using System;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs;

public class AuthUserDTO(User user)
{
    public string Id { get; set; } = user.Id;
    public string FirstName { get; set; } = user.FirstName;
    public string LastName { get; set; } = user.LastName;
    public string FullName { get; set; } = user.FullName;
    public string? Email { get; set; } = user.Email;
    public string? PhoneNumber { get; set; } = user.PhoneNumber;
    public string IdNumber { get; set; } = user.IdNumber;
    public string Role { get; set; } = user.Role;
}

public class RegisterTeacherDTO
{

}

public class RegisterStudentDTO
{

}


