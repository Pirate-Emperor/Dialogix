namespace Dialogix.Models
{
    public class UserPendingNotification
    {
        public int? LoginUserID { get; set; }
        public string? Message { get; set; }
        public bool? IsSent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? FCMToken { get; set; }
    }
}
