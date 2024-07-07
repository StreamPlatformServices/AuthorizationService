using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class ChangePasswordRequestDto
{
    [Required]
    public string OldPassword { set; get; }
    [Required]
    public string NewPassword { get; set; }
}
