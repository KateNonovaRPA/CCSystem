using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entities;
using Models.Contracts;
using Models.ViewModels;
using System;
using System.Linq;

namespace Models.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(MainContext context)
        {
            this.db = context;
        }

        public IQueryable<UserVM> GetUsersList()
        {
            IQueryable<UserVM> q1 = db.Users.Where(x => x.deletedAt != null).Select
                          (
                              user => new UserVM()
                              {
                                  UUID = user.UUID.ToString(),
                                  identityID = user.identityID.ToString(),
                                  administrationName = user.administrationName.ToString(),
                                  name = user.name.ToString(),
                                  email = user.email.ToString(),
                                  createdAt = user.createdAt,
                                  updatedAt = user.updatedAt,
                                  deletedAt = (user.deletedAt !=null) ? user.deletedAt : null,
                              }
                          ); ;
            return q1;
        }

        public bool AddUser(UserVM _model)
        {
            User _user = new User();
            try
            {
                _user.UUID = new Guid();
                _user.identityID = Guid.Empty;
                _user.administrationName = _model.administrationName;
                _user.name = _model.name;
                _user.email = _model.email;
                _user.createdAt = _model.createdAt;

                db.Users.Add(_user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateUser(UserVM _model)
        {
            if (_model.UUID != "")
            {
                User _user = new User();

                _user.administrationName = _model.administrationName;
                _user.name = _model.name;
                _user.email = _model.email;
                _user.administrationName = _model.administrationName;
                _user.identityID = Guid.Parse(_model.identityID);

                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.UUID == Guid.Parse(_model.UUID));

                    _user.createdAt = saved.createdAt;
                    _user.updatedAt = DateTime.Now;

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

        public UserVM GetUserByUUID(Guid _UUID)
        {
            UserVM _user = new UserVM();
            User user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
            if (user != null)
            {
                _user.UUID = user.UUID.ToString();
                _user.identityID = user.identityID.ToString();
                _user.administrationName = user.administrationName;
                _user.administrationName = user.administrationName;
                _user.name = user.name;
                _user.email = user.email;
                _user.createdAt = user.createdAt;
                _user.updatedAt = user.updatedAt;
                _user.deletedAt = user.deletedAt;
            };

            return _user;
        }

        public UserVM GetUserByIdentityID(Guid _identityID)
        {
            UserVM _user = new UserVM();
            User user = db.Users.Where(x => x.identityID == _identityID).FirstOrDefault();
            if (user != null)
            {
                _user.UUID = user.UUID.ToString();
                _user.identityID = user.identityID.ToString();
                _user.administrationName = user.administrationName;
                _user.administrationName = user.administrationName;
                _user.name = user.name;
                _user.email = user.email;
                _user.createdAt = user.createdAt;
                _user.updatedAt = user.updatedAt;
                _user.deletedAt = user.deletedAt;
            };

            return _user;
        }

        public bool DeleteUser(Guid _UUID)
        {
            try
            {
                User user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
                user.deletedAt = DateTime.Now;
                db.Users.Update(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool HardDeleteUser(Guid _UUID)
        {
            try
            {
                User user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}