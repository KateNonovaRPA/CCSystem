using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Context;
using static Models.GlobalConstants;
using Models.Contracts;
using Models.ViewModels;

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
            IQueryable<UserVM> q1 = db.Users.Where(x => x.isDeleted != true).Select
                          (
                              user => new UserVM()
                                  {
                                      UUID = user.UUID.ToString(),
                                      identityID = user.identityID.ToString(),
                                      administrationName = user.administrationName,
                                      administrationOId = user.administrationOId,
                                      employeeIdentifier = user.employeeIdentifier,
                                      employeeNames = user.employeeNames,
                                      employeePosition = user.employeePosition,
                                      lawReason = user.lawReason,
                                      processorID = user.processorID,
                                      remark = user.remark,
                                      serviceType = user.serviceType,
                                      serviceURI = user.serviceURI
                              }
                          );
            return q1;

        }


        public bool AddUser(UserVM _model)
        {
            Users _user = new Users();
            try
            {
                _user.UUID = new Guid();
                _user.identityID = Guid.Empty;
                _user.administrationName = _model.administrationName;
                _user.administrationOId = _model.administrationOId;
                _user.employeeIdentifier = _model.employeeIdentifier;
                _user.employeeNames = _model.employeeNames;
                _user.employeePosition = _model.employeePosition;
                _user.lawReason = _model.lawReason;
                _user.processorID = _model.processorID;
                _user.remark = _model.remark;
                _user.serviceType = _model.serviceType;
                _user.serviceURI = _model.serviceURI;
                _user.dateCreated = DateTime.Now;
                _user.dateUpdated = DateTime.Now;
                _user.isDeleted = false;

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
             
                Users _user = new Users();
       
                _user.administrationName = _model.administrationName;
                _user.administrationOId = _model.administrationOId;
                _user.employeeIdentifier = _model.employeeIdentifier;
                _user.employeeNames = _model.employeeNames;
                _user.employeePosition = _model.employeePosition;
                _user.lawReason = _model.lawReason;
                _user.processorID = _model.processorID;
                _user.remark = _model.remark;
                _user.serviceType = _model.serviceType;
                _user.identityID = Guid.Parse(_model.identityID);
                _user.serviceURI = _model.serviceURI;


                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.UUID == Guid.Parse(_model.UUID));

                    _user.dateCreated = saved.dateCreated;
                    _user.dateUpdated = DateTime.Now;

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
            Users q1 = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
            if (q1 != null) {
                _user.UUID = q1.UUID.ToString();
                _user.identityID = q1.identityID.ToString();
                _user.administrationName = q1.administrationName;
                _user.administrationOId = q1.administrationOId;
                _user.employeeIdentifier = q1.employeeIdentifier;
                _user.employeeNames = q1.employeeNames;
                _user.employeePosition = q1.employeePosition;
                _user.lawReason = q1.lawReason;
                _user.processorID = q1.processorID;
                _user.remark = q1.remark;
                _user.serviceType = q1.serviceType;
                _user.serviceURI = q1.serviceURI;
                _user.dateCreated = q1.dateCreated;
                _user.dateUpdated = q1.dateUpdated;
            };

         return _user;
        }

        public UserVM GetUserByIdentityID(Guid _identityID)
        {
            UserVM _user = new UserVM();
            Users q1 = db.Users.Where(x => x.identityID == _identityID).FirstOrDefault();
            if (q1 != null)
            {
                _user.UUID = q1.UUID.ToString();
                _user.identityID = q1.identityID.ToString();
                _user.administrationName = q1.administrationName;
                _user.administrationOId = q1.administrationOId;
                _user.employeeIdentifier = q1.employeeIdentifier;
                _user.employeeNames = q1.employeeNames;
                _user.employeePosition = q1.employeePosition;
                _user.lawReason = q1.lawReason;
                _user.processorID = q1.processorID;
                _user.remark = q1.remark;
                _user.serviceType = q1.serviceType;
                _user.serviceURI = q1.serviceURI;
                _user.dateCreated = q1.dateCreated;
                _user.dateUpdated = q1.dateUpdated;
            };

            return _user;
        }

        public bool DeleteUser(Guid _UUID)
        {
            try
            {
                Users user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
                user.isDeleted = true;
                user.dateDeleted = DateTime.Now;
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
                Users user = db.Users.Where(x => x.UUID == _UUID).FirstOrDefault();
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
