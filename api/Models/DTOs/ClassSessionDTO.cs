using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class CreateClassSessionDTO
    {
        public required int ClassScheduleId { get; set; }
        public DateTime? StartTime { get; set; }
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? GraceTime { get; set; }

        public ClassSession ToClassSession(ClassSchedule classSchedule)
        {
            return new ClassSession
            {
                ClassScheduleId = classSchedule.Id,
                StartTime = StartTime ?? DateTime.Parse(classSchedule.StartTime),
                Location = Location,
                Latitude = Latitude,
                Longitude = Longitude,
                GraceTime = GraceTime,
            };
        }
    }
}