using System;
using System.Collections.Generic;
using System.Linq;
using Models.Context;
using Models.ViewModels;

namespace Models.Contracts
{
    public interface IAuthService
    {
        bool CheckAuthorization(String _UUID);
        bool AuthorizeUser(Guid _UUID);
        bool DeAuthorizeUser(Guid _UUID);
    }
}
