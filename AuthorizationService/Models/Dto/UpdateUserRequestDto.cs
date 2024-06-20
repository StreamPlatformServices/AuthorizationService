namespace AuthorizationService.Models.Dto;
public class UpdateUserRequestDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? UserName { get; set; }
    public string? NIP { get; set; }
    public string? PhoneNumber { get; set; }

}
