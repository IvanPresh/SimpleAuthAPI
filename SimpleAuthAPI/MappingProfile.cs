using AutoMapper;
using SimpleAuthAPI.DTO;
using SimpleAuthAPI.Entities;
using SimpleAuthAPI.Models;
using SimpleAuthAPI.Services;

namespace SimpleAuthAPI
{
    // Profile class for AutoMapper to configure mappings between entities and DTOs
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Configure mappings
            UserSignupMap();
            UserMap();
        }

        // Maps properties between UserSignUp and UserSignUpDTO
        private void UserSignupMap()
        {
            CreateMap<UserSignUp, UserSignUpDTO>();
            // This mapping configuration 
        }

        // Maps properties between User and UserDTO
        private void UserMap()
        {
            CreateMap<User, UserDTO>();

        }
    }
}
