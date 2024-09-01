using Microsoft.AspNetCore.Mvc;
using SimpleAuthAPI.DTO;
using SimpleAuthAPI.Entities;

namespace SimpleAuthAPI.Services
{
    
    public interface IUserService
    {
        // Retrieve a user by username (email). Returns null if the user does not exist.
        Task<User> GetUserIfExists(string username);

        Task<User> CheckPassword(User user, string password);

        // Authenticate a user by username and password. 
        Task<User> AuthenticateUser(string username, string password);

        // Retrieve a list of roles assigned to the given user.
        Task<List<string>> GetRolesByUser(User user);

        Task<ObjectResult> CreateUser(UserSignUpDTO userSignupDto);

        // Retrieve a list of all users and map them to UserDTO.
        List<UserDTO> GetUsers();
    }
}
