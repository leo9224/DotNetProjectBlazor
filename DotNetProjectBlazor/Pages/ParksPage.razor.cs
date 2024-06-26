﻿using DotNetProjectLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace DotNetProjectBlazor.Pages
{
    partial class ParksPage
    {
        [Parameter]
        public int UserId { get; set; }
        private List<Park> Parks = new List<Park>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
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

                StateHasChanged();
            }
        }

        public async void DeletePark(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", await LocalStorage.GetItemAsStringAsync("Token"));
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"{Config.APIEndpoint}/api/park/{id}");

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
