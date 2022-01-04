using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Contracts
{
    public interface ILawsuitService
    {
        LawsuitVM CreateLawsuit(UserLawsuitDataVM _model);

        bool ActivateLawsuit(Guid userID, int lawsuitID);

        bool InactivateAllUserLawsuits(Guid _UUID);

        bool UploadLawsuitData(string court, string lawsuitEntryNumber, string lawsuitNumber, List<KeyValuePair<string, string>> newLawsuitData);
        IQueryable<LawsuitVM> GetActiveLawsuitsListByCourtID(int courtID);
        IQueryable<UserLawsuitDataVM> GetActiveLawsuitsListByRobot(string robotName);
        LawsuitVM GetLawsuitByNumber(string lawsuitNumber);
        LawsuitVM GetLawsuitByEntryNumber(string lawsuitEntryNumber);
        List<LawsuitVM> GetLawsuitsByUser(Guid userID);
        void AddCity(string city, string court, string both);
        void AddLawsuitType(string type);
        string GetChangedLawsuitsListByUserID(Guid userID);
        //void UploadData();
    }
}