using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Models.Context;
using Models.Contracts;
using Models.Entities;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Models.Services
{
    public class LawsuitService : BaseService, ILawsuitService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailConfiguration _emailConfig;

        public LawsuitService(MainContext context, EmailConfiguration emailConfig)
        {
            db = context;
            _emailConfig = emailConfig;
        }

        public IQueryable<LawsuitVM> GetActiveLawsuitsListByCourtID(int courtID)
        {
            IQueryable<LawsuitVM> lawsuits = db.Lawsuits.Where(x => x.courtID == courtID).GroupJoin(db.UserLawsuits, u => u.ID, ul => ul.lawsuitID, (u, ul) => new { u, ul }).
                Select(lawsuit => new LawsuitVM()
                {
                    lawsuitEntryNumber = lawsuit.u.lawsuitEntryNumber,
                    lawsuitNumber = lawsuit.u.lawsuitNumber,
                    typeId = lawsuit.u.typeID,
                    courtID = lawsuit.u.courtID,
                });
            lawsuits = lawsuits.Distinct();

            return lawsuits;
        }

        public IQueryable<UserLawsuitDataVM> GetActiveLawsuitsListByRobot(string robotName)
        {
            IQueryable<UserLawsuitDataVM> lawsuits = null;
            if (robotName == "justiceBG")
            {
                lawsuits = db.UserLawsuits.Where(x => x.active == true).Select(lawsuit => new UserLawsuitDataVM()
                {
                    case_entry_number = (lawsuit.Lawsuit.lawsuitEntryNumber != null) ? lawsuit.Lawsuit.lawsuitEntryNumber.ToString() : "",
                    case_number =  (lawsuit.Lawsuit.lawsuitNumber != null) ? lawsuit.Lawsuit.lawsuitNumber.ToString() : "",
                    type= lawsuit.Lawsuit.Type.name,
                    court =  lawsuit.Lawsuit.Court.fullName,
                    city = (lawsuit.Lawsuit.Court.City != null) ? lawsuit.Lawsuit.Court.City.name : "",
                    year = lawsuit.Lawsuit.year,
                });
            }
            else if (robotName == "Върховен касационен съд")
            {
                lawsuits = db.UserLawsuits.Where(x => x.active == true && x.Lawsuit.Court.name == robotName).Select(lawsuit => new UserLawsuitDataVM()
                {
                    case_entry_number = lawsuit.Lawsuit.lawsuitEntryNumber.ToString(),
                    case_number =  (lawsuit.Lawsuit.lawsuitNumber != null) ? lawsuit.Lawsuit.lawsuitNumber.ToString() : "",
                    type= lawsuit.Lawsuit.Type.name,
                    court =  lawsuit.Lawsuit.Court.fullName,
                    city = "",
                    year = lawsuit.Lawsuit.year,
                });
            }
            else if (robotName == "Софийски районен съд")
            {
                lawsuits = db.UserLawsuits.Where(x => x.active == true && x.Lawsuit.Court.name == robotName).Select(lawsuit => new UserLawsuitDataVM()
                {
                    case_entry_number = (lawsuit.Lawsuit.lawsuitEntryNumber != null) ? lawsuit.Lawsuit.lawsuitEntryNumber.ToString() : "",
                    case_number = (lawsuit.Lawsuit.lawsuitNumber != null) ? lawsuit.Lawsuit.lawsuitNumber.ToString() : "",
                    type= lawsuit.Lawsuit.Type.name,
                    court =  lawsuit.Lawsuit.Court.fullName,
                    city = "София",
                    year = lawsuit.Lawsuit.year,
                });
            }
            if (lawsuits != null)
                lawsuits = lawsuits.Distinct();
            //string result = JsonConvert.SerializeObject(lawsuits);
            return lawsuits;
        }

        public LawsuitVM GetLawsuitByNumber(string lawsuitNumber, int courtID, string year, int typeID)
        {
            LawsuitVM _lawsuit = new LawsuitVM();
            try
            {
                Lawsuit lawsuit = db.Lawsuits.Where(l => l.lawsuitNumber == lawsuitNumber && l.courtID == courtID && l.year == year && l.typeID ==typeID).FirstOrDefault();
                if (lawsuit != null)
                {
                    _lawsuit.lawsuitNumber = lawsuit.lawsuitNumber;
                    _lawsuit.lawsuitEntryNumber = !String.IsNullOrEmpty(lawsuit.lawsuitEntryNumber) ? lawsuit.lawsuitEntryNumber : "";
                    _lawsuit.type = db.LawsuitTypes.Where(e => e.ID==lawsuit.typeID).FirstOrDefault().name;
                    _lawsuit.court = db.Courts.Where(c => c.ID == lawsuit.courtID).FirstOrDefault().name.ToString();
                    _lawsuit.createdAt =lawsuit.createdAt.ToString("dd-MM-yyyy");
                    _lawsuit.ID =lawsuit.ID;

                    List<LawsuitDataVM> lawsuitDataVMs = new List<LawsuitDataVM>();
                    LawsuitData lastLawsuitData = db.LawsuitData.Where(u => u.lawsuitID == lawsuit.ID).OrderByDescending(u => u.createdAt).FirstOrDefault();
                    int lastChangeNumber = (lastLawsuitData != null) ? lastLawsuitData.changeNumber : 0;
                    if (lastLawsuitData != null)
                    {
                        List<LawsuitData> lawsuitDataList = db.LawsuitData.Include(a => a.CourtAttribute).Where(u => u.lawsuitID == lawsuit.ID && u.changeNumber == lastChangeNumber).ToList();
                        foreach (LawsuitData lawsuitData in lawsuitDataList)
                        {
                            LawsuitDataVM lawsuitDataVM = new LawsuitDataVM();
                            lawsuitDataVM.attributeName = lawsuitData.CourtAttribute.attributeName;
                            lawsuitDataVM.data = lawsuitData.data;
                            lawsuitDataVM.createdAt = lawsuitData.createdAt.ToString("dd-MM-yyyy");
                            lawsuitDataVMs.Add(lawsuitDataVM);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("GetLawsuitByNumber func " + ex.Message);
            }
            return _lawsuit;
        }

        public LawsuitVM GetLawsuitByEntryNumber(string lawsuitEntryNumber, int courtID)
        {
            LawsuitVM _lawsuit = new LawsuitVM();
            try
            {
                Lawsuit lawsuit = db.Lawsuits.Where(l => l.lawsuitEntryNumber == lawsuitEntryNumber && l.courtID == courtID).FirstOrDefault();
                if (lawsuit != null)
                {
                    _lawsuit.lawsuitNumber = !String.IsNullOrEmpty(lawsuit.lawsuitNumber) ? lawsuit.lawsuitNumber : "";
                    _lawsuit.lawsuitEntryNumber = lawsuit.lawsuitEntryNumber;
                    _lawsuit.type = db.LawsuitTypes.Where(e => e.ID==lawsuit.typeID).FirstOrDefault().name;
                    _lawsuit.court = db.Courts.Where(c => c.ID == lawsuit.courtID).FirstOrDefault().name.ToString();
                    _lawsuit.createdAt =lawsuit.createdAt.ToString("dd-MM-yyyy");
                    _lawsuit.ID =lawsuit.ID;

                    List<LawsuitDataVM> lawsuitDataVMs = new List<LawsuitDataVM>();
                    LawsuitData lastLawsuitData = db.LawsuitData.Where(u => u.lawsuitID == lawsuit.ID).OrderByDescending(u => u.createdAt).FirstOrDefault();
                    int lastChangeNumber = (lastLawsuitData != null) ? lastLawsuitData.changeNumber : 0;
                    if (lastLawsuitData != null)
                    {
                        List<LawsuitData> lawsuitDataList = db.LawsuitData.Include(a => a.CourtAttribute).Where(u => u.lawsuitID == lawsuit.ID && u.changeNumber == lastChangeNumber).ToList();
                        foreach (LawsuitData lawsuitData in lawsuitDataList)
                        {
                            LawsuitDataVM lawsuitDataVM = new LawsuitDataVM();
                            lawsuitDataVM.attributeName = lawsuitData.CourtAttribute.attributeName;
                            lawsuitDataVM.data = lawsuitData.data;
                            lawsuitDataVM.createdAt = lawsuitData.createdAt.ToString("dd-MM-yyyy");
                            lawsuitDataVMs.Add(lawsuitDataVM);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("GetLawsuitByEntryNumber func " + ex.Message);
            }
            return _lawsuit;
        }

        public LawsuitVM GetLawsuitByEntryNumberAndLawsuitNumber(string entryNumber, string lawsuitNumber, int courtID)
        {
            LawsuitVM _lawsuit = new LawsuitVM();
            try
            {
                Lawsuit lawsuit = db.Lawsuits.Where(l => l.lawsuitEntryNumber == entryNumber && l.lawsuitNumber == lawsuitNumber && l.courtID == courtID).FirstOrDefault();
                if (lawsuit != null)
                {
                    _lawsuit.lawsuitNumber = !String.IsNullOrEmpty(lawsuit.lawsuitNumber) ? lawsuit.lawsuitNumber : "";
                    _lawsuit.lawsuitEntryNumber = lawsuit.lawsuitEntryNumber;
                    _lawsuit.type = db.LawsuitTypes.Where(e => e.ID==lawsuit.typeID).FirstOrDefault().name;
                    _lawsuit.court = db.Courts.Where(c => c.ID == lawsuit.courtID).FirstOrDefault().name.ToString();
                    _lawsuit.createdAt =lawsuit.createdAt.ToString("dd-MM-yyyy");
                    _lawsuit.ID =lawsuit.ID;

                    List<LawsuitDataVM> lawsuitDataVMs = new List<LawsuitDataVM>();
                    LawsuitData lastLawsuitData = db.LawsuitData.Where(u => u.lawsuitID == lawsuit.ID).OrderByDescending(u => u.createdAt).FirstOrDefault();
                    int lastChangeNumber = (lastLawsuitData != null) ? lastLawsuitData.changeNumber : 0;
                    if (lastLawsuitData != null)
                    {
                        List<LawsuitData> lawsuitDataList = db.LawsuitData.Include(a => a.CourtAttribute).Where(u => u.lawsuitID == lawsuit.ID && u.changeNumber == lastChangeNumber).ToList();
                        foreach (LawsuitData lawsuitData in lawsuitDataList)
                        {
                            LawsuitDataVM lawsuitDataVM = new LawsuitDataVM();
                            lawsuitDataVM.attributeName = lawsuitData.CourtAttribute.attributeName;
                            lawsuitDataVM.data = lawsuitData.data;
                            lawsuitDataVM.createdAt = lawsuitData.createdAt.ToString("dd-MM-yyyy");
                            lawsuitDataVMs.Add(lawsuitDataVM);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("GetLawsuitByEntryNumber func " + ex.Message);
            }
            return _lawsuit;
        }
        public List<LawsuitVM> GetLawsuitsByUser(Guid userID)
        {
            List<LawsuitVM> lawsuits = new List<LawsuitVM>();
            //check is there any changes
            return lawsuits;
        }

        public LawsuitVM CreateLawsuit(UserLawsuitDataVM _model)
        {
            Lawsuit lawsuit = new Lawsuit();
            try
            {
                LawsuitType lawsuitType = db.LawsuitTypes.Where(t => t.name== _model.type).FirstOrDefault();
                if (lawsuitType==null)
                {
                    lawsuitType = new LawsuitType();
                    lawsuitType.name = _model.type;
                    db.LawsuitTypes.Add(lawsuitType);
                    db.SaveChanges();
                }
                Court court = db.Courts.Where(x => x.fullName == _model.court).FirstOrDefault();
                if (court==null)
                {
                    City city = db.Cities.Where(x => x.name== _model.city).FirstOrDefault();
                    if (city != null && _model.city != null)
                    {
                        city.name = _model.city;
                        db.Cities.Add(city);
                        db.SaveChanges();
                        court.cityId = city.ID;
                        court.City = city;
                    }
                    court = new Court();
                    court.name = _model.court;
                    court.createdAt = DateTime.Now;
                    db.Courts.Add(court);
                    db.SaveChanges();
                }
                if (_model.case_number != null)
                    lawsuit.lawsuitNumber = _model.case_number;
                if (_model.case_entry_number != null)
                    lawsuit.lawsuitEntryNumber = _model.case_entry_number;
                lawsuit.year = String.IsNullOrEmpty(_model.year) ? DateTime.Now.Year.ToString() : _model.year;
                lawsuit.typeID = lawsuitType.ID;
                lawsuit.courtID = court.ID;
                lawsuit.createdAt = DateTime.Now;
                lawsuit.Court = court;
                lawsuit.Type = lawsuitType;

                db.Lawsuits.Add(lawsuit);
                db.SaveChanges();

                return ParseToLawsuitVM(lawsuit);
            }
            catch (Exception ex)
            {
                log.Error("CreateLawsuit func " + ex.Message);
                return null;
            }
        }

        public bool ActivateLawsuit(Guid userID, int lawsuitID)
        {
            UserLawsuit currentLawsuit = db.UserLawsuits.Where(u => u.lawsuitID == lawsuitID && u.userID == userID).FirstOrDefault();
            try
            {
                if (currentLawsuit != null)
                {
                    //update lawsuit to be active
                    currentLawsuit.active = true;
                    currentLawsuit.updatedAt = DateTime.Now;

                    db.UserLawsuits.Update(currentLawsuit);
                    db.SaveChanges();
                    return true;
                }
                else // add lawsuit
                {
                    currentLawsuit = new UserLawsuit();
                    currentLawsuit.userID = userID;
                    currentLawsuit.lawsuitID = lawsuitID;
                    currentLawsuit.Lawsuit = db.Lawsuits.Where(u => u.ID == lawsuitID).FirstOrDefault();
                    currentLawsuit.createdAt = DateTime.Now;
                    currentLawsuit.active = true;
                    db.UserLawsuits.Add(currentLawsuit);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("ActivateLawsuit func " + ex.Message);
                return false;
            }
        }

        public bool InactivateAllUserLawsuits(Guid userID)
        {
            List<UserLawsuit> lawsuits = new List<UserLawsuit>();
            lawsuits = db.UserLawsuits.Where(u => u.userID == userID).ToList();
            if (lawsuits !=null && lawsuits.Count >0)
            {
                foreach (UserLawsuit lawsuit in lawsuits)
                {
                    lawsuit.active = false;
                }
                db.UserLawsuits.UpdateRange(lawsuits);
                db.SaveChanges();
            }
            return true;
        }

        public bool UploadLawsuitData(string courtName, string lawsuitEntryNumber, string lawsuitNumber, List<KeyValuePair<string, string>> newLawsuitData)
        {
            try
            {
                List<KeyValuePair<string, string>> lastLawsuitDictionary = new List<KeyValuePair<string, string>>();
                Court court = db.Courts.Where(u => u.fullName == courtName).FirstOrDefault();
                Lawsuit lawsuit = db.Lawsuits.Where(u => u.lawsuitEntryNumber == lawsuitEntryNumber && u.courtID == court.ID).FirstOrDefault();

                //WARN: only for test!!!!
                if (lawsuit == null)
                {
                    LawsuitType type = new LawsuitType();
                    string lawsuitType = (newLawsuitData.Where(e => e.Key == "case_type").FirstOrDefault().Value);
                    string year = (newLawsuitData.Where(e => e.Key == "case_year").FirstOrDefault().Value);
                    year = (String.IsNullOrEmpty(year)) ? (newLawsuitData.Where(e => e.Key == "year").FirstOrDefault().Value) : year;
                    type = db.LawsuitTypes.Where(l => l.name ==lawsuitType).FirstOrDefault();
                    if (type == null)
                    {
                        type = new LawsuitType();
                        type.name = lawsuitType;
                        db.LawsuitTypes.Add(type);
                        db.SaveChanges();
                    }
                    lawsuit = new Lawsuit()
                    {
                        lawsuitEntryNumber = lawsuitEntryNumber,
                        lawsuitNumber = (lawsuitNumber!=null) ? lawsuitNumber : "",
                        courtID = court.ID,
                        Court = court,
                        createdAt = DateTime.Now,
                        typeID = type.ID,
                        Type = type,
                        year = year,
                    };
                    db.Lawsuits.Add(lawsuit);
                    db.SaveChanges();
                }

                LawsuitData lastLawsuitData = db.LawsuitData.Where(u => u.lawsuitID == lawsuit.ID).OrderByDescending(u => u.createdAt).FirstOrDefault();
                int lastChangeNumber = (lastLawsuitData != null) ? lastLawsuitData.changeNumber : 0;
                List<LawsuitData> lastLawsuitDataList = db.LawsuitData.Include(u => u.CourtAttribute).Where(u => u.lawsuitID == lawsuit.ID && u.changeNumber == lastChangeNumber).ToList();
                if (lastLawsuitDataList != null && lastLawsuitDataList.Count > 0)
                {
                    foreach (var data in newLawsuitData.ToList())
                    {
                        if (data.Value == "" || data.Value == null)
                            newLawsuitData.Remove(data);
                    }

                    foreach (LawsuitData data in lastLawsuitDataList)
                    {
                        KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>(data.CourtAttribute.attributeName, data.data);
                        lastLawsuitDictionary.Add(keyValue);
                    }
                    lastLawsuitDictionary = lastLawsuitDictionary.OrderBy(x => x.Key).OrderBy(x => x.Value).ToList();
                    newLawsuitData = newLawsuitData.OrderBy(x => x.Key).OrderBy(x => x.Value).ToList();
                    //if there are any changes
                    if ((lastLawsuitDataList.Count != newLawsuitData.Count || !lastLawsuitDictionary.SequenceEqual(newLawsuitData)) && newLawsuitData.Count>0)
                    {
                        CreateLawsuitData(court.ID, lawsuit.ID, newLawsuitData, lastChangeNumber);

                        var rng = new Random();
                        List<string> stakeholders = new List<string>();
                        List<UserLawsuit> usersLawsuits = db.UserLawsuits.Include(a => a.User).Where(u => u.lawsuitID == lawsuit.ID && u.active == true).ToList();
                        foreach (UserLawsuit usersLawsuit in usersLawsuits)
                            stakeholders.Add(usersLawsuit.User.Email);
                        if (stakeholders != null)
                        {
                            string messageContent = "Настъпиха промени в дело ";
                            if (!String.IsNullOrEmpty(lawsuit.lawsuitEntryNumber))
                                messageContent += "с вх. номер " + lawsuit.lawsuitEntryNumber;
                            else if (!String.IsNullOrEmpty(lawsuit.lawsuitNumber))
                                messageContent += "с номер " + lawsuit.lawsuitNumber;
                            messageContent += " в " + lawsuit.Court.fullName;
                            //if (lawsuit.Court.City != null && !String.IsNullOrEmpty(lawsuit.Court.City.name))
                            //    messageContent += ", град" + lawsuit.Court.City.name;


                            MessageRequest message = new MessageRequest(stakeholders.AsEnumerable(), "Настъпили промени в дело", messageContent);
                            SendEmail(message);
                        }
                    }
                    if (String.IsNullOrEmpty(lawsuit.lawsuitNumber) && !String.IsNullOrEmpty(lawsuitNumber))
                    {
                        lawsuit.lawsuitNumber = lawsuitNumber;
                        db.Update(lawsuit);
                        db.SaveChanges();
                    }
                }
                else  // create new LawsuitData, there isn't previous
                {
                    CreateLawsuitData(court.ID, lawsuit.ID, newLawsuitData, lastChangeNumber);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("UploadLawsuitData func " + ex.Message);
                return false;
            }
        }

        public void UpdateLawsuit(LawsuitVM lawsuitVM)
        {
            Lawsuit currentLawsuit = db.Lawsuits.Where(e => e.ID == lawsuitVM.ID).FirstOrDefault();
            if (String.IsNullOrEmpty(currentLawsuit.lawsuitEntryNumber) && !String.IsNullOrEmpty(lawsuitVM.lawsuitEntryNumber))
                currentLawsuit.lawsuitEntryNumber = lawsuitVM.lawsuitEntryNumber;
            if (String.IsNullOrEmpty(currentLawsuit.lawsuitNumber) && !String.IsNullOrEmpty(lawsuitVM.lawsuitNumber))
                currentLawsuit.lawsuitNumber = lawsuitVM.lawsuitNumber;
            db.Update(currentLawsuit);
            db.SaveChanges();
        }
        public bool CreateLawsuitData(int courtId, int lawsuitID, List<KeyValuePair<string, string>> lawsuitData, int lastChangedNumber)
        {
            try
            {
                foreach (KeyValuePair<string, string> data in lawsuitData)
                {
                    LawsuitData newLawsuitData = new LawsuitData();
                    CourtAttribute attribute = db.CourtAttributes.Where(c => c.courtID == courtId && c.attributeName == data.Key).FirstOrDefault();
                    DateTime now = DateTime.Now;
                    Lawsuit currentLawsuit = db.Lawsuits.Where(l => l.ID == lawsuitID).FirstOrDefault();
                    if (attribute != null)
                    {
                        newLawsuitData.courtAttributeID = attribute.ID;
                        newLawsuitData.CourtAttribute = attribute;
                    }
                    else
                    {
                        CourtAttribute newAttribute = new CourtAttribute();
                        newAttribute.attributeName = data.Key;
                        newAttribute.courtID = courtId;
                        newAttribute.Court = db.Courts.Where(c => c.ID == courtId).FirstOrDefault();
                        newAttribute.createdAt = now;

                        db.CourtAttributes.Add(newAttribute);
                        db.SaveChanges();

                        newLawsuitData.courtAttributeID = newAttribute.ID;
                        newLawsuitData.CourtAttribute = newAttribute;
                    }
                    if (data.Value != "" && data.Value != null)
                    {
                        newLawsuitData.data = data.Value;
                        // newLawsuitData.lawsuitID = lawsuitID;
                        newLawsuitData.changeNumber = lastChangedNumber +1;
                        newLawsuitData.createdAt = now;
                        newLawsuitData.lawsuitID = lawsuitID;
                        newLawsuitData.Lawsuit = db.Lawsuits.Where(x => x.ID == lawsuitID).FirstOrDefault();
                        db.LawsuitData.Add(newLawsuitData);
                        db.SaveChanges();
                    }
                    currentLawsuit.updatedAt = now;
                    db.Update(currentLawsuit);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("CreateLawsuitData func " + ex.Message);
                return false;
            }
        }

        public List<ChangedLawsuitData> GetChangedLawsuitsListByUserID(Guid userID)
        {
            //string jsonresp = "";
            List<ChangedLawsuitData> allChangedData = new List<ChangedLawsuitData>();
            try
            {
                List<int> userLawsuitIDs = db.UserLawsuits.Where(u => u.active == true && u.userID == userID).Select(u => u.lawsuitID).ToList();
                List<Lawsuit> userLawsuits = db.Lawsuits.Include(t => t.Court).Include(t => t.Type).Where(t => userLawsuitIDs.Contains(t.ID)  && t.updatedAt.Date == DateTime.Today.Date).ToList();

                foreach (Lawsuit userLawsuit in userLawsuits)
                {
                    ChangedLawsuitData lawsuitData = new ChangedLawsuitData()
                    {
                        lawsuitEntryNumber = userLawsuit.lawsuitEntryNumber,
                        court = userLawsuit.Court.name,
                        city = (String.IsNullOrEmpty(userLawsuit.Court.cityId.ToString())) ? "" : db.Cities.Where(c => c.ID == userLawsuit.Court.cityId).FirstOrDefault().name,
                    };
                    int lawsuitTypeID = userLawsuit.Type.lawsuitTypeDictionaryID.GetValueOrDefault();
                    //WARN: match types
                    if (lawsuitTypeID != 0)
                    {
                        string type = db.LawsuitTypeDictionary.Where(d => d.ID == lawsuitTypeID).FirstOrDefault().name;
                    }
                    allChangedData.Add(lawsuitData);
                }
                //jsonresp = JsonConvert.SerializeObject(allChangedData);
            }
            catch (Exception ex)
            {
                log.Error("GetChangedLawsuitsListByUserID func " + ex.Message);
                return null;
            }
            return allChangedData;
        }

        public List<ChangedLawsuitData> GetChangedLawsuitsListByUserID(Guid userID, DateTime from)
        {
            //string jsonresp = "";
            List<ChangedLawsuitData> allChangedData = new List<ChangedLawsuitData>();
            try
            {
                List<int> userLawsuitIDs = db.UserLawsuits.Where(u => u.active == true && u.userID == userID).Select(u => u.lawsuitID).ToList();
                List<Lawsuit> userLawsuits = db.Lawsuits.Include(t => t.Court).Include(t => t.Type).Where(t => userLawsuitIDs.Contains(t.ID)  && t.updatedAt.Date >= from.Date).ToList();

                foreach (Lawsuit userLawsuit in userLawsuits)
                {
                    ChangedLawsuitData lawsuitData = new ChangedLawsuitData()
                    {
                        lawsuitNumber = userLawsuit.lawsuitNumber,
                        lawsuitEntryNumber = userLawsuit.lawsuitEntryNumber,
                        court = userLawsuit.Court.name,
                        city = (String.IsNullOrEmpty(userLawsuit.Court.cityId.ToString())) ? null : db.Cities.Where(c => c.ID == userLawsuit.Court.cityId).FirstOrDefault().name,
                        type = (userLawsuit.Type != null) ? userLawsuit.Type.name : null,
                    };
                    int lawsuitTypeID = userLawsuit.Type.lawsuitTypeDictionaryID.GetValueOrDefault();
                    //WARN: match types
                    if (lawsuitTypeID != 0)
                    {
                        string type = db.LawsuitTypeDictionary.Where(d => d.ID == lawsuitTypeID).FirstOrDefault().name;
                    }
                    allChangedData.Add(lawsuitData);
                }
                //jsonresp = JsonConvert.SerializeObject(allChangedData);
            }
            catch (Exception ex)
            {
                log.Error("GetChangedLawsuitsListByUserID func " + ex.Message);
                return null;
            }
            return allChangedData;
        }

        public List<LawsuitInfoVM> GetAllUserLawsuits(Guid userID)
        {
            List<LawsuitInfoVM> allChangedData = new List<LawsuitInfoVM>();
            try
            {
                List<int> userLawsuitIDs = db.UserLawsuits.Where(u => u.userID == userID).Select(u => u.lawsuitID).ToList();
                List<Lawsuit> userLawsuits = db.Lawsuits.Include(t => t.Court).Include(t => t.Type).Where(t => userLawsuitIDs.Contains(t.ID)).ToList();

                foreach (Lawsuit userLawsuit in userLawsuits)
                {
                    LawsuitInfoVM lawsuitData = new LawsuitInfoVM();

                    lawsuitData.lawsuitNumber = userLawsuit.lawsuitNumber;
                    lawsuitData.lawsuitEntryNumber = userLawsuit.lawsuitEntryNumber;
                    lawsuitData.type = (userLawsuit.Type != null) ? userLawsuit.Type.name : "";
                    lawsuitData.court = userLawsuit.Court.name;
                    lawsuitData.city = (String.IsNullOrEmpty(userLawsuit.Court.cityId.ToString())) ? null : db.Cities.Where(c => c.ID == userLawsuit.Court.cityId).FirstOrDefault().name;
                    if (userLawsuit.updatedAt != DateTime.MinValue)
                    {
                        lawsuitData.updatedAt =  userLawsuit.updatedAt.ToString("dd-MM-yyyy");
                    }
                    //lawsuitData.updatedAt = (userLawsuit.updatedAt != DateTime.MinValue) ? userLawsuit.updatedAt.ToString("dd-MM-yyyy") : null;
                    lawsuitData.active = db.UserLawsuits.Where(u => u.userID == userID && u.lawsuitID == userLawsuit.ID).FirstOrDefault().active;
                    //WARN: match types
                    //TODO: check the types
                    if (userLawsuit.Type.lawsuitTypeDictionaryID != null)
                    {
                        int lawsuitTypeID = userLawsuit.Type.lawsuitTypeDictionaryID.GetValueOrDefault();
                        string type = db.LawsuitTypeDictionary.Where(d => d.ID == lawsuitTypeID).FirstOrDefault().name;
                    }
                    allChangedData.Add(lawsuitData);
                }
                //jsonresp = JsonConvert.SerializeObject(allChangedData);
            }
            catch (Exception ex)
            {
                log.Error("Get all user lawsuits request " + ex.Message);
                return null;
            }
            return allChangedData;
        }
        private LawsuitVM ParseToLawsuitVM(Lawsuit lawsuit)
        {
            LawsuitVM lawsuitVM = new LawsuitVM();
            try
            {
                if (lawsuit != null)
                {
                    lawsuitVM.lawsuitEntryNumber = lawsuit.lawsuitEntryNumber;
                    lawsuitVM.lawsuitNumber = lawsuit.lawsuitNumber;
                    lawsuitVM.ID = lawsuit.ID;
                    lawsuitVM.courtID = lawsuit.courtID;
                    lawsuitVM.court = lawsuit.Court.name;
                    lawsuitVM.type = lawsuit.Type.name;
                    lawsuitVM.typeId = lawsuit.Type.ID;
                    lawsuitVM.createdAt = lawsuit.createdAt.ToString("dd-MM-yyyy");
                    lawsuitVM.updatedAt =lawsuit.updatedAt.ToString("dd-MM-yyyy");
                    if (lawsuit.LawsuitDatas != null)
                    {
                        lawsuitVM.lawsuitData = new List<LawsuitDataVM>();
                        foreach (LawsuitData item in lawsuit.LawsuitDatas)
                        {
                            LawsuitDataVM currentLawsuitDataVM = new LawsuitDataVM()
                            {
                                attributeID = item.ID,
                                attributeName = item.CourtAttribute.attributeName,
                                data = item.data,
                                createdAt = item.createdAt.ToString("dd-MM-yyyy"),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("ParseToLawsuitVM func " + ex.Message);
            }
            return lawsuitVM;
        }

        public void SendEmail(MessageRequest message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MessageRequest message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h3>{0}</h3>", message.Content) };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public void AddLawsuitType(string type)
        {
            LawsuitTypeDictionary lawsuitType = new LawsuitTypeDictionary();
            try
            {
                lawsuitType = db.LawsuitTypeDictionary.Where(l => l.name == type).FirstOrDefault();
                if (lawsuitType == null)
                {
                    lawsuitType = new LawsuitTypeDictionary();
                    lawsuitType.name = type;
                    db.LawsuitTypeDictionary.Add(lawsuitType);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("AddLawsuitType func " + ex.Message);
            }
        }

        public int GetLawsuitTypeID(string type)
        {

            LawsuitType currentType = db.LawsuitTypes.Where(t => t.name== type).FirstOrDefault();
            if (currentType != null)
                return currentType.ID;
            else
                return 0;
        }
    }
}