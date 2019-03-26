using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApiWrapper
{
    public static class WebApi
    {
        private static readonly HttpClient client = new HttpClient();
        public static string WebApiKey { get; set; }

        public static string GetKey(string username, string password)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();

            var response = client.GetAsync($"http://localhost:29005/api/Token/Get?username={username}&password={password}").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<string>(responseString);
            }
            return "";
        }

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

        public static T GetData<T>(string controller, string action = "Get", Dictionary<string, object> parameters = null)
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

            return GetData<T>(url);
        }

        public static T GetDataById<T>(string controller, int id, string action = "GetById")
        {
            var url = $"http://localhost:29005/api/{controller}/{action}?id={id}";
            

            if (url[url.Length - 1] == '&')
            {
                url = url.Remove(url.Length - 1, 1);
            }

            return GetData<T>(url);
        }

        private static async Task<T> GetDataAsync<T>(string url)
        {
            //client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + WebApiKey);

            var response = await client.GetStringAsync(url);
            if (response == "[]")
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(response);
        }

        private static T GetData<T>(string url)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + WebApiKey);

            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                if (responseString == "[]")
                {
                    return default(T);
                }

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
        public static Task<int> PostAsync(string controllerName, object data, string actionName = "")
        {
            int result = 0;

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
                httpWebRequest.Headers.Add("Authorization", "Bearer " + WebApiKey);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string resultString = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(resultString) && resultString != "[]")
                    {
                        result = Convert.ToInt32(streamReader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Task.FromResult(result);
        }

        public static async Task<bool> PutAsync(string controllerName, object data, string actionName = "")
        {
            string resultString = string.Empty;
            HttpClient client = new HttpClient();
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
                httpWebRequest.Headers.Add("Authorization", "Bearer " + WebApiKey);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    resultString = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return await Task.FromResult(Convert.ToBoolean(resultString));
        }

        public static Task<bool> DeleteAsync(string controllerName, object data, string actionName = "")
        {
            string result = string.Empty;

            string json = JsonConvert.SerializeObject(data);

            try
            {
                string url = $"http://localhost:29005/api/{controllerName}/{data}";
                if (!string.IsNullOrEmpty(actionName))
                {
                    url += @"/" + actionName;
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + WebApiKey);

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
            return Task.FromResult(Convert.ToBoolean(result));
        }
    }
}
