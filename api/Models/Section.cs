using System;

namespace api.Models;

public class Section
{
    public int Id { get; set; }
    public required int YearLevel { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";

    public string? TeacherId { get; set; }
    public User? Teacher { get; set; }
    public User[] Students { get; set; } = [];

    public ClassSchedule[] ClassSchedules { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
