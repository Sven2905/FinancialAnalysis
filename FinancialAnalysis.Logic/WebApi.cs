using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic
{
    public static class WebApi
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<T> GetDataAsync<T>(string controller, string action = "Get", Dictionary<string, object> parameters = null)
        {
            var url = $"http://localhost:29005/api/{controller}/{action}";

            if (parameters?.Count > 0)
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

        public static T GetData<T>(string controller, string action = "Get", Dictionary<string, object> parameters = null, string webApiKey = "")
        {
            var url = $"http://localhost:29005/api/{controller}/{action}";
            if (parameters?.Count > 0)
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

            return GetData<T>(url, webApiKey);
        }

        private static async Task<T> GetDataAsync<T>(string url)
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();
            if (Globals.ActiveUser != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Globals.ActiveUser.WebApiKey);
            }

            var response = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private static T GetData<T>(string url, string webApiKey)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();
            if (Globals.ActiveUser != null)
            {
                if (string.IsNullOrEmpty(webApiKey))
                {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Globals.ActiveUser.WebApiKey);
                }
                else
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + webApiKey);
                }
            }

            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            return default(T);
        }

        /// <summary>
        /// Schickt eine POST-Nachricht an die angegebene Zieladresse
        /// </summary>
        /// <param name="serverIp">Addresse des Ziels</param>
        /// <param name="port">Port des Ziels</param>
        /// <param name="controllerName">Name des Controllers bzw. der API</param>
        /// <param name="json">Zu übertragenen Daten im json-Format</param>
        /// <returns></returns>
        public static Task<string> PostAsync(string controllerName, object data, string actionName = "")
        {
            string result = string.Empty;

            string json = JsonConvert.SerializeObject(data);

            try
            {
                string url = $"http://localhost:29005/api/{controllerName}";
                if (!string.IsNullOrEmpty(actionName))
                {
                    url += @"/" + actionName;
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + Globals.ActiveUser.WebApiKey);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Task.FromResult(result);
        }

        public static Task<string> PAsync(string controllerName, object data, string actionName = "")
        {
            string result = string.Empty;

            string json = JsonConvert.SerializeObject(data);

            try
            {
                string url = $"http://localhost:29005/api/{controllerName}";
                if (!string.IsNullOrEmpty(actionName))
                {
                    url += @"/" + actionName;
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + Globals.ActiveUser.WebApiKey);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Task.FromResult(result);
        }
    }
}
