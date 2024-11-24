using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TgBot.Models
{
    public class Author
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        //[JsonIgnore]
        //    public IEnumerable<BookAuthor>? bookAuthors { get; set; }
    }

    public class Book
    {
        //    [Key]
        public Guid id { get; set; }
        //public string id { get; set; }

        public int? unilibId { get; set; }
        //public object unilibId { get; set; }
        public long? book_Price { get; set; }
        //public int book_Price { get; set; }
        public long? book_Copy_Count { get; set; }
        //public int book_Copy_Count { get; set; }
        public long? countLibrary { get; set; }
        //public int countLibrary { get; set; }
        public string? isbN_No { get; set; }
        //public object isbN_No { get; set; }
        public string? udk { get; set; }
        //public object udk { get; set; }
        public string? book_Title { get; set; }
        //public string book_Title { get; set; }
        public string? book_Description { get; set; }
        //public string book_Description { get; set; }
        public string? book_Author { get; set; }
        //public object book_Author { get; set; }
        public string? publisher { get; set; }
        //public string publisher { get; set; }
        public int? publish_Date { get; set; }
        //public int publish_Date { get; set; }
        public int? page_Count { get; set; }
        //public int page_Count { get; set; }
        public Guid? coverImageId { get; set; }
        //public string coverImageId { get; set; }

        [ForeignKey(nameof(coverImageId))]
        public File? coverImage { get; set; }
        //public CoverImage2 coverImage { get; set; }
        public Guid? book_fileId { get; set; }
        //public string book_fileId { get; set; }
        [ForeignKey(nameof(book_fileId))]
        public File? book_File { get; set; }
        //public BookFile2 book_File { get; set; }
        public CategoryEnum? category { get; set; }
        //public int category { get; set; }
        public Guid? contentTypeId { get; set; }
        //public string contentTypeId { get; set; }

        //[JsonIgnore]
        //    public ContentType? contentType { get; set; }

        public Guid? languageId { get; set; }
        //public string languageId { get; set; }
        public Guid? countryId { get; set; }
        //public string countryId { get; set; }
        //[JsonIgnore]
        //    public Country? country { get; set; }
        public Guid? fieldId { get; set; }
        //public string fieldId { get; set; }
        //[JsonIgnore]
        //    public Field? ield { get; set; }
        public Guid? JournalId { get; set; }
        //public object journalId { get; set; }
        //    //[JsonIgnore]
        //    //    public Journal? Journal { get; set; }
        public ICollection<BookTag>? bookTags { get; set; }
        //public List<BookTag2> bookTags { get; set; }
        public ICollection<BookAuthor>? bookAuthors { get; set; }
        //public List<BookAuthor2> bookAuthors { get; set; }
        public IEnumerable<BookCard>? bookCards { get; set; }
        //public List<BookCard2> bookCards { get; set; }
        public string? created_By { get; set; }
        //public string created_By { get; set; }
        public DateTime created_At { get; set; }
        //public DateTime created_At { get; set; }
        public string? lastUpdated_By { get; set; }
        //public string lastUpdated_By { get; set; }
        public DateTime lastUpdated_At { get; set; }
        //public DateTime lastUpdated_At { get; set; }
    }



    public class BookAuthor
    {
        public Guid bookId { get; set; }
        //public string bookId { get; set; }
        public Book? book { get; set; }
        //public Book2 book { get; set; }
        public Guid authorId { get; set; }
        //public string authorId { get; set; }
        public Author? author { get; set; }
        //public Author2 author { get; set; }
    }

    public class BookCard
    {
        [Key]
        //    public Guid id { get; set; }
        public string id { get; set; }
        //    public string? cardId { get; set; }
        public string cardId { get; set; }
        //    public string? cardId { get; set; }
        public string book_Inventory_No { get; set; }
        //public string? book_Inventory_No { get; set; }
        public Guid buildingId { get; set; }
        //public string buildingId { get; set; }
        //[JsonIgnore]
        //    public Building? buildings { get; set; }
        public Guid bookId { get; set; }
        //public string bookId { get; set; }
        public Book? book { get; set; }
        //public object book { get; set; }
        public BookCardStatus? status { get; set; }
        //public int status { get; set; }
        //[JsonIgnore]
        //    //    public IEnumerable<UserCardBookCard>? UserCardBookCards { get; set; }
    }

    public enum BookCardStatus
    {
        InRent = 1,
        Order = 2,
        InLibrary = 3,
        Invalid = 4
    }

    public class BookTag
    {
        public Guid bookId { get; set; }
        public Book? book { get; set; }
        public Guid tagId { get; set; }
        public Tag? tag { get; set; }

        //public string bookId { get; set; }
        //public Book2 book { get; set; }
        //public string tagId { get; set; }
        //public Tag2 tag { get; set; }
    }

    //public class Building
    //{
    //    [Key]
    //    public Guid id { get; set; }
    //    public string? name { get; set; }
    //    public string? location { get; set; }
    //    //[JsonIgnore]
    //    public IEnumerable<BookCard>? bookCards { get; set; }
    //    //[JsonIgnore]
    //    public IEnumerable<User>? users { get; set; }
    //}

    public enum CategoryEnum
    {
        Literature = 1,
        Article = 4,
        Dissertation = 3,
        Monographs = 2
    }


    public class ContentType
    {

        public Guid id { get; set; }
        public string? name { get; set; }
        public bool? literature { get; set; }
        public bool? article { get; set; }
        public bool? dissertation { get; set; }
        public bool? monographs { get; set; }
        //public string id { get; set; }
        //public string name { get; set; }
        //public bool literature { get; set; }
        //public bool article { get; set; }
        //public bool dissertation { get; set; }
        //public bool monographs { get; set; }

        //    //[JsonIgnore]
        //    public ICollection<Book>? books { get; set; }
        //    //[JsonIgnore]
        //    //    public IEnumerable<Journal>? Journals { get; set; }
    }

    public class Country
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        //public string id { get; set; }
        //public string name { get; set; }
        //public string code { get; set; }

        //    //[JsonIgnore]
        //    public IEnumerable<Book>? books { get; set; }
        //    //[JsonIgnore]
        //    //    public IEnumerable<Journal>? Journals { get; set; }
    }


    public class Field
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        //public string id { get; set; }
        //public string name { get; set; }

        //    //[JsonIgnore]
        //    public IEnumerable<Book>? books { get; set; }
        //    //[JsonIgnore]
        //    //    public IEnumerable<Journal>? Journals { get; set; }
    }

    public class File
    {
        public Guid id { get; set; }
        public long size { get; set; }
        public string? mimeType { get; set; }
        public string? extension { get; set; }
        public string? filename { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public string? fileUrl { get; set; }
    }

    public class Language
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        //public string id { get; set; }
        //public string name { get; set; }
        //public string code { get; set; }

        //    //[JsonIgnore]
        //    public IEnumerable<Book>? books { get; set; }
    }

    public class RootBook
    {
        public bool isSuccess { get; set; }
        public Book result { get; set; }
        public int statusCode { get; set; }
        public List<object> errorMessages { get; set; }
    }

    public class Tag
    {
        public Guid id { get; set; }
        public string? name { get; set; }

        public IEnumerable<BookTag>? bookTags { get; set; }
        //public string id { get; set; }
        //public string name { get; set; }
        //public List<object> bookTags { get; set; }
    }
}
