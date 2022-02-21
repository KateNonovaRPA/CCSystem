using Models.ViewModels;

namespace Models.Repositories
{
    public interface IEmailSender
    {
        void SendEmail(MessageRequest message);
    }
}