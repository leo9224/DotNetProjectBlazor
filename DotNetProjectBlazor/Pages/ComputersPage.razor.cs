using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace DotNetProjectBlazor.Pages
{
	partial class ComputersPage
	{
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public int ParkId { get; set; }
        [Parameter]
        public int RoomId { get; set; }
        private List<Computer> Computers = new List<Computer>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/computer/get_by_room/{RoomId}");

                IEnumerable<Computer>? computers = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<Computer>>();

                if (computers != null && computers?.Count() != 0)
                {
                    Computers = computers.ToList(); ;
                }

                StateHasChanged();
            }
        }

        public async void DeleteComputer(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"{Config.APIEndpoint}/api/computer/{id}");

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
