using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class UpdateContentCreatorRequestDto : BaseUpdateUserRequestDto
{
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Numer NIP musi mieć 10 cyfr.")]
    public string? NIP { get; set; }

    [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi mieć 9 cyfr.")]
    public string? PhoneNumber { get; set; }

}
