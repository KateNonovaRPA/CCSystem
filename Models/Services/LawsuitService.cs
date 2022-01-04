﻿using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Contracts;
using Models.Entities;
using Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Models.Services
{
    public class LawsuitService : BaseService, ILawsuitService
    {
        public LawsuitService(MainContext context)
        {
            db = context;
        }
        public IQueryable<LawsuitVM> GetActiveLawsuitsListByCourtID(int courtID)
        {
            IQueryable<LawsuitVM> lawsuits = db.Lawsuits.Where(x => x.courtID == courtID).GroupJoin(db.UserLawsuits, u => u.ID, ul => ul.lawsuitID, (u, ul) => new { u, ul }).
                Select(lawsuit => new LawsuitVM()
                {
                    lawsuitEntryNumber = lawsuit.u.lawsuitEntryNumber,
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
                    case_entry_number = lawsuit.Lawsuit.lawsuitEntryNumber.ToString(),
                    type= lawsuit.Lawsuit.Type.name,
                    court =  lawsuit.Lawsuit.Court.fullName,
                    city = (lawsuit.Lawsuit.Court.City != null) ? lawsuit.Lawsuit.Court.City.name : "",
                });

            }
            else if (robotName == "Върховен касационен съд")
            {
                lawsuits = db.UserLawsuits.Where(x => x.active == true && x.Lawsuit.Court.name == robotName).Select(lawsuit => new UserLawsuitDataVM()
                {
                    case_entry_number = lawsuit.Lawsuit.lawsuitEntryNumber.ToString(),
                    type= lawsuit.Lawsuit.Type.name,
                    court =  lawsuit.Lawsuit.Court.fullName,
                    city = "",
                });

            }
            else if (robotName == "Софийски районен съд")
            {
                lawsuits = db.UserLawsuits.Where(x => x.active == true && x.Lawsuit.Court.name == robotName).Select(lawsuit => new UserLawsuitDataVM()
                {
                    case_entry_number = lawsuit.Lawsuit.lawsuitEntryNumber.ToString(),
                    type= lawsuit.Lawsuit.Type.name,
                    court =  lawsuit.Lawsuit.Court.fullName,
                    city = (lawsuit.Lawsuit.Court.City != null) ? lawsuit.Lawsuit.Court.City.name : "",
                });
            }
            if (lawsuits != null)
                lawsuits = lawsuits.Distinct();
            //string result = JsonConvert.SerializeObject(lawsuits);
            return lawsuits;
        }

        public LawsuitVM GetLawsuitByNumber(string lawsuitNumber)
        {
            LawsuitVM _lawsuit = new LawsuitVM();
            try
            {
                Lawsuit lawsuit = db.Lawsuits.Where(l => l.lawsuitNumber == lawsuitNumber).FirstOrDefault();
                if (lawsuit != null)
                {
                    _lawsuit.lawsuitNumber = lawsuit.lawsuitNumber;
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
            catch
            {
            }
            return _lawsuit;
        }

        public LawsuitVM GetLawsuitByEntryNumber(string lawsuitEntryNumber)
        {
            LawsuitVM _lawsuit = new LawsuitVM();
            try
            {
                Lawsuit lawsuit = db.Lawsuits.Where(l => l.lawsuitEntryNumber == lawsuitEntryNumber).FirstOrDefault();
                if (lawsuit != null)
                {
                    //_lawsuit.lawsuitNumber = lawsuit.lawsuitNumber;
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
            catch
            {
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

                lawsuit.lawsuitEntryNumber = _model.case_entry_number;
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
            catch
            {
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
                        Type = type
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
                return false;
            }
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
            catch
            {
                return false;
            }
        }

        public string GetChangedLawsuitsListByUserID(Guid userID)
        {
            string jsonresp = "";
            try
            {
                List<int> test = db.UserLawsuits.Where(u => u.active == true).Select(u => u.lawsuitID).ToList();
                List<Lawsuit> userLawsuits = db.Lawsuits.Include(t => t.Court).Include(t => t.Type).Where(t => test.Contains(t.ID)  && t.updatedAt.Date == DateTime.Today.Date).ToList();
                List<ChangedLawsuitData> allChangedData = new List<ChangedLawsuitData>();
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
                jsonresp = JsonConvert.SerializeObject(allChangedData);
            }
            catch (Exception ex)
            {
            }
            return jsonresp;
        }

        private LawsuitVM ParseToLawsuitVM(Lawsuit lawsuit)
        {
            LawsuitVM lawsuitVM = new LawsuitVM();
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
            return lawsuitVM;
        }

        private static Dictionary<string, string> ParseObjectToDictionary(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dictionary;
        }

        private static Dictionary<string, string> ParseJsonToDictionary(string json)
        {
            //string json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dictionary;
        }

        //WARN: for test
        //public void UploadData()
        //{
        ////if (db.Cities.Count() == 0)
        ////{
        ////Upload Courts
        //DataTable excelDt = ExportToDataTable(@"data\Courts.xlsx");
        //try
        //{
        //    foreach (DataRow currentRow in excelDt.Rows)
        //    {
        //        string cellData = currentRow[0].ToString();
        //        string[] subs = SplitString(cellData);
        //        AddCity((subs.Length >1) ? subs[1].Remove(0, 1) : "", subs[0], cellData);

        //    }
        //}
        //catch (Exception ex)
        //{
        //}
        ////}
        ////string lines = "";
        ////List<City> cities = new List<City>();
        ////cities = db.Cities.ToList();
        ////foreach (City city in cities)
        ////{
        ////    lines += "\n" + " new City { ID = "+city.ID+", name = \""+city.name+ "\" };";
        ////}

        //string courtLines = "";
        //List<Court> courts = new List<Court>();
        //courts = db.Courts.ToList();

        //foreach (Court court in courts)
        //{
        //    if (court.cityId != null)
        //    {
        //        courtLines +=  "\n" + "new Court {  ID = "+court.ID+", name = \""+court.name+"\", fullName=\""+court.fullName+"\", cityId = "+court.cityId+", createdAt = DateTime.Now };";
        //    }
        //    else
        //    {
        //        courtLines +=  "\n" + "new Court {  ID = "+court.ID+", name = \""+court.name+"\", fullName=\""+court.fullName+"\", createdAt = DateTime.Now };";
        //    }
        //}

        ////if (db.LawsuitTypes.Count() == 0)
        ////{
        //DataTable excelDtTypes = ExportToDataTable(@"data\Case_Types.xlsx");
        //string line = "";
        //int i = 1;
        //foreach (DataRow currentRow in excelDtTypes.Rows)
        //{
        //    line += "\n" + "new LawsuitTypeDictionary { ID = "+i+" name = \""+currentRow[0].ToString()+"\" },";
        //    i++;
        //    //AddLawsuitType(currentRow[0].ToString());
        //}
        //}

    }
}