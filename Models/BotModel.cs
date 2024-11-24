using System.ComponentModel.DataAnnotations;

namespace TgBot.Models
{
    public class BotUserData
    {
        [Key]
        public int? Id { get; set; }

        public long UserId { get; set; }

        public string? UserTelNum { get; set; }

        public string? UserNameSearch { get; set; }

        public string? UserName { get; set; }

        public string? UserLast { get; set; }

        public string? Api_Access_Token { get; set; }
        public string? Api_HemisId { get; set; }
        public string? Api_FirstName { get; set; }
        public string? Api_LastName { get; set; }
        public string? Api_Email { get; set; }
        public string? Api_Phone { get; set; }
        public string? Api_Group { get; set; }

        //public string UserBio { get; set; }
    }
}
