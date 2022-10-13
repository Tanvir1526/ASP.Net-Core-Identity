using IdnetityWebApp.Authorization;
using IdnetityWebApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace IdnetityWebApp.Pages
{
    [Authorize(policy:"HRManagerOnly")]
    public class HRMangerModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        [BindProperty]
        public List<WeatherForecastDTO> WeatherForecastItems { get; set; }
        public HRMangerModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task OnGetAsync()
        {
           
            WeatherForecastItems = await InvokeEndPoint<List<WeatherForecastDTO>>("OurWebApi", "WeatherForecast");
        }
        private async Task<JwtToken>Authenticate()
        {
            // authentication and getting the token
            var httpClient = httpClientFactory.CreateClient("OurWebApi");
            var res = await httpClient.PostAsJsonAsync("auth", new Credential
            {
                UserName = "admin",
                Password = "password"
            });
            res.EnsureSuccessStatusCode();
            string StrJwt = await res.Content.ReadAsStringAsync();
                
            HttpContext.Session.SetString("access_token", StrJwt);

               return  JsonConvert.DeserializeObject<JwtToken>(StrJwt);
        }
        private async Task<T>InvokeEndPoint<T>(string clientName,string url)
        { //get token from session
            JwtToken token = null;
            var strTokenObj = HttpContext.Session.GetString("access_token");

            if (string.IsNullOrEmpty(strTokenObj))
            {
                token = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj);
            }
            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await Authenticate();
            }

            var httpClient = httpClientFactory.CreateClient(clientName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

            return await httpClient.GetFromJsonAsync<T>(url);
        }
    }

}

