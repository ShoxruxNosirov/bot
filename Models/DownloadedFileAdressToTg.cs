using System.ComponentModel.DataAnnotations;

namespace TgBot.Models
{
    public class DownloadedFileAdressToTg
    {
        [Key] 
        public int Id { get; set; }
        public Guid fileId { get; set; }
        public string fileTgAdressId { get; set; }
    }
}
