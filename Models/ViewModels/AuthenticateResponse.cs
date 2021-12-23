using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(UserVM user, string token)
        {
            Id = user.UUID;
            FirstName = user.firstName;
            LastName = user.lastName;
            Email = user.email;
            Token = token;
        }
    }
}
