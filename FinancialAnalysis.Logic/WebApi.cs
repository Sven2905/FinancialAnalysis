using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic
{
    public static class WebApi
    {
        public static async Task<T> GetDataAsync<T>(string controller, string action, Dictionary<string, object> parameters)
        {
            var url = $"http://localhost:29005/api/{controller}/{action}";

            if (parameters.Count > 0)
            {
                url += "?";

                foreach (var item in parameters)
                {
                    if (item.Value is DateTime)
                    {
                        url += item.Key + "=" + ((DateTime)(item.Value)).ToString("yyyy-MM-ddTHH:mm:ss") + "&";
                    }
                    else
                    {
                        url += item.Key + "=" + item.Value + "&";
                    }
                }
            }

            if (url[url.Length - 1] == '&')
            {
                url = url.Remove(url.Length - 1, 1);
            }

            return await GetDataAsync<T>(url);
        }

        public static T GetData<T>(string controller, string action, Dictionary<string, object> parameters)
        {
            var url = $"http://localhost:29005/api/{controller}/{action}";
            if (parameters.Count > 0)
            {
                url += "?";

                foreach (var item in parameters)
                {
                    if (item.Value is DateTime)
                    {
                        url += item.Key + "=" + ((DateTime)(item.Value)).ToString("yyyy-MM-ddTHH:mm:ss") + "&";
                    }
                    else
                    {
                        url += item.Key + "=" + item.Value + "&";
                    }
                }
            }

            if (url[url.Length - 1] == '&')
            {
                url = url.Remove(url.Length - 1, 1);
            }

            return GetData<T>(url);
        }

        private static async Task<T> GetDataAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Globals.ActiveUser.WebApiKey);

                var response = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }

        private static T GetData<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Globals.ActiveUser.WebApiKey);
                var response = httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
                return default(T);
            }
        }
    }
}
