using System;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs;

public class CreateGuardianDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? ContactNumber { get; set; }
    public required string Address { get; set; }
    public StudentGuardianRelationship Relationship { get; set; } = StudentGuardianRelationship.Parent;

}

public class UpdateGuardianDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? ContactNumber { get; set; }
    public required string Address { get; set; }
    public StudentGuardianRelationship Relationship { get; set; } = StudentGuardianRelationship.Parent;
}

public class GetGuardianDTO(Guardian guardian)
{
    public string FullName { get; set; } = $"{guardian.FirstName} {guardian.LastName}";
    public string Email { get; set; } = guardian.Email;
    public string? ContactNumber { get; set; } = guardian.ContactNumber;
    public string Address { get; set; } = guardian.Address;
    public StudentGuardianRelationship Relationship { get; set; } = guardian.Relationship;
}