using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.ViewModels.LawsuitsData
{
    public class JusticeBgLawsuit
    {
        [JsonProperty("year")]
        public string year { get; set; }

        [JsonProperty("case_type")]
        public string case_type { get; set; }

        public string case_number { get; set; }
        public string case_code { get; set; }
        public List<CaseAct> case_acts { get; set; }

        [JsonProperty("case_entry_number")]
        public string case_entry_number { get; set; }

        [JsonProperty("case_collegiality")]
        public string case_collegiality { get; set; }

        public List<CaseSession> case_sessions { get; set; }

        [JsonProperty("court")]
        public string court { get; set; }

        [JsonProperty("case_date")]
        public string case_date { get; set; }
        public string subject_matter { get; set; }
    }

    public class CaseAct
    {
        public string act_judge { get; set; }
        public string act_type { get; set; }
        public string act_date { get; set; }
        public string act_number { get; set; }
    }

    public class CaseSession
    {
        public string session_prosecutor { get; set; }
        public string session_date { get; set; }
        public string session_type { get; set; }
        public string session_secretary { get; set; }
    }

    public class VKSRoot
    {
        public string user { get; set; }
        public JusticeBgLawsuit justiceBGLawsuit { get; set; }
    }
}