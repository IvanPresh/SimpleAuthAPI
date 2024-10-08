﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleAuthAPI.Models
{
    public class UserLogin
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}
