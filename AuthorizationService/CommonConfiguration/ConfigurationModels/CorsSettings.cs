namespace AuthorizationService.CommonConfiguration.ConfigurationModels;
public class CorsSettings
{
    public string[] AllowedHosts { get; set; }
    public string[] AllowedHeaders { get; set; }
    public string[] AllowedMethods { get; set; }
}
