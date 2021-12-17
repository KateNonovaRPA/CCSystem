using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class UserLawsuitDataVM
    {
        [JsonProperty("case_number")]
        public string case_number { get; set; }
        public string type { get; set; }
        public string court { get; set; }
        public string city { get; set; }
    }
    public class UserLawsuitsDataVM
    {
        public UserVM user { get; set; }
        public List<UserLawsuitDataVM> cases { get; set; }
    }
}