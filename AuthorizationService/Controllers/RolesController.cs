using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthorizationService.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(Roles = "ADMIN")]

    public class RolesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        protected ResponseDto _response;

        public RolesController(IConfiguration configuration, IRoleService roleService)
        {
            _configuration = configuration;
            _roleService = roleService;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RolesResponseDto))]
        public async Task<ActionResult> GetAllRoles()
        {
            var roleResponse = await _roleService.GetAllRoles();
            if (roleResponse == null)
            {
                _response.IsSuccess = false;
                _response.Message = "An error occurred."; ;
                return BadRequest(_response);
            }
            _response.Result = roleResponse;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string role)
        {
            var errorMessage = await _roleService.CreateRole(role);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string role)
        {
            var errorMessage = await _roleService.DeleteRole(role);
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
