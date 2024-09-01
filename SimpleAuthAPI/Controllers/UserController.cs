using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthAPI.Services;
using System.Security.Claims;

namespace SimpleAuthAPI.Controllers
{
    [Route("api/[controller]")] // Defines the base route for the controller
    [ApiController] // 
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Endpoint to get a list of users, restricted to "Admin" role
        [Authorize(Roles = "Admin")]
        [HttpGet("get-users")] 
        public IActionResult GetUsers()
        {
            // Retrieve the list of users from the user service
            var users = _userService.GetUsers();

            // Return the list of users with a description
            return Ok(new { Description = "List of Users", Value = users });
        }

        // Endpoint to get details of the currently signed-in user
        [Authorize] // Requires the user to be authenticated
        [HttpGet("get-signed-in-user")] // 
        public IActionResult GetSignedInUser()
        {
            // Retrieve the current user's claims 
            var currentUser = HttpContext.User;

            // Extract user details from claims
            var firstName = currentUser.Claims.FirstOrDefault(c => c.Type == "firstName")?.Value;
            var lastName = currentUser.Claims.FirstOrDefault(c => c.Type == "lastName")?.Value;
            var phone = currentUser.Claims.FirstOrDefault(c => c.Type == "phoneNumber")?.Value;

            // Check if the user has the "Admin" role
            var isAdmin = currentUser.HasClaim(c => c.Type == ClaimTypes.Role);

            // Return the current user's details
            return Ok(new { FirstName = firstName, LastName = lastName, Phone = phone, IsAdmin = isAdmin });
        }
    }
}
