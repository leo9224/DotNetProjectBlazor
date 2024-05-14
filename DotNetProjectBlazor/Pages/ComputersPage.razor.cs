using System.Net.Http.Json;
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

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", Token);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/computer/get_by_room/{RoomId}");

            IEnumerable<Computer>? computers = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<Computer>>();

            /*if (computers is not null)
            {
                foreach (Computer computer in computers)
                {
                    int? typeId = computer.type_id;

                    if (typeId is not null)
                    {
                        HttpResponseMessage httpResponseMessageForType = await httpClient.GetAsync($"https://localhost:7014/api/type/{typeId}");
                        Type? type = await httpResponseMessageForType.Content.ReadFromJsonAsync<Type>();

                        if (type is not null)
                        {
                            computer.os = type.description;
                        }
                    }
                    else
                    {
                        computer.os = "Unknown";
                    }
                }
            }*/

            if (computers != null && computers?.Count() != 0)
            {
                Computers = computers.ToList(); ;
            }
        }

        public async void DeleteComputer(int id)
        {
            HttpClient httpClient = new HttpClient();
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
