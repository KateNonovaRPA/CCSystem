using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class UserVM
    {
        public string UUID { get; set; }
        public string identityID { get; set; }
        public string processorID { get; set; }
        public string administrationName { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
    }
}
