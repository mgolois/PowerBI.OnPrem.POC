using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PowerBi.OnPrem.Core
{
    public enum ReponseContent
    {
        Json,
        Bytes
    }
    public static class HttpHelper
    {
        private static HttpClient client;
        private const string contentTypeJson = "application/json";
        static HttpHelper()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                UseDefaultCredentials = true
            };
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentTypeJson));

        }

        public static async Task<T> UploadFile<T>(string url, byte[] bytes, string name, string fileName)
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(bytes), name, fileName);

            var responseMessage = await client.PostAsync(url, content);
            var responseString = await responseMessage.Content.ReadAsStringAsync();

            return await GetJsonContent<T>(responseMessage, url);
        }

        public static async Task<U> Post<T, U>(string url, T item)
        {
            var itemJson = JsonConvert.SerializeObject(item);
            var content = new StringContent(itemJson, Encoding.UTF8, contentTypeJson);

            var responseMessage = await client.PostAsync(url, content);

            return await GetJsonContent<U>(responseMessage, url);
        }
        public static async Task<T> Post<T>(string url, T item)
        {
            return await Post<T, T>(url, item);
        }

        public static async Task<T> Get<T>(string url, ReponseContent type = ReponseContent.Json) where T : class
        {
            var responseMessage = await client.GetAsync(url);

            switch (type)
            {
                case ReponseContent.Bytes:
                    return await GetBytesContent<T>(responseMessage, url);
                case ReponseContent.Json:
                default:
                    return await GetJsonContent<T>(responseMessage, url);

            }
        }

        private static async Task<T> GetJsonContent<T>(HttpResponseMessage responseMessage, string url)
        {
            var responseString = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                JObject o = JObject.Parse(responseString);
                return JsonConvert.DeserializeObject<T>(o.SelectToken("$.value").ToString());
            }

            throw new Exception($"Posting to '{url}'  failed. {responseMessage.ReasonPhrase ?? ""}", new Exception(responseString));

        }
        private static async Task<T> GetBytesContent<T>(HttpResponseMessage responseMessage, string url) where T : class
        {

            if (responseMessage.IsSuccessStatusCode)
            {
                return (await responseMessage.Content.ReadAsByteArrayAsync()) as T;
            }

            throw new Exception($"Posting to '{url}'  failed. {responseMessage.ReasonPhrase ?? ""}");
        }
    }
}
