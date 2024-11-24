using System.Text.Json;
using TgBot.Models;

namespace TgBot.ApiRequests
{
    public class ApiRequest_BookSearch
    {
        public static async Task<Books> RequestBooks(string book_Title, int pageNumber, int pageSize)
        {
            string apiUrl = $"https://e-libraryrest.samdu.uz/api/Book/Filter?Book_Title={book_Title}&pageNumber={pageNumber}&pageSize={pageSize}";  // Replace with your API endpoint
            //string bearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkudW5pbGlicmFyeS51elwvYXBpXC9jcm1cL2F1dGhcL3JlZnJlc2giLCJpYXQiOjE2OTIwOTc5ODgsImV4cCI6MTY5NTEyMzA2MSwibmJmIjoxNjkzODI3MDYxLCJqdGkiOiJGNXNqR1FVbHFPS3RSV214Iiwic3ViIjozMjF9.GU0tzhPAjlvN3sMndjWKkfZD6U1CAOgQazWfaHqYrHQ";  // Replace with your actual bearer token

            using (HttpClient client = new HttpClient())
            {
                // Set the Authorization header with the bearer token
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make a GET request to the API endpoint
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    //Console.WriteLine($"{jsonResponse}\n");
                    ////Console.WriteLine(jsonResponse["]);
                    Books userData = JsonSerializer.Deserialize<Books>(jsonResponse);
                    return userData;
                }
            }
            return null;
        }
    }
}
