using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly string APIKey;
        private DbContextOptions<MainContext> _dbContextOptions;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AuthService(string _APIKey, DbContextOptions<MainContext> dbContextOptions)
        {
            this.APIKey = _APIKey;
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
        /// 

        public resAuthRequestVM AuthCode(AuthRequestVM auth)
        {
            resAuthRequestVM authRes = new resAuthRequestVM();
            if (auth.ClientID == null || auth.APIKey == null || auth.user == null)
            {
                return null;
            }
            else
            {
                if (auth.APIKey != APIKey)
                    return null;
                else
                {
                    User _user = new User();
                    try
                    {
                        User currentUser = new User();
                        using (var db = new MainContext(_dbContextOptions))
                        {

                            currentUser = db.Users.AsNoTracking().FirstOrDefault(x => x.ClientID == Guid.Parse(auth.ClientID));
                            string newClientSecret = "";
                            newClientSecret = Guid.NewGuid().ToString();
                            //if (currentUser == null || String.IsNullOrEmpty(currentUser.ClientSecret))
                            //{
                            //    newClientSecret = Guid.NewGuid().ToString();
                            //};
                            if (currentUser != null) //update user
                            {
                                //if (!String.IsNullOrEmpty(currentUser.AccessToken))
                                //{
                                //    
                                //}
                                currentUser.AccessToken = "";
                                currentUser.FullName = auth.user.fullName;
                                currentUser.Email = auth.user.email;
                                currentUser.updatedAt = DateTime.Now;
                                currentUser.ClientSecret = newClientSecret;
                                db.Users.Update(currentUser);
                                db.SaveChanges();
                                authRes.ClientSecret = newClientSecret;

                                string test = Base64Encode(auth.ClientID+ ":" + newClientSecret);
                            }
                            else //create new user
                            {
                                _user.AccessToken = "";
                                _user.Email =  auth.user.email;
                                _user.FullName =  auth.user.fullName;
                                _user.Type = (String.IsNullOrEmpty(auth.user.type)) ? "user" : auth.user.type;
                                _user.createdAt = DateTime.Now;
                                _user.ClientID =  Guid.Parse(auth.ClientID);
                                _user.ClientSecret = newClientSecret;
                                db.Users.Add(_user);
                                db.SaveChanges();
                                authRes.ClientSecret = newClientSecret;
                                string test = Base64Encode(auth.ClientID+ ":" + newClientSecret);
                            }
                            return authRes;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        return null;
                    }
                }
            }
            return authRes;
        }
        public UserInfoVM GetUserInfo(string clientID)
        {
            UserInfoVM userInfo = new UserInfoVM();
            if (clientID == null)
            {
                return null;
            }
            else
            {
                User dbUser = new User();
                try
                {
                    Guid cliendIDGuid = new Guid();
                    if (Guid.TryParse(clientID, out cliendIDGuid))
                    {
                        using (var db = new MainContext(_dbContextOptions))
                        {
                            dbUser = db.Users.Where(u => u.ClientID == Guid.Parse(clientID)).FirstOrDefault();
                            if (dbUser.ClientID != null)
                            {
                                userInfo.email = dbUser.Email;
                                userInfo.fullName = dbUser.FullName;
                                userInfo.createdAt = dbUser.createdAt;
                                userInfo.updatedAt = dbUser.updatedAt;
                                //TODO: add active in db
                                userInfo.active = true;
                            }
                        }

                    }

                }
                catch
                {
                    return null;
                }
                return userInfo;
            }
        }

        public bool UpdateUserInfo(string clientID, UserInfoVM user)
        {
            bool isUpdated = false;
            if (!String.IsNullOrEmpty(clientID) && user!=null)
            {
                User dbUser = new User();
                try
                {
                    Guid cliendIDGuid = new Guid();
                    if (Guid.TryParse(clientID, out cliendIDGuid))
                    {
                        using (var db = new MainContext(_dbContextOptions))
                        {
                            dbUser = db.Users.Where(u => u.ClientID == Guid.Parse(clientID)).FirstOrDefault();
                            if (dbUser.ClientID != null)
                            {
                                if(!String.IsNullOrEmpty(user.email))
                                    dbUser.Email = user.email;
                                if (!String.IsNullOrEmpty(user.fullName))
                                    dbUser.FullName = user.fullName;
                                dbUser.updatedAt = DateTime.Now;
                                //TODO: add active
                                db.Users.Update(dbUser);
                                db.SaveChanges();
                                isUpdated = true;
                            }
                        }
                    }
                }
                catch
                {
                    return isUpdated;
                }
            }
            return isUpdated;
        }
        public resTokenVM GetAccessToken(string authorizationCode)
        {
            resTokenVM accessTokenVM = new resTokenVM();
            if (!String.IsNullOrEmpty(authorizationCode))
            {
                //The Authorization header contains your integration key and secret key, concatenated by a colon character, converted into base64,
                string decodedAuthCode = Base64Decode(authorizationCode);
                if (!String.IsNullOrEmpty(decodedAuthCode))
                {
                    string[] authCredentials = decodedAuthCode.Split(':');
                    if (authCredentials != null)
                    {
                        using (var db = new MainContext(_dbContextOptions))
                        {
                            User currentUser = new User();
                            currentUser = db.Users.AsNoTracking().FirstOrDefault(u => u.ClientID == Guid.Parse(authCredentials[0]));
                            if (currentUser.ClientSecret == authCredentials[1])
                            {
                                //create accessToken
                                string newAccessToken = "";
                                var tokenHandler = new JwtSecurityTokenHandler();
                                var key = Encoding.ASCII.GetBytes(APIKey);
                                var tokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Expires = DateTime.UtcNow.AddYears(50),
                                    SigningCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature)
                                };
                                var token = tokenHandler.CreateToken(tokenDescriptor);
                                newAccessToken = tokenHandler.WriteToken(token);

                                currentUser.AccessToken = newAccessToken;
                                db.Users.Update(currentUser);
                                db.SaveChanges();
                                accessTokenVM.accessToken = newAccessToken;
                                accessTokenVM.tokenType = "Bearer";

                            }
                        }
                    }
                }
            }

            return accessTokenVM;

        }
        public bool ValidateAuthCode(string authorizationCode)
        {
            bool isValid = false;
            if (!String.IsNullOrEmpty(authorizationCode))
            {
                //The Authorization header contains your integration key and secret key, concatenated by a colon character, converted into base64,
                string decodedAuthCode = Base64Decode(authorizationCode);
                if (!String.IsNullOrEmpty(decodedAuthCode))
                {
                    string[] authCredentials = decodedAuthCode.Split(':');
                    if (authCredentials != null)
                    {
                        User currentUser = db.Users.Where(u => u.ClientID == Guid.Parse(authCredentials[0])).FirstOrDefault();
                        if (currentUser.ClientSecret == authCredentials[1])
                        {
                            isValid = true;
                        }
                    }

                }
            }
            return isValid;
        }
        private string Base64Encode(string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public bool ValidateAPIKey(string integratorKey)
        {
            if (!String.IsNullOrEmpty(integratorKey))
            {
                if (integratorKey == APIKey)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
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
                        string clientSecret = "";

                        if (currentUser == null || (!String.IsNullOrEmpty(currentUser.ClientSecret) && String.IsNullOrEmpty(currentUser.AccessToken)))
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(APIKey);
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
                            _user.Type = (String.IsNullOrEmpty(user.type)) ? "user" : user.type;
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
                    log.Error(ex.Message);
                    return "An error occurred";
                }
            }
            return null;
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