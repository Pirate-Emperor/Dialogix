namespace Dialogix.Models
{
    public class UserConnection
    {
        public int? ConnectionID { get; set; }
        public int? RequestID { get; set; }
        public int? AcceptID { get; set; }
        public int? TimeStamp { get; set; }
        public bool? IsRequestAccepted { get; set; }
    }
}
