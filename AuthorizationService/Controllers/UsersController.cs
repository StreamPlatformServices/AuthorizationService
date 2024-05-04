using Microsoft.AspNetCore.Mvc;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;

namespace AuthorizationService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        protected ResponseDto _response;

        public UsersController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsersResponseDto))]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var usersResponse = await _userService.GetUsersWithRoles();
            if (usersResponse.Users == null)
            {
                _response.IsSuccess = false;
                _response.Message = "An error occurred."; ;
                return BadRequest(_response);
            }
            _response.Result = usersResponse;
            return Ok(_response);
        }

        [HttpPost("assign-role/{id}")]
        public async Task<IActionResult> AssignRole(string id, [FromQuery] string roles)
        {
            var errorMessage = await _userService.AssignRole(id, roles.ToUpper());
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
