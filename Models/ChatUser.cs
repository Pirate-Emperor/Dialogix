using System.ComponentModel.DataAnnotations;

namespace Dialogix.Models
{
    public class ChatUser
    {

        public int? ChatUserID { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public IFormFile? ProfilePictureURL { get; set; }
        public string? HdnProfilePicture { get; set; }
        public IFormFile? ProfilePictureURLCamera { get; set; }
        public bool? Status { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public bool? TermsCondition { get; set; }
        public string? HdnTermsCondition { get; set; }
        public GenderType? Gender { get; set; }
        public string? FCMToken { get; set; }
        public enum GenderType
        {
            Male,
            Female,
            Other
        }

    }
}
