using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entities;
using Models.Contracts;
using System;
using System.Linq;

namespace Models.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(MainContext context)
        {
            this.db = context;
        }

        public bool CheckAuthorization(String _identityID)
        {
            if (string.IsNullOrEmpty(_identityID)) { return false; }
            User q1 = db.Users.Where(x => x.identityID == Guid.Parse(_identityID)).FirstOrDefault();
            if (q1 != null)
            {
                if (q1.identityID != null)
                {
                    return true;
                };
            };
            return false;
        }

        public bool AuthorizeUser(Guid _UUID)
        {
            if (_UUID != Guid.Empty)
            {
                User _user = new User();
                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.UUID == _UUID);

                    if (saved.identityID != Guid.Empty)
                    {
                        return false;
                    };
                    _user.administrationName = saved.administrationName;
                    _user.email = saved.email;
                    _user.name = saved.name;
                    _user.createdAt = saved.createdAt;
                    _user.updatedAt = DateTime.Now;
                    _user.deletedAt = saved.deletedAt;
                    _user.identityID = Guid.NewGuid();
                    db.Users.Update(_user);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        public bool DeAuthorizeUser(Guid _UUID)
        {
            if (_UUID != Guid.Empty)
            {
                User _user = new User();
                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.UUID == _UUID);

                    if (saved.identityID == Guid.Empty)
                    {
                        return false;
                    };

                    _user.administrationName = saved.administrationName;
                    _user.email = saved.email;
                    _user.name = saved.name;
                    _user.createdAt = saved.createdAt;
                    _user.updatedAt = DateTime.Now;
                    _user.deletedAt = saved.deletedAt;
                    _user.identityID = Guid.Empty;

                    db.Users.Update(_user);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
    }
}