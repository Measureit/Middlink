using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Middlink.Authentication
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
