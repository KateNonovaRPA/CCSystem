using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Models.Contracts;
using Models.ViewModels;
using Models.ViewModels.LawsuitsData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace CourtsCheckSystem.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class RobotController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<LawsuitsController> logger;
        private readonly IAuthService authService;
        private readonly ILawsuitService lawsuitService;

        public RobotController(ILogger<LawsuitsController> _logger, ILawsuitService _lawsuitService)
        {
            logger = _logger;
            lawsuitService = _lawsuitService;
        }

        // post data from the robot
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("~/api/robot/SendLawsuitData")]
        public HttpStatusCode SendLawsuitData([FromBody] RobotData robotData)
        {
            //user validation
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
            {
                return HttpStatusCode.Forbidden;
            }
            ListWithDuplicates lawsuitDictiony = new ListWithDuplicates();
            string court = "";
            string lawsuitNumber = "";
            try
            {
                if (robotData.justiceBgLawsuit != null)
                {
                    court = robotData.justiceBgLawsuit.court;
                    lawsuitNumber = robotData.justiceBgLawsuit.case_number;
                    if (robotData.justiceBgLawsuit.case_sessions.Count > 0)
                    {
                        foreach (var item in robotData.justiceBgLawsuit.case_sessions)
                        {
                            Dictionary<string, string> dictionarySections = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item));
                            lawsuitDictiony.Add(dictionarySections);
                        }
                    }
                    if (robotData.justiceBgLawsuit.case_sessions.Count > 0)
                    {
                        foreach (var item in robotData.justiceBgLawsuit.case_acts)
                        {
                            Dictionary<string, string> dictionaryActs = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item));
                            lawsuitDictiony.Add(dictionaryActs);
                        }
                    }
                    robotData.justiceBgLawsuit.case_acts = null;
                    robotData.justiceBgLawsuit.case_sessions = null;

                    Dictionary<string, string> lawsuitInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(robotData.justiceBgLawsuit));
                    lawsuitDictiony.Add(lawsuitInfo);
                }
                else if (robotData.VKSLawsuit !=null)
                {
                    court = "Върховен касационен съд";
                    lawsuitNumber = robotData.VKSLawsuit.case_number;
                    if (robotData.VKSLawsuit.session_details.Count > 0)
                    {
                        foreach (var item in robotData.VKSLawsuit.session_details)
                        {
                            Dictionary<string, string> dictionarySessions = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item));
                            lawsuitDictiony.Add(dictionarySessions);
                        }
                    }

                    if (robotData.VKSLawsuit.act_details.Count > 0)
                    {
                        foreach (var item in robotData.VKSLawsuit.act_details)
                        {
                            Dictionary<string, string> dictionaryActs = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item));
                            lawsuitDictiony.Add(dictionaryActs);
                        }
                    }
                    if (robotData.VKSLawsuit.exit_details != null)
                    {
                        Dictionary<string, string> exitDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(robotData.VKSLawsuit.exit_details));
                        lawsuitDictiony.Add(exitDetails);
                    }
                    robotData.VKSLawsuit.exit_details = null;
                    robotData.VKSLawsuit.session_details = null;
                    robotData.VKSLawsuit.act_details = null;

                    Dictionary<string, string> lawsuitInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(robotData.VKSLawsuit));
                    lawsuitDictiony.Add(lawsuitInfo);
                }
                else if (robotData.SRSLawsuit !=null)
                {
                    court = "Софийски районен съд";
                    lawsuitNumber = robotData.SRSLawsuit.case_number;
                    if (robotData.SRSLawsuit.case_acts.Count > 0)
                    {
                        foreach (var item in robotData.SRSLawsuit.case_acts)
                        {
                            Dictionary<string, string> dictionaryActs = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item));
                            lawsuitDictiony.Add(dictionaryActs);
                        }
                    }

                    if (robotData.SRSLawsuit.case_entry_docs.Count > 0)
                    {
                        foreach (var item in robotData.SRSLawsuit.case_entry_docs)
                        {
                            Dictionary<string, string> dictionaryEntryDocs = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item));
                            lawsuitDictiony.Add(dictionaryEntryDocs);
                        }
                    }

                    robotData.SRSLawsuit.case_acts = null;
                    robotData.SRSLawsuit.case_entry_docs = null;

                    Dictionary<string, string> lawsuitInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(robotData.SRSLawsuit));
                    lawsuitDictiony.Add(lawsuitInfo);
                }
                lawsuitService.UploadLawsuitData(court, lawsuitNumber, lawsuitDictiony);
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.LogError("Post lawsuits data:" + court, ex);
                logger.LogDebug(lawsuitNumber);
                return HttpStatusCode.BadRequest;
            }
        }

        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("~/api/UserLawsuits")]
        public HttpStatusCode UserLawsuits([FromBody] UserLawsuitsDataVM lawsuits)
        {
            //user authorization
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
                        if (lawsuitVM.ID == 0)
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
        [Microsoft.AspNetCore.Mvc.Route("~/api/getLawsuits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult getLawsuits()
        {
            //authorization
            string accessToken = Request.Headers[HeaderNames.Authorization];
            if (String.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Missing header");
            }
            //WARN: to delete userID
            Guid robotID = new Guid("71967346-b744-469b-b8d7-159530990028");
            string robotName = "justiceBG";
            robotName = "Върховен касационен съд";
            string result = lawsuitService.GetActiveLawsuitsListByRobot(robotName);
            //string result = lawsuitService.GetActiveLawsuitsListByRobot(userID);
            return Ok(result);
        }
    }
}