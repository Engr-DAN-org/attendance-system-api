using System.Runtime.Serialization;

namespace api.Enums;

public enum StudentGuardianRelationship
{
    [EnumMember(Value = "Parent")]
    Parent,

    [EnumMember(Value = "Guardian")]
    Guardian,

    [EnumMember(Value = "Other")]
    Other
}
