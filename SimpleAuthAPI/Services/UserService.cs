using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthAPI.DTO;
using SimpleAuthAPI.Entities;

namespace SimpleAuthAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, SignInManager<User> signInManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // Authenticate a user by username and password
        public async Task<User> AuthenticateUser(string username, string password)
        {
            try
            {
                // Retrieve the user by username (email)
                var existingUser = await GetUserIfExists(username);
                if (existingUser != null)
                {
                    // Check if the provided password is correct
                    existingUser = await CheckPassword(existingUser, password);
                }
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        // Check if the provided password matches the user's hashed password
        public async Task<User> CheckPassword(User user, string password)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            // Sign in the user if the password is correct
            await _signInManager.PasswordSignInAsync(user.Email, password, false, false);

            return user;
        }

        // Create a new user and assign a role
        public async Task<ObjectResult> CreateUser(UserSignUpDTO userSignupDto)
        {
            // Check if the role assigned to the user exists
            var roleFound = await _roleManager.FindByNameAsync(userSignupDto.UserRole);

            if (roleFound == null)
            {
                _logger.LogError("Role with name {0} does not exist", userSignupDto.UserRole);
                return new BadRequestObjectResult($"UserRole '{userSignupDto.UserRole}' does not exist.");
            }

            // Create a new User entity from the signup DTO
            var user = new User
            {
                Email = userSignupDto.Email,
                EmailConfirmed = true,
                NormalizedEmail = userSignupDto.Email.ToUpper(),
                NormalizedUserName = userSignupDto.Email.ToUpper(),
                UserName = userSignupDto.Email,
                PhoneNumber = userSignupDto.PhoneNumber,
                FirstName = userSignupDto.FirstName,
                LastName = userSignupDto.LastName
            };

            // Create the user in the database
            var result = await _userManager.CreateAsync(user, userSignupDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Something went wrong. Failed to create user.");
                return new UnprocessableEntityObjectResult("Something went wrong. Failed to create user.");
            }

            // Assign the role to the newly created user
            var userResult = await _userManager.AddToRoleAsync(user, userSignupDto.UserRole);

            // Check if the role was assigned successfully
            if (!userResult.Succeeded)
            {
                _logger.LogInformation("Couldn't assign user a role");
                return new UnprocessableEntityObjectResult("Couldn't assign user a role");
            }

            return new OkObjectResult("User has been added successfully.");
        }

        // Retrieve a list of roles assigned to a user
        public async Task<List<string>> GetRolesByUser(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        // Retrieve a user by email
        public async Task<User> GetUserIfExists(string username)
        {
            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        // Retrieve and map a list of all users to UserDTO
        public List<UserDTO> GetUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();
                return _mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}
