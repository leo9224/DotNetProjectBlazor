using DotNetProjectLibrary.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;

namespace DotNetProjectBlazor.Pages
{
    partial class AuthenticationPage
    {
        private string? Email;
        private string? Password;
        private bool IsInvalidEmailAndPassword = true;
        private bool IsValidCredentials = true;


        public void OnEmailChange(ChangeEventArgs args)
        {
            Email = args.Value?.ToString();
            CheckValidEmailAndPassword();
        }

        public void OnPasswordChange(ChangeEventArgs args)
        {
            Password = args.Value?.ToString();
            CheckValidEmailAndPassword();
        }

        private void CheckValidEmailAndPassword()
        {
            if (Email is not null && Password is not null)
            {
                if (Authentication.CheckValidEmail(Email) && Authentication.CheckValidPassword(Password))
                {
                    IsInvalidEmailAndPassword = false;
                }
                else
                {
                    IsInvalidEmailAndPassword = true;
                }
            }
            else
            {
                IsInvalidEmailAndPassword = true;
            }
        }

        public async void Authenticate(MouseEventArgs args)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/authentication/{Email}/{Password}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string stringResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(stringResponse);

                string token = $"Bearer {jsonResponse.GetValue("token")}";
                int userId = jsonResponse.GetValue("user_id")!.ToObject<int>();

                await LocalStorage.SetItemAsStringAsync("Token", token);

                NavigationManager.NavigateTo($"/{userId}");
            }
            else
            {
                IsValidCredentials = false;
            }
        }
    }
}
