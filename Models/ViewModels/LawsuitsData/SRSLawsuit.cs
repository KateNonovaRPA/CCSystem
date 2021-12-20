using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.ViewModels.LawsuitsData
{
    public class SRSLawsuit
    {
        [JsonProperty("case_entry")]
        public string case_entry { get; set; }
        [JsonProperty("case_date")]
        public string case_date { get; set; }
        [JsonProperty("case_status")]
        public string case_status { get; set; }
        [JsonProperty("case_acts")]
        public List<CaseAct> case_acts { get; set; }
        [JsonProperty("case_type")]
        public string case_type { get; set; }
        [JsonProperty("case_staff")]
        public string case_staff { get; set; }
        [JsonProperty("case_number")]
        public string case_number { get; set; }
        [JsonProperty("case_year")]
        public string case_year { get; set; }
        public List<CaseEntryDoc> case_entry_docs { get; set; }

        public class CaseAct
        {
            [JsonProperty("act_document")]
            public string act_document { get; set; }
            [JsonProperty("act_type")]
            public string act_type { get; set; }
            [JsonProperty("act_number")]
            public string act_number { get; set; }
            [JsonProperty("act_date")]
            public string act_date { get; set; }
        }

        public class CaseEntryDoc
        {
            [JsonProperty("entry_number")]
            public string entry_number { get; set; }
            [JsonProperty("entry_type")]
            public string entry_type { get; set; }
            [JsonProperty("entry_date")]
            public string entry_date { get; set; }
        }

        public class Root
        {
            public UserVM user { get; set; }
            public SRSLawsuit SRSLawsuit { get; set; }
        }
    }
}