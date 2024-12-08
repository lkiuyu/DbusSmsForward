using System.Net;
using System.Text;
using System.Text.Json.Nodes;

namespace DbusSmsForward.Helper
{
    public class HttpHelper
    {
        public static string HttpGet(string Url)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using (HttpClient client = new HttpClient(handler))
            {
                HttpResponseMessage response = client.GetAsync(Url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
        }

        public static string Post(string url, JsonObject obj)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (HttpClient client = new HttpClient(handler))
            {
                var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
        }
    }
}
