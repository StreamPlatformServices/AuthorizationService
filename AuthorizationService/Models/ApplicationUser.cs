using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
