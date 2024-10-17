using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto.Requests
{
    public class DeleteAccountRequestDto
    {
        [Required]
        public string Password { get; set; }

    }
}
