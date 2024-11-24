using System.Text.Json;
using System.Text;
using TgBot.Models;

namespace TgBot.ApiRequests
{
    public class ApiRequest_User_Aut
    {
        public static async Task<BotUserData> Request(string _login, string _parol, bool is_hemis)
        {
            string apiUrl;
            if (is_hemis)
            {
                apiUrl = "https://e-libraryrest.samdu.uz/api/User/LoginWithHemis";
            }
            else
            {
                apiUrl  = "https://e-libraryrest.samdu.uz/api/User/Login";
            }

            //string bearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkudW5pbGlicmFyeS51elwvYXBpXC9jcm1cL2F1dGhcL3JlZnJlc2giLCJpYXQiOjE2OTIwOTc5ODgsImV4cCI6MTY5NTEyMzA2MSwibmJmIjoxNjkzODI3MDYxLCJqdGkiOiJGNXNqR1FVbHFPS3RSV214Iiwic3ViIjozMjF9.GU0tzhPAjlvN3sMndjWKkfZD6U1CAOgQazWfaHqYrHQ";  // Replace with your actual bearer token

            using (HttpClient clientt = new HttpClient())
            {
                // Set the Authorization header with the bearer token
                //clientt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                LoginForm loginForm = new()
                {
                    Login = _login,
                    Password = _parol
                };

                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string strJson = JsonSerializer.Serialize(loginForm, opt);

                // Make a GET request to the API endpoint
                HttpResponseMessage response = await clientt.PostAsync(apiUrl, new StringContent(strJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    //Console.WriteLine($"{jsonResponse}\n");
                    ////Console.WriteLine(jsonResponse["]);
                    RootUserAut userData = JsonSerializer.Deserialize<RootUserAut>(jsonResponse);

                    BotUserData botModel = new BotUserData()
                    {
                        Api_Access_Token = userData.result.access_token,
                        Api_FirstName = userData.result.user.firstName,
                        Api_LastName = userData.result.user.lastName,
                        Api_Email = userData.result.user.email,
                        Api_Group = userData.result.user.group?.ToString(),
                        Api_HemisId = userData.result.user.hemisId?.ToString(),
                        Api_Phone = userData.result.user.phone
                    };

                    return botModel;
                }
                else
                {
                    return null;
                }

            }

        }

        private class LoginForm
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

    }
}
