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

        bool UploadLawsuitData(string court, string lawsuit, List<KeyValuePair<string, string>> newLawsuitData);
        IQueryable<LawsuitVM> GetActiveLawsuitsListByCourtID(int courtID);
        LawsuitVM GetLawsuitByNumber(string lawsuitNumber);

        List<LawsuitVM> GetLawsuitsByUser(Guid userID);
        void AddCity(string city, string court, string both);
        void AddLawsuitType(string type);
        string GetChangedLawsuitsListByUserID(Guid userID);
        void UploadData();
    }
}