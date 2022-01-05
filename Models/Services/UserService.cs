using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Contracts;
using Models.Entities;
using Models.ViewModels;
using System;
using System.Linq;

namespace Models.Services
{
    public class UserService : BaseService, IUserService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserService(MainContext context)
        {
            this.db = context;
        }

        //Get list of all users
        public IQueryable<UserVM> GetUsersList()
        {
            IQueryable<UserVM> q1 = db.Users.Where(x => x.deletedAt != null).Select
                          (
                              user => new UserVM()
                              {
                                  UUID = user.UUID.ToString(),
                                  fullName = user.FullName.ToString(),
                                  type = user.Type.ToString(),
                                  email = user.Email.ToString(),
                                  createdAt = user.createdAt,
                                  updatedAt = user.updatedAt,
                                  deletedAt = (user.deletedAt !=null) ? user.deletedAt : null,
                              }
                          ); ;
            return q1;
        }

        //Create new user
        public bool AddUser(UserVM _model)
        {
            User _user = new User();
            try
            {
                _user.UUID = new Guid();
                _user.AccessToken = _model.AccessToken;
                _user.Email = _model.email;
                _user.FullName = _model.fullName;
                _user.Type = _model.type;
                _user.createdAt = _model.createdAt;

                db.Users.Add(_user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        //Update already existing user
        public bool UpdateUser(UserVM _model)
        {
            if (_model.UUID != "")
            {
                User _user = new User();

                _user.Email = _model.email;
                _user.FullName = _model.fullName;
                _user.Type = _model.type;
                _user.AccessToken = _model.AccessToken;

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
                    log.Error(ex.Message);
                    return false;
                }
            }
            return false;
        }

        //Get a user by GUID
        public UserVM GetUserByUUID(Guid _UUID)
        {
            UserVM _user = new UserVM();
            User user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
            if (user != null)
            {
                _user.UUID = user.UUID.ToString();
                _user.email = user.Email;
                _user.fullName = user.FullName;
                _user.type = user.Type;
                _user.createdAt = user.createdAt;
                _user.updatedAt = user.updatedAt;
                _user.deletedAt = user.deletedAt;
            };

            return _user;
        }

        //Get a user by AccessToken
        public UserVM GetUserByAccessToken(string _accessToken)
        {
            UserVM _user = new UserVM();
            User user = db.Users.Where(x => x.AccessToken == _accessToken).FirstOrDefault();
            if (user != null)
            {
                _user.UUID = user.UUID.ToString();
                _user.email = user.Email;
                _user.fullName = user.FullName;
                _user.type = user.Type;
                _user.createdAt = user.createdAt;
                _user.updatedAt = user.updatedAt;
                _user.deletedAt = user.deletedAt;
            };

            return _user;
        }

        //Delete a user by GUID
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
                log.Error(ex.Message);
                return false;
            }
        }

        //Hard delete a user by GUID
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
                log.Error(ex.Message);
                return false;
            }
        }
    }
}