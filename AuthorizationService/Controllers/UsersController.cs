using AuthorizationService.Dto.Requests;
using AuthorizationService.Dto.Responses;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers;

[Route("users")]
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
    /*  [Authorize]*/
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
            _response.Message = "Failed to fetch users.";
            return BadRequest(_response);
        }
        _response.Result = usersResponse;

        return Ok(_response);
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult> GetUser()
    {
        try
        {
            var userResponse = await _userService.GetUser(User);
            if (userResponse == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to retrieve user data.";
                return BadRequest(_response);
            }
            _response.Result = userResponse;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while retrieving user data. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete]
    public async Task<ActionResult> RemoveUser([FromBody] DeleteAccountRequestDto passwordDto)
    {
        try
        {
            var errorMessage = await _userService.RemoveUser(passwordDto.Password, User);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while deleting the user account. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [Authorize(Policy = "RequireEndUserRole")]
    [HttpPut("end-user")]
    public async Task<IActionResult> UpdateEndUser([FromBody] BaseUpdateUserRequestDto user)
    {
        try
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
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while updating user data. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [Authorize(Policy = "RequireContentCreatorRole")]
    [HttpPut("content-creator")]
    public async Task<IActionResult> UpdateContentCreator([FromBody] UpdateContentCreatorRequestDto user)
    {
        try
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
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while updating content creator data. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [Authorize]
    [HttpPatch("password")]
    public async Task<IActionResult> ChangePasword([FromBody] ChangePasswordRequestDto passwordRequest)
    {
        try
        {
            var errorMessage = await _userService.ChangePassword(passwordRequest, User);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while changing the password. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("admin")]
    public async Task<IActionResult> UpdateAdmin([FromBody] BaseUpdateUserRequestDto user)
    {
        try
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
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while updating user admin data. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPatch("{username}/status")]
    public async Task<ActionResult> UpdateStatus([FromBody] UpdateUserStatusRequestDto isActive, string username)
    {
        try
        {
            var errorMessage = await _userService.UpdateStatus(isActive, username);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = $"An error occurred while updating user status. {ex.Message}";
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }
}
