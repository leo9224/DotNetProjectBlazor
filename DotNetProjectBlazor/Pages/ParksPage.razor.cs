using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DotNetProjectBlazor.Pages
{
    partial class ParksPage
    {
        [Parameter]
        public int UserId { get; set; }
        private List<Park> Parks = new List<Park>();

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", Token);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/user_park/get_by_user/{UserId}");

            IEnumerable<UserPark>? userParks = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPark>>();

            if (userParks != null && userParks?.Count() != 0)
            {
                foreach (UserPark userPark in userParks!)
                {
                    HttpResponseMessage parkHttpResponseMessage = await httpClient.GetAsync($"{Config.APIEndpoint}/api/park/{userPark.park_id}");
                    Park? park = await parkHttpResponseMessage.Content.ReadFromJsonAsync<Park>();

                    if (park != null)
                    {
                        Parks.Add(park);
                    }
                }
            }
        }
    }
}
