using AlfalahApp.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlfalahApp.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            User currentUser = await _httpClient.GetFromJsonAsync<User>("api/users/getcurrentuser");

            if (currentUser != null && currentUser.Username != null)
            {
                //create a claim
                var claim = new Claim(ClaimTypes.Name, currentUser.Username);
                //create claimidentity
                var claimIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
                //create claimPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
