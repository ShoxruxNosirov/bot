using Microsoft.EntityFrameworkCore;
using TgBot.Models;

namespace TgBotVetMedIst_uchun
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<BotUserData> BotUserDB { get; set; }

        public DbSet<DownloadedFileAdressToTg> FileTgAdressDb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            string connectionString = "Server=DESKTOP-DHKVHDC;Database=BotDbb;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True";
            optionsBuilder.UseSqlServer(connectionString);
            //string connectionString = "Server=localhost;Database=YourDatabase;User=YourUser;Password=YourPassword;";
            //optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0))); // Configure MySQL provider

        }
    }
}
