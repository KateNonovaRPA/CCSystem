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
        private readonly ICourtService courtService;

        public LawsuitsController(ILogger<LawsuitsController> _logger, ILawsuitService _lawsuitService, IAuthService _authService, IUserService _userService, ICourtService _courtService)
        {
            logger = _logger;
            lawsuitService = _lawsuitService;
            authService = _authService;
            userService = _userService;
            courtService = _courtService;
        }

        /// <summary>
        /// Save all lawsuits that the user is interested in
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/user/lawsuits")]
        public IActionResult userLawsuits([FromBody] UserLawsuitsDataVM lawsuits)
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
                return Unauthorized(new APIErrorVM() { error ="Missing header" });
            if (!accessToken.Contains("Bearer "))
                return BadRequest(new APIErrorVM() { error ="Wrong authorization type" });
            accessToken = accessToken.Replace("Bearer ", "");
            if (!authService.CheckAuthorization(accessToken, "user"))
                return Unauthorized(new APIErrorVM() { error ="Unauthorized Access" });

            UserVM currentUser = userService.GetUserByAccessToken(accessToken);
            try                                                
            {
                if (lawsuits !=null)
                {
                    //Inactive all user's lawsuits
                    lawsuitService.InactivateAllUserLawsuits(Guid.Parse(currentUser.UUID));
                    foreach (UserLawsuitDataVM currentLawsuit in lawsuits.cases)
                    {
                        LawsuitVM lawsuitVM = new LawsuitVM();
                        CourtVM courtVM = new CourtVM();
                        courtVM = courtService.GetCourtByName(currentLawsuit.court);
                        int lawsuitTypeID = 0;
                        if (!String.IsNullOrEmpty(currentLawsuit.type))
                            lawsuitTypeID = lawsuitService.GetLawsuitTypeID(currentLawsuit.type);
                        //GetLawsuitID
                        if (!String.IsNullOrEmpty(currentLawsuit.case_entry_number) && !String.IsNullOrEmpty(currentLawsuit.case_number))
                        {
                            lawsuitVM = lawsuitService.GetLawsuitByEntryNumberAndLawsuitNumber(currentLawsuit.case_entry_number, currentLawsuit.case_number, courtVM.ID);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(currentLawsuit.case_entry_number))
                                lawsuitVM = lawsuitService.GetLawsuitByEntryNumber(currentLawsuit.case_entry_number, courtVM.ID);
                            if (!String.IsNullOrEmpty(currentLawsuit.case_number))
                                lawsuitVM = lawsuitService.GetLawsuitByNumber(currentLawsuit.case_number, courtVM.ID, currentLawsuit.year, lawsuitTypeID);
                        }
                        if (lawsuitVM == null || lawsuitVM.ID == 0)
                            lawsuitVM = lawsuitService.CreateLawsuit(currentLawsuit);
                        else
                        {
                            if ((String.IsNullOrEmpty(lawsuitVM.lawsuitEntryNumber) && !String.IsNullOrEmpty(currentLawsuit.case_entry_number)) ||
                                (String.IsNullOrEmpty(lawsuitVM.lawsuitNumber) && !String.IsNullOrEmpty(currentLawsuit.case_number)))
                                lawsuitService.UpdateLawsuit(lawsuitVM);
                        }
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
            return Ok("Successful request");
        }

        /// <summary>
        /// Get all changed lawsuits
        /// </summary>
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("~/api/user/changedLawsuits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult changedLawsuits([FromBody] PeriodChangedLawsuits period)
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
                return Unauthorized(new APIErrorVM() { error ="Missing header" });
            if (!accessToken.Contains("Bearer "))
                return BadRequest(new APIErrorVM() { error ="Wrong authorization type" });
            accessToken = accessToken.Replace("Bearer ", "");
            if (!authService.CheckAuthorization(accessToken, "user"))
                return Unauthorized(new APIErrorVM() { error ="Unauthorized Access" });

            UserVM currentUser = userService.GetUserByAccessToken(accessToken);
            List<ChangedLawsuitData> result = new List<ChangedLawsuitData>();
            if (period != null)
            {
                if(period.dateFrom != null)
                {
                    DateTime startDate = DateTime.Now;
                    if(DateTime.TryParse(period.dateFrom, out startDate))
                    {
                        result = lawsuitService.GetChangedLawsuitsListByUserID(Guid.Parse(currentUser.UUID), startDate);
                    }
                }               
            }
            else
            {
                //TODO: check datatime
                result = lawsuitService.GetChangedLawsuitsListByUserID(Guid.Parse(currentUser.UUID));
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("~/api/user/lawsuits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult userLawsuits()
        {
            //user authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
                return Unauthorized(new APIErrorVM() { error ="Missing header" });
            if (!accessToken.Contains("Bearer "))
                return BadRequest(new APIErrorVM() { error ="Wrong authorization type" });
            accessToken = accessToken.Replace("Bearer ", "");
            if (!authService.CheckAuthorization(accessToken, "user"))
                return Unauthorized(new APIErrorVM() { error ="Unauthorized Access" });

            UserVM currentUser = userService.GetUserByAccessToken(accessToken);

            List<LawsuitInfoVM> result = lawsuitService.GetAllUserLawsuits(Guid.Parse(currentUser.UUID));
            return Ok(result);
        }



    }

}