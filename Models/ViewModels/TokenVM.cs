using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.ViewModels
{
    public class resTokenVM
    {
        public string accessToken { get; set; }
        public string tokenType { get; set; }
    }
}
