using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;

namespace DotNetProjectBlazor.Pages
{
    partial class ParkFormPage
    {
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public int ParkId { get; set; }
        public Park? Park { get; set; } = new Park() { name = "" };

        protected override async Task OnInitializedAsync()
        {
            if (ParkId != -1)
            {
                HttpClient httpClient = new HttpClient();
                //httpClient.DefaultRequestHeaders.Add("Authorization", Token);
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/park/{ParkId}");

                Park = await httpResponseMessage.Content.ReadFromJsonAsync<Park>();
            }
        }

        public async void SubmitForm(MouseEventArgs args)
        {
            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", Token);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(Park), Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"));


            if (ParkId == -1)
            {
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"{Config.APIEndpoint}/api/park", content);
                Park? park = await httpResponseMessage.Content.ReadFromJsonAsync<Park>();

                HttpContent content2 = new StringContent($"{{\"user_id\":{UserId},\"park_id\":{park.id}}}", Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"));
                HttpResponseMessage httpResponseMessage2 = await httpClient.PostAsync($"{Config.APIEndpoint}/api/user_park", content2);
            }
            else
            {
                HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"{Config.APIEndpoint}/api/park/{ParkId}", content);
            }

            NavigationManager.NavigateTo($"/{UserId}");
        }
    }
}
