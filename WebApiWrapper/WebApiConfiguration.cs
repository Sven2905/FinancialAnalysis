using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper
{
    public static class WebApiConfiguration
    {
        public static string WebApiKey { get; private set; }
        private static readonly HttpClient client = new HttpClient();
        public static string Server { get; set; } = "sven.tech";
        public static int Port { get; set; } = 29005;

        public static void GetKey(string username, string password)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();

            var response = client.GetAsync($"http://{Server}:{Port}/api/Token/Get?username={username}&password={password}").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                WebApiKey = JsonConvert.DeserializeObject<string>(responseString);
            }
        }
    }
}
