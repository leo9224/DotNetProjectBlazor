using DotNetProjectLibrary.Models;
using System.Net.Http.Json;
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

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", Token);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/room/get_by_park/{ParkId}");

            IEnumerable<Room>? rooms = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<Room>>();

            if (rooms != null && rooms?.Count() != 0)
            {
                Rooms = rooms.ToList();
            }
        }
    }
}
