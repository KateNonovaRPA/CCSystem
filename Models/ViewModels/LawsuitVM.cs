using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class LawsuitVM
    {
        public int ID { get; set; }
        public string lawsuitNumber { get; set; }
        public int typeId { get; set; }
        public string type { get; set; }
        public int courtID { get; set; }
        public string court { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public int lastChangeNumber { get; set; }
        public List<LawsuitDataVM> lawsuitData { get; set; }

    }
    public class LawsuitDataVM
    {
        public int attributeID { get; set; }
        public string attributeName { get; set; }  
        public string data { get; set; }
        public string createdAt { get; set; }
    }
}
