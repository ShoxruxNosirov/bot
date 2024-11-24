using System.Net.Http.Headers;
using System.Text.Json;
using TgBot.Models;

namespace TgBot.ApiRequests
{
    public class ApiRequest_BookFile
    {
        public static async Task<RootBook> RequestBook(string fileId, string token)
        {
            string fileUrl = $"https://e-libraryrest.samdu.uz/api/Book/{fileId}";
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(fileUrl);
                if (response.IsSuccessStatusCode)
                {

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    RootBook bookData = JsonSerializer.Deserialize<RootBook>(jsonResponse);
                    return bookData;
                }
            }
            return null;
        }
    }

}
