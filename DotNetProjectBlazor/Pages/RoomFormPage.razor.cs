using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace DotNetProjectBlazor.Pages
{
    partial class RoomFormPage
    {
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public int ParkId { get; set; }
        [Parameter]
        public int RoomId { get; set; }
        public Room? Room { get; set; } = new Room() { name = "" };

        protected override async Task OnInitializedAsync()
        {
            if (RoomId != -1)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/room/{RoomId}");

                Room = await httpResponseMessage.Content.ReadFromJsonAsync<Room>();
            }
        }

        public async void SubmitForm(MouseEventArgs args)
        {
            Room.park_id = ParkId;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
            HttpContent content = new StringContent(JsonConvert.SerializeObject(Room), Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"));


            if (RoomId == -1)
            {
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"{Config.APIEndpoint}/api/room", content);
                Room? room = await httpResponseMessage.Content.ReadFromJsonAsync<Room>();
            }
            else
            {
                HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"{Config.APIEndpoint}/api/room/{RoomId}", content);
            }

            NavigationManager.NavigateTo($"/{UserId}/{ParkId}");
        }
    }
}
