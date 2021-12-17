using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Contracts;
using Models.ViewModels;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace AuthServer.Controllers
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

        // POST: api/Authorize/GetAuthorization
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [Route("api/Authorize/GetAuthorization")]
        public ActionResult<string> GetAuthorization([FromBody] encAuthRequestVM _encRequest)
        {
            string decryptedRequest = EncriptionHelper.Decrypt(_encRequest.Request, _encRequest.ReqID.ToString());
            string decodedRequest = Base64UrlEncoder.Encoder.Decode(decryptedRequest);

            AuthRequestVM authRequest = JsonSerializer.Deserialize<AuthRequestVM>(decodedRequest);

            //TODO: Use service that authorize the request!

            AuthResponseVM myRespose = new AuthResponseVM();
            myRespose.GUIDCode = authRequest.GUIDCode;
            myRespose.Status = authService.CheckAuthorization(authRequest.user.identityID);

            //Prepare response:
            string responceJson = JsonSerializer.Serialize(myRespose);
            string encodedResponse = Base64UrlEncoder.Encoder.Encode(responceJson);
            string encryptedResponse = EncriptionHelper.Encrypt(encodedResponse, _encRequest.ReqID.ToString());

            return CreatedAtAction("GetAuthorization", new { Timestamp = DateTime.UtcNow.ToString() }, encryptedResponse);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}