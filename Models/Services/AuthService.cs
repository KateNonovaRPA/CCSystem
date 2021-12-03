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
    public class AuthService : BaseService, IAuthService
    {

        public AuthService(MainContext context)
        {
            this.db = context;
        }

        public bool CheckAuthorization(String _identityID)
        {
            if (string.IsNullOrEmpty(_identityID)) { return false; }
            Users q1 = db.Users.Where(x => x.identityID == Guid.Parse(_identityID)).FirstOrDefault();
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
            if (_UUID!= Guid.Empty)
            {          
                Users _user = new Users();
                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.UUID == _UUID);

                    if (saved.identityID != Guid.Empty) { 
                        return false; 
                    };

                    _user.administrationName = saved.administrationName;
                    _user.administrationOId = saved.administrationOId;
                    _user.employeeIdentifier = saved.employeeIdentifier;
                    _user.employeeNames = saved.employeeNames;
                    _user.employeePosition = saved.employeePosition;
                    _user.lawReason = saved.lawReason;
                    _user.processorID = saved.processorID;
                    _user.remark = saved.remark;
                    _user.serviceType = saved.serviceType;
                    _user.serviceURI = saved.serviceURI;
                    _user.dateCreated = saved.dateCreated;
                    _user.dateUpdated = DateTime.Now;

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
                Users _user = new Users();
                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.UUID == _UUID);

                    if (saved.identityID == Guid.Empty)
                    {
                        return false;
                    };

                    _user.administrationName = saved.administrationName;
                    _user.administrationOId = saved.administrationOId;
                    _user.employeeIdentifier = saved.employeeIdentifier;
                    _user.employeeNames = saved.employeeNames;
                    _user.employeePosition = saved.employeePosition;
                    _user.lawReason = saved.lawReason;
                    _user.processorID = saved.processorID;
                    _user.remark = saved.remark;
                    _user.serviceType = saved.serviceType;
                    _user.serviceURI = saved.serviceURI;
                    _user.dateCreated = saved.dateCreated;
                    _user.dateUpdated = DateTime.Now;

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
