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
        private const string WEB_API_RESOURCE_ID = "";

        [FunctionName("DemoAuth")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var authServiceTokenProvider = new AzureServiceTokenProvider();
                var authToken = await authServiceTokenProvider.GetAccessTokenAsync(WEB_API_RESOURCE_ID);

                var authHeader = new AuthenticationHeaderValue("Bearer", authToken);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = authHeader;

                var result = await httpClient.GetAsync("https://myservice.azurewebsites.net/api/demo");
                var resultContent = await result.Content.ReadAsStringAsync();

                return new OkObjectResult(JsonConvert.DeserializeObject(resultContent));
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
