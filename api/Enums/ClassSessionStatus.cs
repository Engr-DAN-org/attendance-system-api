using System.Runtime.Serialization;

namespace api.Enums
{
    public enum ClassSessionStatus
    {
        [EnumMember(Value = "Created")]
        Created,

        [EnumMember(Value = "Started")]
        Started,

        [EnumMember(Value = "Ended")]
        Ended,

        [EnumMember(Value = "Canceled")]
        Canceled,
    }
}