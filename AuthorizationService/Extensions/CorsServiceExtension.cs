namespace AuthorizationService.Extensions
{
    public static class CorsServiceExtension
    {
        public static IServiceCollection AddCorsServices(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });
            return services;
        }

    }
}
