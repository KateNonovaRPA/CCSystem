using System;

namespace Models.ViewModels
{
    public class AuthRequestVM
    {
        public UserVM user { get; set; }
        public string GUIDCode { get; set; }
    }

    public class encAuthRequestVM
    {
        public Guid ReqID { get; set; }
        public string Request { get; set; }
    }
}