namespace Dialogix.Models
{
    public class SendNotificationMessage
    {
        public int NotificationID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string MessageData { get; set; }
        public bool IsNotificationSend { get; set; }

    }
}
