namespace AuthorizationService.Models.Dto;
public class ResponseDto
{
    public object? Result { get; set; }
    public bool? IsSuccess { get; set; } = true;
    public object? Message { get; set; } = "";
}
