using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts
{
    public interface ICourtService
    {
        bool CreateCourt(CourtVM _model);
        bool UpdateCourt(CourtVM _model);
        CourtVM GetCourtByName(string courtName);
        int? CreateCourtAttribute(int courtId, string attributeName);
        //void AddCity(string city, string court, string both);
    }
}
