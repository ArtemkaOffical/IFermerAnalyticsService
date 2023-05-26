using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace IFermerAnalyticsService
{
    public static class WebRequest
    {
        public static void Get(this IHttpClientFactory client, string url, Action<int,string> action)
        {
            var httpClient = client.CreateClient();
            var response = httpClient.Send(new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)//new Uri("http://192.168.137.86:8080/api/test"),
            });
            action((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        public static void Post(this IHttpClientFactory client, string url,object data ,Action<int, string> action)
        {
            var httpClient = client.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(data),Encoding.UTF8, "application/json");
            var response = httpClient.Send(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),//new Uri("http://192.168.137.86:8080/api/test"),
                Content = jsonContent
            });
            action((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
    }
}
