using Microsoft.AspNetCore.Mvc;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationService.Controllers
{
    [Route("users")]
    [Authorize]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserService _userService;
        protected ResponseDto _response;

        public UsersController(IUserService userService)
        {
            _userService = userService;
            _response = new ResponseDto();
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

        [HttpGet("getUser")]
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


        [Authorize(Policy = "RequireEndUserRole")]
        [HttpPut("updateEndUser")]
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
        [HttpPut("updateContentCreator")]
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
}
