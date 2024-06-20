using AuthorizationService.Models;

namespace AuthorizationService.Extensions
{
    public static class AuthorizationServiceExtension
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(UserRole.Admin.ToString()));
                options.AddPolicy("RequireEndUserRole", policy => policy.RequireRole(UserRole.EndUser.ToString()));
                options.AddPolicy("RequireContentCreatorRole", policy => policy.RequireRole(UserRole.ContentCreator.ToString()));
            });

            return services;
        }
    }
}
