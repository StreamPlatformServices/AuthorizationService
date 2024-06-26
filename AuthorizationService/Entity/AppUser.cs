using System.ComponentModel.DataAnnotations.Schema;
using AuthorizationService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Entity;
public class AppUser : IdentityUser
{

    public bool IsActive { get; set; } = false;
    public string Role { get; set; }
    public long? PhoneNumber { get; set; }
    public long? NIP { get; set; }

    [NotMapped]
    public UserRole UserRoleEnum
    {
        get
        {
            if (Enum.TryParse(Role, out UserRole roleValue))
            {
                return roleValue;
            }

            return UserRole.EndUser;
        }
        set => Role = value.ToString();
    }
}
