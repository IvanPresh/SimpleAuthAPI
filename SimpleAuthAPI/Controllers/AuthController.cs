using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthAPI.DTO;
using SimpleAuthAPI.Models;
using SimpleAuthAPI.Services;

namespace SimpleAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //check if a user exists with the provided username and password
            var user = await _userService.AuthenticateUser(userLogin.Username, userLogin.Password);

            //if user credentials are not authentic return bad request with a message 
            if (user is null)
            {
                return this.Problem("username or password is incorrect", statusCode: 400);
            }

            //get roles 
            var roles = await _userService.GetRolesByUser(user);

            //generate a jwt token
            var token = _tokenService.GenerateToken(user, roles);

            //return token in the response
            return Ok(new { Token = token });

        }
        // [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserSignUp model)
        {

            var userSignUpDTO = _mapper.Map<UserSignUpDTO>(model);
            var result = await _userService.CreateUser(userSignUpDTO);

            return result;

        }


    }
}
