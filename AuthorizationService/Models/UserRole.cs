using System.ComponentModel;
using System.Runtime.Serialization;

namespace AuthorizationService.Models;
    public enum UserRole
    {

    [Description("Admin")]
    [EnumMember(Value = "Admin")]
    Admin,

    [Description("EndUser")]
    [EnumMember(Value = "EndUser")]
    EndUser,

    [Description("ContentCreator")]
    [EnumMember(Value = "ContentCreator")]
    ContentCreator
}
