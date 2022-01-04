using System;
using System.Text.Json.Serialization;

namespace Models.ViewModels
{
    public class UserVM
    {
        public string UUID { get; set; }
        public string clientID { get; set; }
        public string fullName { get; set; }
        public string type { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
        public string AccessToken { get; set; }
    }
    public class JsonWebToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; } = "bearer";
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}