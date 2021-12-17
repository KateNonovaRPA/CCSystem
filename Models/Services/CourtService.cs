using Models.Entities;
using Models.Contracts;
using Models.ViewModels;
using System;
using System.Linq;

namespace Models.Services
{
    public class CourtService : BaseService, ICourtService
    {
        // add new court
        public bool CreateCourt(CourtVM _model)
        {
            Court court = new Court();
            Entities.City city = db.Cities.Where(t => t.name == _model.cityName).FirstOrDefault();
            //CourtType courtType = new CourtType();
            //courtType = db.CourtTypes.Where(c=>c.Type.)
            try
            {
                court.name = _model.name;
                court.cityId = city.ID;
                court.City = city;
                //TODO: Add type

                db.Courts.Add(court);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // update court

        public bool Update(CourtVM _model)
        {
            Court court = new Court();
            try
            {
                court.name = _model.name;
                //TODO: Add type

                db.Courts.Add(court);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // add court attributes
        public int? CreateCourtAttribute(int courtId, string attributeName)
        {
            CourtAttribute attribute = new CourtAttribute();
            try
            {
                attribute.attributeName = attributeName;
                attribute.courtID = courtId;
                attribute.createdAt = DateTime.Now;

                //TODO: Add type

                db.CourtAttributes.Add(attribute);
                db.SaveChanges();
                return attribute.courtID;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}