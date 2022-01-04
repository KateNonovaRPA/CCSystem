using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Contracts;
using Models.Repositories;
using Models.ViewModels;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace CourtsCheckSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        private readonly ILogger<AuthorizeController> logger;
        private readonly IAuthService authService;

        public AuthorizeController(ILogger<AuthorizeController> _logger, IAuthService _authService)
        {
            logger = _logger;
            authService = _authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Microsoft.AspNetCore.Mvc.Route("~/api/authenticate")]
        public IActionResult Authenticate([FromBody] UserVM user)
        {
            string token = authService.AuthorizeUser(user);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }


        public JsonWebToken Get([FromQuery] string grant_type, [FromQuery] string username, [FromQuery] string password, [FromQuery] string refresh_token)
        {
            // Authenticate depending on the grant type.
            UserVM user = grant_type == "refresh_token" ? GetUserByToken(refresh_token) : GetUserByCredentials(username, password);

            if (user == null)
                throw new UnauthorizedAccessException("No!");

            int ageInMinutes = 20;  // However long you want...

            DateTime expiry = DateTime.UtcNow.AddMinutes(ageInMinutes);

            var token = new JsonWebToken
            {
                access_token = authService.AuthorizeUser(user),
                expires_in   = ageInMinutes * 60
            };

            if (grant_type != "refresh_token")
                token.refresh_token = GenerateRefreshToken(user);

            return token;
        }

        private UserVM GetUserByToken(string refreshToken)
        {
            // TODO: Check token against your database.
            if (refreshToken == "test")
                return new UserVM { email = "test" };

            return null;
        }

        private UserVM GetUserByCredentials(string username, string password)
        {
            // TODO: Check username/password against your database.
            if (username == password)
                return new UserVM { email = username };

            return null;
        }

        private string GenerateRefreshToken(UserVM user)
        {
            // TODO: Create and persist a refresh token.
            return "test";
        }

        //// POST: api/Authorize/GetAuthorization
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //[Route("api/Authorize/GetAuthorization")]
        //public ActionResult<string> GetAuthorization([FromBody] encAuthRequestVM _encRequest)
        //{
        //    string decryptedRequest = EncriptionHelper.Decrypt(_encRequest.Request, _encRequest.ReqID.ToString());
        //    string decodedRequest = Base64UrlEncoder.Encoder.Decode(decryptedRequest);

        //    AuthRequestVM authRequest = JsonSerializer.Deserialize<AuthRequestVM>(decryptedRequest);

        //    //TODO: Use service that authorize the request!

        //    AuthResponseVM myRespose = new AuthResponseVM();
        //    myRespose.GUIDCode = authRequest.GUIDCode;
        //    myRespose.Status = authService.CheckAuthorization(authRequest.user.clientID);

        //    //Prepare response:
        //    string responceJson = JsonSerializer.Serialize(myRespose);
        //    string encodedResponse = Base64UrlEncoder.Encoder.Encode(responceJson);
        //    string encryptedResponse = EncriptionHelper.Encrypt(encodedResponse, _encRequest.ReqID.ToString());

        //    return CreatedAtAction("GetAuthorization", new { Timestamp = DateTime.UtcNow.ToString() }, encryptedResponse);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}