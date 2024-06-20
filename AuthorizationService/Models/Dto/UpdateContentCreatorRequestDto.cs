namespace AuthorizationService.Models.Dto;
public class UpdateContentCreatorRequestDto : BaseUpdateUserRequestDto
{
    public string? NIP { get; set; }
    public string? PhoneNumber { get; set; }

}
