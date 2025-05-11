using System.Runtime.Serialization;

namespace api.Enums
{
    public enum AttendanceStatus
    {
        [EnumMember(Value = "Present")]
        Present,

        [EnumMember(Value = "Absent")]
        Absent,

        [EnumMember(Value = "Late")]
        Late,

        [EnumMember(Value = "Excused")]
        Excused,

        [EnumMember(Value = "Unmarked")]
        Unmarked,
    }
}