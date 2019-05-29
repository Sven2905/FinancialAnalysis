using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiWrapper
{
    [Serializable]
    public class WebApiConfiguration
    {
        public static WebApiConfiguration Instance { get; } = new WebApiConfiguration();
        public static string WebApiKey { get; private set; }
        private static readonly HttpClient client = new HttpClient();
        public string Server { get; set; }
        public int Port { get; set; }

        public static void GetKey(string username, string password)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();

            HttpResponseMessage response = client.GetAsync($"http://{Instance.Server}:{Instance.Port}/api/Token/Get?username={username}&password={password}").Result;

            if (response.IsSuccessStatusCode)
            {
                HttpContent responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                WebApiKey = JsonConvert.DeserializeObject<string>(responseString);
            }
        }
    }
}
