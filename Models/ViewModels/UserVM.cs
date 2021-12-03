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
        public string administrationOId { get; set; }
        public string employeeNames { get; set; }
        public string employeePosition { get; set; }
        public string employeeIdentifier { get; set; }
        public string lawReason { get; set; }
        public string remark { get; set; }
        public string serviceType { get; set; }
        public string serviceURI { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateUpdated { get; set; }
    }
}
