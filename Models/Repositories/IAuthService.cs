﻿using Models.ViewModels;
using System;

namespace Models.Contracts
{
    public interface IAuthService
    {
        bool CheckAuthorization(string _accessToken, string userType);

        string AuthorizeUser(UserVM user);

        bool DeAuthorizeUser(string clientID);
        bool ValidateAPIKey(string integratorKey);

        resAuthRequestVM AuthCode(AuthRequestVM auth);
        resTokenVM GetAccessToken(string authorizationCode);



    }
}