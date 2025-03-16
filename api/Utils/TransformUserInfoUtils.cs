using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Utils
{
    public class TransformUserInfoUtils
    {
        public static GetStudentDTO GetStudentInfo(User student)
        {
            return new GetStudentDTO
            {
                Role = student.Role,
                FullName = student.FullName,
                Email = student.Email ?? string.Empty,
                YearLevel = student.Section != null ? student.Section.YearLevel : 0,
                IdNumber = student.IdNumber,
                Section = student.Section != null ? student.Section.Name : string.Empty,
                Guardian = student.Guardian != null ? new GetGuardianDTO
                {
                    FullName = student.Guardian.FullName,
                    Email = student.Guardian.Email,
                    Address = student.Guardian.Address,
                } : null,
                AttendanceRecords = student.AttendanceRecords
            };
        }

        public static GetTeacherDTO GetTeacherInfo(User teacher)
        {
            return new GetTeacherDTO
            {
                Role = teacher.Role,
                FullName = teacher.FullName,
                Email = teacher.Email ?? string.Empty,
                IdNumber = teacher.IdNumber,
                ClassSchedules = teacher.ClassSchedules,
                ClassSessions = teacher.ClassSessions,
            };
        }
    }
}