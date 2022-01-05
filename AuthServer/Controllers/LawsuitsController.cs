using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Models.Contracts;
using Models.ViewModels;
using System;
using System.Collections.Generic;

namespace CourtsCheckSystem.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class LawsuitsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<LawsuitsController> logger;
        private readonly IAuthService authService;
        private readonly ILawsuitService lawsuitService;
        private readonly IUserService userService;

        public LawsuitsController(ILogger<LawsuitsController> _logger, ILawsuitService _lawsuitService, IAuthService _authService, IUserService _userService)
        {
            logger = _logger;
            lawsuitService = _lawsuitService;
            authService = _authService;
            userService = _userService;
        }

        /// <summary>
        /// Receive all lawsuits that the user is interested in
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/userLawsuits")]
        public IActionResult userLawsuits([FromBody] UserLawsuitsDataVM lawsuits)
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
                return Unauthorized("Missing header.");
            if (!authService.CheckAuthorization(accessToken, "user"))
                return Unauthorized();

            UserVM currentUser = userService.GetUserByAccessToken(accessToken);
            //Guid userID = new Guid("71967346-b744-469b-b8d7-159530990028");
            try
            {
                if (lawsuits !=null)
                {
                    //Inactive all user's lawsuits
                    lawsuitService.InactivateAllUserLawsuits(Guid.Parse(currentUser.UUID));
                    foreach (UserLawsuitDataVM currentLawsuit in lawsuits.cases)
                    {
                        //GetLawsuitID
                        LawsuitVM lawsuitVM = lawsuitService.GetLawsuitByEntryNumber(currentLawsuit.case_entry_number);
                        if (lawsuitVM == null || lawsuitVM.ID == 0)
                            lawsuitVM = lawsuitService.CreateLawsuit(currentLawsuit);
                        //Activate lawsuit
                        lawsuitService.ActivateLawsuit(Guid.Parse(currentUser.UUID), lawsuitVM.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("User Lawsuits:", ex);
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Get all changed lawsuits
        /// </summary>
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("~/api/changedLawsuits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult changedLawsuits()
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
                return Unauthorized("Missing header.");
            if (!authService.CheckAuthorization(accessToken, "user"))
                return Unauthorized();

            UserVM currentUser = userService.GetUserByAccessToken(accessToken);
            List<ChangedLawsuitData> result = lawsuitService.GetChangedLawsuitsListByUserID(Guid.Parse(currentUser.UUID));
            return Ok(result);
        }
    }
}