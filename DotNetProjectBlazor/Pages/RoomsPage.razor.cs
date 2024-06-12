using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace DotNetProjectBlazor.Pages
{
	partial class RoomsPage
	{
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public int ParkId { get; set; }
        private List<Room> Rooms = new List<Room>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/room/get_by_park/{ParkId}");

                IEnumerable<Room>? rooms = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<Room>>();

                if (rooms != null && rooms?.Count() != 0)
                {
                    Rooms = rooms.ToList();
                }

                StateHasChanged();
            }
        }

        public async void DeleteRoom(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"{Config.APIEndpoint}/api/room/{id}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo(NavigationManager.Uri, true);
            }
            else
            {

            }
        }
    }
}
