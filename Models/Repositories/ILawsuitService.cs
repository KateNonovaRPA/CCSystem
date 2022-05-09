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
        List<LawsuitInfoVM> GetAllUserLawsuits(Guid userID);
        LawsuitVM GetLawsuitByNumber(string lawsuitNumber, int courtID, string year, int typeID);
        LawsuitVM GetLawsuitByEntryNumber(string lawsuitEntryNumber, int courtID);
        LawsuitVM GetLawsuitByEntryNumberAndLawsuitNumber(string entryNumber, string lawsuitNumber, int courtID);
        List<LawsuitVM> GetLawsuitsByUser(Guid userID);
        void AddLawsuitType(string type);
        List<ChangedLawsuitData> GetChangedLawsuitsListByUserID(Guid userID);
        List<ChangedLawsuitData> GetChangedLawsuitsListByUserID(Guid userID, DateTime from);
        void UpdateLawsuit(LawsuitVM lawsuitVM);
        int GetLawsuitTypeID(string type);

    }
}