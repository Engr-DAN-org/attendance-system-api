using System.Runtime.Serialization;

namespace api.Enums
{
    public enum SortOrder
    {
        [EnumMember(Value = "asc")]
        asc,
        [EnumMember(Value = "desc")]
        desc
    }
}