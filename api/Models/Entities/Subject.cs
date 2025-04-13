using System;
using api.Utils;

namespace api.Models.Entities;

public class Subject
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public ClassSchedule[] ClassSchedules { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();
}
