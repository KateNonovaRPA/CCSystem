using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Models.Contracts;
using Models.ViewModels;
using System;
using System.Net;

namespace CourtsCheckSystem.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class LawsuitsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<LawsuitsController> logger;
        private readonly IAuthService authService;
        private readonly ILawsuitService lawsuitService;

        public LawsuitsController(ILogger<LawsuitsController> _logger, ILawsuitService _lawsuitService)
        {
            logger = _logger;
            lawsuitService = _lawsuitService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/sendUserLawsuits")]
        public HttpStatusCode UserLawsuits([FromBody] UserLawsuitsDataVM lawsuits)
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
            {
                return HttpStatusCode.Forbidden;
            }

            Guid userID = new Guid("71967346-b744-469b-b8d7-159530990028");
            try
            {
                if (lawsuits !=null)
                {
                    //Inactive all user's lawsuits
                    lawsuitService.InactivateAllUserLawsuits(userID);
                    foreach (UserLawsuitDataVM currentLawsuit in lawsuits.cases)
                    {
                        //GetLawsuitID
                        LawsuitVM lawsuitVM = lawsuitService.GetLawsuitByNumber(currentLawsuit.case_number);
                        if (lawsuitVM == null || lawsuitVM.ID == 0)
                            lawsuitVM = lawsuitService.CreateLawsuit(currentLawsuit);
                        //Make the lawsuit active for the current userme
                        lawsuitService.ActivateLawsuit(userID, lawsuitVM.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("User Lawsuits:", ex);
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.OK;
        }

        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("~/api/changedLawsuits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ChangedLawsuits([FromBody] UserVM user)
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Missing header");
            }
            //WARN: to delete userID
            Guid userID = new Guid("71967346-b744-469b-b8d7-159530990028");
            string result = lawsuitService.GetChangedLawsuitsListByUserID(userID);
            return Ok(result);
        }
    }
}