using System;
using api.Enums;

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

public class GetGuardianDTO
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? ContactNumber { get; set; }
    public required string Address { get; set; }
    public StudentGuardianRelationship Relationship { get; set; }

}