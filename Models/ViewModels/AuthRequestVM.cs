using System;

namespace Models.ViewModels
{
    public class AuthRequestVM
    {
        public UserVM user { get; set; }
        public string APIKey { get; set; }
        public string ClientID { get; set; }
    }

    public class resAuthRequestVM
    {      
        public string ClientSecret { get; set; }
    }
}