using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIDaemonClient
{
    public class DaemonHttpClient : IDaemonHttpClient
    {
        private HttpClient httpClient;
        public HttpClient HttpClient
        {
            get
            {
                return httpClient;
            }
        }

        private readonly ILogger<DaemonHttpClient> _logger = null;

        public DaemonHttpClient(ILogger<DaemonHttpClient> logger)
        {
            _logger = logger;
        }

        public void ConfigureRequestHeaders(string accessToken) //configuring the HttpClient.
        {
            //Socket Exhaustion - For Each HttpClient the connection after the Http request will be held for a period of time after the call.
            //Creating and disposing these HttpClient instances for each request keeps the connections alive which is a waste as no subsequent request will follow.

            httpClient = new HttpClient();

            var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

            if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(x => x.MediaType == "application/json"))
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        }

        public async Task<bool> HttpGetAsync(string URL)
        {
            Console.WriteLine($"\nProcessing HttpGet Request for: {URL} \n");

            HttpResponseMessage response = await httpClient.GetAsync(URL);

            if (response.IsSuccessStatusCode)
            {
                OutputResponseContent(response).Wait();
                return true;
            }

            return false;
        }

        public async Task<bool> HttpPostStringAsync(string URL, string postContent)
        {
            var content = new StringContent(postContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(URL, content);

            if (response.IsSuccessStatusCode)
            {
                OutputResponseContent(response).Wait();
                return true;
            }

            return false;
        }

        private async Task OutputResponseContent(HttpResponseMessage response)
        {
            var jsonResponse = response.Content.ReadAsStringAsync();
            Console.WriteLine(await jsonResponse);
        }
    }
}
