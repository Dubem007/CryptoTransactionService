
namespace UserServices.Domain.DTOs
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public bool IsTest { get; set; }
    }


    public class MsgQueue
    {
        public int DelayInMilliseconds { get; set; }
        public bool IsAutoAcknowledged { get; set; }
    }
    public class MessagesExpiryDurationInSeconds
    {
        public int CorpCode { get; set; }
    }

}
