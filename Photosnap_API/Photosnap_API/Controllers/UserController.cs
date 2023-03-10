using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Photosnap_API.Jwt;
using Photosnap_Mongodb.DTO_s.UserDTO;
using Photosnap_Mongodb.Service.UserService;

namespace Photosnap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IMongoDatabase database)
        {
            _userService= new UserService(database);
        }

        [HttpPost]
        [Route("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromForm] UserDTO userBasicDTO)
        {
            try
            {
                var response = await _userService.CreateUser(userBasicDTO);
                response.Token = JwtToken.GenerateToken(response.Username);
                return new JsonResult(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromForm] UserCredentialsDTO credentials)
        {
            try
            {
                var loginResult = await _userService.Login(credentials);
                if (loginResult.Username == credentials.Username)
                {
                    loginResult.Token = JwtToken.GenerateToken(loginResult.Username);
                    return new JsonResult(loginResult);
                }
                return BadRequest(loginResult.LoginInformation);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
