using AuthorizationService.Models;

namespace AuthorizationService.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);

        string GetPublicKey();
    }
}
