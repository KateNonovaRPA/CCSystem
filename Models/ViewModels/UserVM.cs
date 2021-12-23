using System;
using System.Text.Json.Serialization;

namespace Models.ViewModels
{
    public class UserVM
    {
        public string UUID { get; set; }
        public string identityID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
    }
}