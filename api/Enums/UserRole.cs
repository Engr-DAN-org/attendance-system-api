using System.Runtime.Serialization;

namespace api.Enums;

public enum UserRole
{
    [EnumMember(Value = "Teacher")]
    Teacher,

    [EnumMember(Value = "Student")]
    Student,

    [EnumMember(Value = "Admin")]
    Admin
}
