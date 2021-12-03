using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AuthResponseVM
    {
        public bool Status { get; set; }
        public string GUIDCode { get; set; }
    }

    public class encAuthResponseVM
    {
        public string encriptedRequest { get; set; }
    }
}
