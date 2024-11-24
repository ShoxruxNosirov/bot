namespace TgBot.Models
{

    public class Books
    {
        public bool isSuccess { get; set; }
        public ResultBooks result { get; set; }
        public int statusCode { get; set; }
        public List<object> errorMessages { get; set; }
    }

    public class ResultBooks
    {
        public List<Book>? items { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
        public int totalCount { get; set; }
        public int? totalPages { get; set; }
    }

}
