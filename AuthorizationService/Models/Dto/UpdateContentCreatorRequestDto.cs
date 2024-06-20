namespace AuthorizationService.Models.Dto
{
    public class UpdateContentCreatorRequestDto : BaseUpdateUserRequestDto
    {
        public int? NIP { get; set; }
        public int? PhoneNumber { get; set; }

    }
}
