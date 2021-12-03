using System;
using System.Collections.Generic;
using System.Linq;
using Models.Context;
using Models.ViewModels;

namespace Models.Contracts
{
    public interface IUserService
    {
        IQueryable<UserVM> GetUsersList();
        bool AddUser(UserVM _model);
        bool UpdateUser(UserVM _model);
        UserVM GetUserByUUID(Guid _UUID);
        UserVM GetUserByIdentityID(Guid _identityID);
        bool DeleteUser(Guid _UUID);
        bool HardDeleteUser(Guid _UUID);

    }
}
