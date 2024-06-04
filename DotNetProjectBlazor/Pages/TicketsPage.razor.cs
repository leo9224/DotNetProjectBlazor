using System.Net.Http.Json;
using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace DotNetProjectBlazor.Pages
{
    partial class TicketsPage
    {
        [Parameter]
        public int UserId { get; set; }
        private List<Ticket> Tickets = new List<Ticket>();

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/ticket");

            IEnumerable<Ticket>? tickets = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<Ticket>>();

            if (tickets != null && tickets?.Count() != 0)
            {
                Tickets = tickets.ToList();
            }
        }

        public async void DeleteTicket(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"{Config.APIEndpoint}/api/ticket/{id}");

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
