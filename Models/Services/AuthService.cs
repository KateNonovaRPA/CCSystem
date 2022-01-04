using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Context;
using Models.Contracts;
using Models.Entities;
using Models.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Models.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly string tokenKey;
        private DbContextOptions<MainContext> _dbContextOptions;

        public AuthService(string _tokenKey, DbContextOptions<MainContext> dbContextOptions)
        {
            this.tokenKey = _tokenKey;
            _dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// Check authorization by received access token.
        /// </summary>
        /// <param name="_accessToken"> user's access token</param>
        /// <param name="userType"> user type that has permission to the request </param>
        /// <returns>true or false</returns>
        public bool CheckAuthorization(String _accessToken, string userType)
        {
            if (string.IsNullOrEmpty(_accessToken)) { return false; }
            using (var db = new MainContext(_dbContextOptions))
            {
                User currentUser = db.Users.Where(x => x.AccessToken == _accessToken).FirstOrDefault();
                if (currentUser != null)
                {
                    if (currentUser.AccessToken != null)
                    {
                        //TODO: add type
                        if (userType.ToLower() == "robot" && currentUser.Type.ToLower() == userType.ToLower())
                        {
                            return true;
                        }
                        else if (userType.ToLower()=="user" && currentUser.Type.ToLower() != "robot")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    };
                };
            }
            return false;
        }

        /// <summary>
        /// Authorize user by send an access token.
        /// </summary>
        /// <param name="user"> User data</param>
        /// <returns>access token</returns>
        public string AuthorizeUser(UserVM user)
        {
            if (user!=null)
            {
                User _user = new User();
                try
                {
                    User currentUser = new User();
                    using (var db = new MainContext(_dbContextOptions))
                    {
                        currentUser = db.Users.AsNoTracking().FirstOrDefault(x => x.ClientID == Guid.Parse(user.clientID));
                        string newAccessToken = "";

                        if (currentUser == null || String.IsNullOrEmpty(currentUser.AccessToken))
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(tokenKey);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Expires = DateTime.UtcNow.AddYears(50),
                                SigningCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            newAccessToken = tokenHandler.WriteToken(token);
                        };
                        //update user
                        if (currentUser != null)
                        {
                            if (String.IsNullOrEmpty(currentUser.AccessToken))
                            {
                                currentUser.AccessToken = newAccessToken;
                            }
                            currentUser.FullName = user.fullName;
                            currentUser.Type = user.type;
                            currentUser.Email = user.email;
                            currentUser.updatedAt = DateTime.Now;
                            db.Users.Update(currentUser);
                            db.SaveChanges();
                            return currentUser.AccessToken;
                        }
                        else
                        {
                            _user.Email = user.email;
                            _user.FullName = user.fullName;
                            _user.Type = user.type;
                            _user.createdAt = DateTime.Now;
                            _user.ClientID =  Guid.Parse(user.clientID);
                            _user.AccessToken = newAccessToken;
                            db.Users.Add(_user);
                            db.SaveChanges();
                            return newAccessToken;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            return "Missing data";
        }

        public bool DeAuthorizeUser(string clientID)
        {
            if (!string.IsNullOrEmpty(clientID))
            {
                User _user = new User();
                try
                {
                    var saved = db.Users.AsNoTracking().FirstOrDefault(x => x.ClientID == Guid.Parse(clientID));

                    if (saved.ClientID == Guid.Empty)
                    {
                        return false;
                    };

                    _user.Email = saved.Email;
                    _user.FullName = saved.FullName;
                    _user.createdAt = saved.createdAt;
                    _user.updatedAt = DateTime.Now;
                    _user.deletedAt = saved.deletedAt;
                    _user.AccessToken = null;

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