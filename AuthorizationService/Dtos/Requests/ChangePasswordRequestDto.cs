using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto.Requests;
public class ChangePasswordRequestDto
{
    [Required]
    public string OldPassword { set; get; }
    [Required]
    public string NewPassword { get; set; }
}
