using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Models;
public class AppUser : IdentityUser
{

    public bool IsActive { get; set; } = false;
    public string Email { get; set; }
    public string Role { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NIP { get; set; }

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
