using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto.Requests;
public class RegistrationContentCreatorRequestDto : BaseRegistrationRequestDto
{
    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Numer NIP musi mieć 10 cyfr.")]
    public string? NIP { get; set; }

    [Required]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi mieć 9 cyfr.")]
    public string? PhoneNumber { get; set; }
}
