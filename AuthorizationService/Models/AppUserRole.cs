using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Models
{
    public class AppUserRole: IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
