﻿namespace SimpleAuthAPI.DTO
{
    public class UserSignUpDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; set; }
    }
}
