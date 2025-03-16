using System;
using api.Enums;

namespace api.Utils;

public class RoleStringifierUtils
{
    public static readonly string Teacher = UserRole.Teacher.ToString();
    public static readonly string Admin = UserRole.Admin.ToString();
    public static readonly string Student = UserRole.Student.ToString();
    public static readonly string TeacherAdmin = $"{Teacher}, {Admin}";
}
