using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Contracts
{
    public interface ICourtService
    {
        bool CreateCourt(CourtVM _model);
        bool Update(CourtVM _model);
        int? CreateCourtAttribute(int courtId, string attributeName);
    }
}
