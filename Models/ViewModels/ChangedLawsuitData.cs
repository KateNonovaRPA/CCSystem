using System;

namespace Models.ViewModels
{
    public class ChangedLawsuitData
    {
        public string lawsuitNumber { get; set; }
        public string lawsuitEntryNumber { get; set; }
        public string type { get; set; }
        public string court { get; set; }
        public string city { get; set; }
    }
    public class UserLawsuitInfo
    {
        public string lawsuitNumber { get; set; }
        public string lawsuitEntryNumber { get; set; }
        public string type { get; set; }
        public string court { get; set; }
        public string city { get; set; }
        public bool active { get; set; }
    }
    public class PeriodChangedLawsuits
    {
        public string dateFrom { get; set; }
    }
}