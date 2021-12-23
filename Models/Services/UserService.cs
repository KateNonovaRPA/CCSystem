﻿using Microsoft.EntityFrameworkCore;
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
                                  identityID = user.identityID.ToString(),
                                  firstName = user.FirstName.ToString(),
                                  lastName = user.LastName.ToString(),
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
                _user.identityID = Guid.Empty;
                _user.Email = _model.email;
                _user.FirstName = _model.firstName;
                _user.LastName = _model.lastName;
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

        //Update already existing user
        public bool UpdateUser(UserVM _model)
        {
            if (_model.UUID != "")
            {
                User _user = new User();

                _user.Email = _model.email;
                _user.FirstName = _model.firstName;
                _user.LastName = _model.lastName;
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

        //Get a user by GUID
        public UserVM GetUserByUUID(Guid _UUID)
        {
            UserVM _user = new UserVM();
            User user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
            if (user != null)
            {
                _user.UUID = user.UUID.ToString();
                _user.identityID = user.identityID.ToString();
                _user.email = user.Email;
                _user.firstName = user.FirstName;
                _user.lastName = user.LastName;
                _user.createdAt = user.createdAt;
                _user.updatedAt = user.updatedAt;
                _user.deletedAt = user.deletedAt;
            };

            return _user;
        }

        //Get a user by ID
        public UserVM GetUserByIdentityID(Guid _identityID)
        {
            UserVM _user = new UserVM();
            User user = db.Users.Where(x => x.identityID == _identityID).FirstOrDefault();
            if (user != null)
            {
                _user.UUID = user.UUID.ToString();
                _user.identityID = user.identityID.ToString();
                _user.email = user.Email;
                _user.firstName = user.FirstName;
                _user.lastName = user.LastName;
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
                return false;
            }
        }
    }
}