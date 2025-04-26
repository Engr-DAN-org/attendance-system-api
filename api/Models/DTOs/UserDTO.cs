using System;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs;

public class AuthUserDTO(User user)
{
    public string Id { get; set; } = user.Id;
    public string IdNumber { get; set; } = user.IdNumber;
    public string FirstName { get; set; } = user.FirstName;
    public string LastName { get; set; } = user.LastName;
    public string FullName { get; set; } = user.FullName;
    public string? Email { get; set; } = user.Email;
    public string? PhoneNumber { get; set; } = user.PhoneNumber;
    public string Role { get; set; } = user.Role;
    public UserStatus Status { get; set; } = user.Status;

    public int? SectionId { get; set; } = user.SectionId;
    public int? GuardianId { get; set; } = user.GuardianId;

    //Relations
    public GetGuardianDTO? Guardian { get; set; } = user.Guardian != null ? new GetGuardianDTO(user.Guardian) : null;
    public GetSectionDTO? Section { get; set; } = user.Section != null ? new GetSectionDTO(user.Section, false) : null;

}

public class RegisterTeacherDTO
{

}

public class RegisterStudentDTO
{

}

public class RegisterUserDTO
{
    public required string IdNumber { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public UserRole UserRole { get; set; } = UserRole.Student;
    public string? PhoneNumber { get; set; }
    public int? SectionId { get; set; }
    public CreateGuardianDTO? Guardian { get; set; }
}


public class UsersQueryDTO : BaseQueryDTO<AuthUserDTO>
{
    public UsersQueryDTO(int totalCount, int totalPages, int page, int pageSize, List<User> data)
    {
        TotalCount = totalCount;
        TotalPages = totalPages;
        Page = page;
        PageSize = pageSize;
        Data = [.. data.Select(user => new AuthUserDTO(user))];
    }

}