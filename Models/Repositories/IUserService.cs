using Models.ViewModels;
using System;
using System.Linq;

namespace Models.Contracts
{
    public interface IUserService
    {
        IQueryable<UserVM> GetUsersList();

        bool AddUser(UserVM _model);

        bool UpdateUser(UserVM _model);

        UserVM GetUserByUUID(Guid _UUID);

        UserVM GetUserByAccessToken(string _accessToken);

        bool DeleteUser(Guid _UUID);

        bool HardDeleteUser(Guid _UUID);

        //// helper methods

        // private string generateJwtToken(UserVM user)
        // {
        //     // generate token that is valid for 7 days
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(new[] { new Claim("id", user..ToString()) }),
        //         Expires = DateTime.UtcNow.AddDays(7),
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //     };
        //     var token = tokenHandler.CreateToken(tokenDescriptor);
        //     return tokenHandler.WriteToken(token);
        // }
    }
}