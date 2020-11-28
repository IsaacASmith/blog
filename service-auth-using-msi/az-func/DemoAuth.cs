using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System;

namespace AzFunc
{
    public static class DemoAuth
    {
        private const string WEB_API_URL = "https://my_web_api.azurewebsites.net";
        private const string WEB_API_CLIENT_ID = "my_client_id";

        [FunctionName("DemoAuthGET")]
        public static async Task<IActionResult> RunDemo([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            try
            {
                var authServiceTokenProvider = new AzureServiceTokenProvider();
                var authToken = await authServiceTokenProvider.GetAccessTokenAsync(WEB_API_CLIENT_ID);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                var result = await httpClient.GetAsync($"{WEB_API_URL}/api/demo");
                if(!result.IsSuccessStatusCode)
                {
                    throw new Exception ($"API responded with failure status code: {result.StatusCode}");
                }

                var resultContent = await result.Content.ReadAsStringAsync();

                return new OkObjectResult($"Successfully authenticated to the web API! Received the following response: {resultContent}");
            }
            catch(Exception ex)
            {
                return new ObjectResult(new
                {
                    errMessage = ex.Message
                })
                { StatusCode = 500 };
            }
        }
    }
}
