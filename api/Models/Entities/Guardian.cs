using System;
using api.Enums;

namespace api.Models.Entities;

public class Guardian
{
    public int Id { get; set; }
    public required string StudentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public required string Email { get; set; }
    public string? ContactNumber { get; set; }
    public required string Address { get; set; }
    public required User Student { get; set; }
    public StudentGuardianRelationship Relationship { get; set; } = StudentGuardianRelationship.Parent;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
