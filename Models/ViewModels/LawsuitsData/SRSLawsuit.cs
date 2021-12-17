using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.LawsuitsData
{
    public class SRSLawsuit
    {
        public string case_entry { get; set; }
        public string case_date { get; set; }
        public string case_status { get; set; }
        public List<CaseAct> case_acts { get; set; }
        public string case_type { get; set; }
        public string case_staff { get; set; }
        public string case_number { get; set; }
        public string case_year { get; set; }
        public List<CaseEntryDoc> case_entry_docs { get; set; }
        public class CaseAct
        {
            public string act_document { get; set; }
            public string act_type { get; set; }
            public string act_number { get; set; }
            public string act_date { get; set; }
        }

        public class CaseEntryDoc
        {
            public string entry_number { get; set; }
            public string entry_type { get; set; }
            public string entry_date { get; set; }
        }
        public class Root
        {
            public UserVM user { get; set; }
            public SRSLawsuit SRSLawsuit { get; set; }
        }
    }
}
