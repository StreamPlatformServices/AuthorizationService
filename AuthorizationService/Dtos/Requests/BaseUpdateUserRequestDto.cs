using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto.Requests;
public class BaseUpdateUserRequestDto
{
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? UserName { get; set; }

}
