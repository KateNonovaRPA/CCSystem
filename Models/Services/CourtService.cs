using Models.Context;
using Models.Contracts;
using Models.Entities;
using Models.ViewModels;
using System;
using System.Linq;

namespace Models.Services
{
    public class CourtService : BaseService, ICourtService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CourtService(MainContext context)
        {
            db = context;
        }

        /// <summary>
        /// Add new court
        /// </summary>
        /// <param name="_model"> Court data</param>
        /// <returns>true or false</returns>
        public bool CreateCourt(CourtVM _model)
        {
            Court court = new Court();
            City city = db.Cities.Where(t => t.name == _model.cityName).FirstOrDefault();

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
                log.Error(ex.Message);
                return false;
            }
        }

        public CourtVM GetCourtByName(string courtName)
        {
            CourtVM currentCourtVM = new CourtVM();
            Court court = db.Courts.Where(e => e.name == courtName).FirstOrDefault();
            if (court != null)
            {
                currentCourtVM.ID = court.ID;
                currentCourtVM.name = court.name;
                currentCourtVM.cityID = (court.City != null) ? court.City.ID : 0;
            }
            return currentCourtVM;
        }

        /// <summary>
        /// Update an existing court
        /// </summary>
        /// <param name="_model"> Court data</param>
        /// <returns>true or false</returns>

        public bool UpdateCourt(CourtVM _model)
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

        /// <summary>
        /// Create new court attribute
        /// </summary>
        /// <param name="courtId"></param>
        /// /// <param name="attributeName"></param>
        /// <returns>attribute ID </returns>
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
                return attribute.ID;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}