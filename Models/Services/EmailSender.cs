using MailKit.Net.Smtp;
using MimeKit;
using Models.ViewModels;

namespace Models.Services
{
    public class EmailSender : BaseService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

       
    }
}