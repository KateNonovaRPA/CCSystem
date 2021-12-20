using System.Collections.Generic;

namespace Models.ViewModels.LawsuitsData
{
    public class ExitDetails
    {
        public string exit_number { get; set; }
        public string exit_type { get; set; }
        public string exit_court { get; set; }
        public string exit_date { get; set; }
    }

    public class SessionDetail
    {
        public string session_hall { get; set; }
        public string session_result { get; set; }
        public string session_date { get; set; }
        public string session_type { get; set; }
    }

    public class ActDetail
    {
        public string act_type { get; set; }
        public string act_result { get; set; }
        public string act_number { get; set; }
    }

    public class VKSLawsuit
    {
        public string case_date { get; set; }
        public string case_type { get; set; }
        public string case_number { get; set; }
        public string case_dep { get; set; }
        public string case_collegiality { get; set; }
        public string case_result { get; set; }
        public string case_entry_number { get; set; }
        public string case_year { get; set; }
        public ExitDetails exit_details { get; set; }
        public List<SessionDetail> session_details { get; set; }
        public List<ActDetail> act_details { get; set; }
    }

    public class Root
    {
        public VKSLawsuit VKSLawsuit { get; set; }
    }
}