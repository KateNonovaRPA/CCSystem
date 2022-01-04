using Microsoft.IdentityModel.Tokens;
using Models.Context;
using Models.Repositories;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Models.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService(MainContext context)
        {
            db = context;
        }
    }
}