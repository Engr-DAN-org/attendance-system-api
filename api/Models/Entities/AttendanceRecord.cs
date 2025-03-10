using System;

namespace api.Models.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }
    public int ClassScheduleId { get; set; }
    public required ClassSchedule ClassSchedule { get; set; }

    public bool IsOverRidden() => CreatedAt != UpdatedAt;

    //  Date for date record
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public required string StudentId { get; set; }
    public required User Student { get; set; }


    // ✅ Store student's location at scan time
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public float? Distance { get; set; }

    public bool IsLate() => TimeIn > ClassSchedule.StartTime.AddMinutes(ClassSchedule.GracePeriod);
    public TimeOnly TimeIn { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    public TimeOnly? TimeOut { get; set; }

    public string StudentName => Student.FullName;
    public string Subject => ClassSchedule.Subject.Name;
    public string Section => ClassSchedule.Section.Name;
    public string Teacher => ClassSchedule.Teacher?.FullName ?? "N/A";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // ✅ Haversine formula to calculate distance in meters
    private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371e3; // Radius of Earth in meters
        double dLat = (lat2 - lat1) * Math.PI / 180.0;
        double dLon = (lon2 - lon1) * Math.PI / 180.0;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c; // Distance in meters
    }
}
