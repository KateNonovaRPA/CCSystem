using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Models.Contracts;
using Models.ViewModels;
using System.Diagnostics;

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
            {
                return Unauthorized();
            }
            else if (token == "An error occurred")
            {
                return BadRequest();
            }
            else
            {
                return Ok(token);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Microsoft.AspNetCore.Mvc.Route("~/api/auth")]
        public IActionResult Auth([FromBody] AuthRequestVM auth)
        {
            resAuthRequestVM clientSecret = new resAuthRequestVM();
            if (auth.ClientID == null || auth.APIKey == null || auth.user == null)
            {
                return BadRequest(clientSecret);
            }
            else
            {
                if (authService.ValidateAPIKey(auth.APIKey))
                {
                    resAuthRequestVM newAuthCode = authService.AuthCode(auth);
                    if (newAuthCode != null)
                        return Ok(newAuthCode);
                    else
                        return Unauthorized(new APIErrorVM() { error ="Unauthorized Access" });
                }
                else
                {
                    return Unauthorized(new APIErrorVM() { error ="Wrong API key" });
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Microsoft.AspNetCore.Mvc.Route("~/api/auth/token")]
        public IActionResult AuthToken()
        {
            //WARN: Test
            //string testAuthCode = Base64Encode("49333AC5-03C9-4517-927F-197B729F3E32" + ":" + "579dd594-fc30-4c29-a4a1-e88b51e17a26");

            string authorizationCode = Request.Headers[HeaderNames.Authorization];
            if (!authorizationCode.Contains("Bearer"))
            {
                return BadRequest(new APIErrorVM() { error ="Wrong authorization type" });
            }
            else
            {
                resTokenVM accessToken = authService.GetAccessToken(authorizationCode.Replace("Bearer ", ""));
                if (accessToken != null)
                {
                    return Ok(accessToken);
                }
                else
                    return Unauthorized(new APIErrorVM() { error ="Unauthorized Access" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //WARN: for test
        //public static string Base64Encode(string plainText)
        //{
        //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //    return System.Convert.ToBase64String(plainTextBytes);
        //}
    }
}