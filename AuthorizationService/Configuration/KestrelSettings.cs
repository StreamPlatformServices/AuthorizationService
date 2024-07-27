namespace AuthorizationService.Configuration;
public class KestrelSettings
{
    public string ListeningIPv4Address { set; get; } = "127.0.0.1";
    public int PortNumber { set; get; } = 7060;
    public int TlsPortNumber { set; get; } = 7061;
    public bool UseTls { set; get; } = true;
}
