using System;
using api.Enums;
using api.Utils;

namespace api.Models.Entities;

public class AttendanceRecord
{
    // =============================================
    // 🆔 Primary Properties
    // =============================================
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Unmarked;
    public bool IsOverRidden { get; set; } = false;

    // =============================================
    // 🔗 Relationships
    // =============================================
    public required string ClassSessionId { get; set; }
    public ClassSession? ClassSession { get; set; }
    public required string StudentId { get; set; }
    public User? Student { get; set; }
    public string? OverriddenBy { get; set; } = null;

    // =============================================
    // 🕒 Timestamps
    // =============================================
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime? ClockInRecord { get; set; }
    public DateTime? OverriddenAt { get; set; } = null;
    public DateOnly Date { get; set; } = DateTimeUtils.DateNow();

    // =============================================
    // 🧭 Location & Distance
    // =============================================
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    private float? _distance = null;
    public float? Distance
    {
        get => _distance;
        set => _distance = value;
    }

    // =============================================
    // 🧾 Computed Properties
    // =============================================
    public string StudentName => Student?.FullName ?? "Deleted Student";

    // =============================================
    // 📏 Distance Calculation Methods
    // =============================================
    public void UpdateDistance(double? referenceLatitude, double? referenceLongitude)
    {
        Distance = GetCalculatedDistance(referenceLatitude, referenceLongitude);
    }

    public float? GetCalculatedDistance(double? referenceLatitude, double? referenceLongitude)
    {
        try
        {
            if (Latitude == null || Longitude == null ||
                referenceLatitude == null || referenceLongitude == null)
            {
                return null;
            }

            double distanceInMeters = HaversineDistance(
                Latitude.Value,
                Longitude.Value,
                referenceLatitude.Value,
                referenceLongitude.Value
            );

            return (float)distanceInMeters;
        }
        catch
        {
            return null;
        }
    }

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