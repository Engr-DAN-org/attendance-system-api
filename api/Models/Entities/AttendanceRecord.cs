using System;
using api.Utils;

namespace api.Models.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }

    // 🔗 Relationships
    public int ClassScheduleId { get; set; }
    public ClassSchedule? ClassSchedule { get; set; }
    public required string StudentId { get; set; }
    public User? Student { get; set; }

    // 🕒 Timestamps
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime ClockInRecord { get; set; } = DateTimeUtils.DateTimeNow();
    public DateOnly Date { get; set; } = DateTimeUtils.DateNow();

    // 🧭 Location details
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public float? Distance { get; set; }

    // 🕓 Attendance times
    public TimeOnly TimeIn { get; set; } = DateTimeUtils.TimeNow();
    public TimeOnly? TimeOut { get; set; }

    // 🧠 Logic
    public bool IsLate()
    {
        if (TimeOnly.TryParse(ClassSchedule?.StartTime, out var startTime))
        {
            var allowedTime = startTime.AddMinutes(ClassSchedule.GracePeriod);
            return TimeIn > allowedTime;
        }

        // If invalid StartTime, consider not late or log the error externally
        return false;
    }

    public bool IsOverRidden() => CreatedAt != UpdatedAt;

    // 🧾 Display helpers
    public string StudentName => Student?.FullName ?? "Deleted Student";
    public string SubjectName => ClassSchedule?.Subject?.Name ?? "Deleted Subject";
    public string SectionName => ClassSchedule?.Section?.Name ?? "Deleted Section";
    public string Teacher => ClassSchedule?.Teacher?.FullName ?? "N/A";

    // 📏 Distance calculation
    private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371e3; // Radius of Earth in meters
        double dLat = (lat2 - lat1) * Math.PI / 180.0;
        double dLon = (lon2 - lon1) * Math.PI / 180.0;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}
