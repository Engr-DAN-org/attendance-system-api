using System;
using api.Utils;

namespace api.Models.Entities;

public class Subject
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public ClassSchedule[] ClassSchedules { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();
}
