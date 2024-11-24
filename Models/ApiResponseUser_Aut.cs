namespace TgBot.Models
{
    public class RootUserAut
    {
        public bool isSuccess { get; set; }
        public ResultUser result { get; set; }
        public int statusCode { get; set; }
        public List<object> errorMessages { get; set; }
    }

    public class ResultUser
    {
        public User user { get; set; }
        public string access_token { get; set; }
    }
    public class User
    {
        public string id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public object cardId { get; set; }
        public object hemisId { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public object userPhotoId { get; set; }
        public object userPhoto { get; set; }
        public string? buildingId { get; set; }
        public int? role { get; set; }
        public object? faculty { get; set; }
        public object? course { get; set; }
        public object? debts { get; set; }
        public List<object>? posts { get; set; }
        public object? group { get; set; }
        public bool? isActive { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? lastUpdatedAt { get; set; }
    }
}
