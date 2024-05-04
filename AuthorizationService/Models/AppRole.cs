using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Models
{
    public class AppRole: IdentityRole<string>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
