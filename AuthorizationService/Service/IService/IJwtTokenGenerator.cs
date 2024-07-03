using AuthorizationService.Models;

namespace AuthorizationService.Service.IService;
public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser applicationUser, UserRole role);

    string GetPublicKey();
}
