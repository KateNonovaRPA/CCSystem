using System;

namespace Models.Contracts
{
    public interface IAuthService
    {
        bool CheckAuthorization(String _UUID);

        bool AuthorizeUser(Guid _UUID);

        bool DeAuthorizeUser(Guid _UUID);

        
    }
}