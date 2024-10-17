using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto.Requests;
public class BaseRegistrationRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [MaxLength(64)]
    [Required]
    public string Password { get; set; }
}
