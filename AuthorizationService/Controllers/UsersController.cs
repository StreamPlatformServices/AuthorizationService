using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers;

[Route("users")]
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    protected ResponseDto _response;

    public UsersController(IUserService userService)
    {
        _userService = userService;
        _response = new ResponseDto();
    }

    [HttpPost("end-user")]
    public async Task<IActionResult> RegisterEndUser([FromBody] BaseRegistrationRequestDto model)
    {

        var errorMessage = await _userService.RegisterEndUser(model);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    [HttpPost("content-creator")]
    public async Task<IActionResult> RegisterContentCreator([FromBody] RegistrationContentCreatorRequestDto model)
    {

        var errorMessage = await _userService.RegisterContentCreator(model);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    /*        [Authorize(Policy = "RequireAdminRole")]*/ // TODO: Uncomment
    [HttpPost("admin-user")]
    public async Task<IActionResult> RegisterAdminUser([FromBody] BaseRegistrationRequestDto model)
    {

        var errorMessage = await _userService.RegisterAdminUser(model);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsersResponseDto))]
    public async Task<ActionResult> GetUsers()
    {
        var usersResponse = await _userService.GetUsers();
        if (usersResponse.Users == null)
        {
            _response.IsSuccess = false;
            _response.Message = "Nie udało się pobrać użytkowników."; ;
            return BadRequest(_response);
        }
        _response.Result = usersResponse;

        return Ok(_response);
    }

    [HttpGet("user")]
    public async Task<ActionResult> GetUser()
    {
        var userResponse = await _userService.GetUser(User);
        if (userResponse == null)
        {
            _response.IsSuccess = false;
            _response.Message = "Nie udało się pobrać danych użytkownika.";
            return BadRequest(_response);
        }
        _response.Result = userResponse;

        return Ok(_response);
    }

    [HttpDelete("user")]
    public async Task<ActionResult> RemoveUser()
    {
        var errorMessage = await _userService.RemoveUser(User);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    [Authorize(Policy = "RequireEndUserRole")]
    [HttpPut("end-user")]
    public async Task<IActionResult> UpdateEndUser([FromBody] BaseUpdateUserRequestDto user)
    {

        var errorMessage = await _userService.UpdateEndUser(user, User);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        return Ok(_response);

    }

    [Authorize(Policy = "RequireContentCreatorRole")]
    [HttpPut("content-creator")]
    public async Task<IActionResult> UpdateContentCreator([FromBody] UpdateContentCreatorRequestDto user)
    {

        var errorMessage = await _userService.UpdateContentCreator(user, User);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        return Ok(_response);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("admin")]
    public async Task<IActionResult> UpdateAdmin([FromBody] BaseUpdateUserRequestDto user)
    {

        var errorMessage = await _userService.UpdateAdmin(user, User);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        return Ok(_response);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("{username}/status")]
    public async Task<ActionResult> UpdateStatus(string username)
    {
        var errorMessage = await _userService.UpdateStatus(username);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
}
