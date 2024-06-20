using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers;
[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    protected ResponseDto _response;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
        _response = new();
    }

    [HttpPost("end-user")]
    public async Task<IActionResult> RegisterEndUser([FromBody] BaseRegistrationRequestDto model)
    {

        var errorMessage = await _authService.RegisterEndUser(model);
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

        var errorMessage = await _authService.RegisterContentCreator(model);
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

        var errorMessage = await _authService.RegisterAdminUser(model);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await _authService.Login(model);
        if (loginResponse == null)
        {
            _response.IsSuccess = false;
            _response.Message = "Nieprawidłowy login lub hasło.";
            return BadRequest(_response);
        }
        _response.Result = loginResponse;

        return Ok(_response);
    }

    [HttpGet("publickey")]
    public IActionResult GetTokenPublicKey()
    {
        var jwtPublicKey = _authService.GetPublicKey();
        if (string.IsNullOrEmpty(jwtPublicKey))
        {
            _response.IsSuccess = false;
            _response.Message = "Creation of token failed";
            return StatusCode(500, _response);
        }
        _response.Result = jwtPublicKey;
        return Ok(_response);

    }
}
