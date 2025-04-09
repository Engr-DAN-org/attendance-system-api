using System.Runtime.Serialization;

namespace api.Enums
{
    public enum UserStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Inactive")]

        Inactive,

        [EnumMember(Value = "Suspended")]
        Suspended,
    }
}