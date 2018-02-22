using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PowerBi.OnPrem.Core
{
    public static class HttpHelper
    {
        public static async Task<T> Post<T>(string url, T item)
        {
            var itemJson = JsonConvert.SerializeObject(item);
            var content = new StringContent(itemJson);
            HttpClient client = new HttpClient();
            var responseMessage = await client.PostAsync(url, content);
            var responseString = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }

            throw new Exception($"Posting to '{url}'  failed.", new Exception(responseString));
        }
    }
    public static class PowerBiOnPremClient
    {
        private static string reportApiBaseUrl;
        static PowerBiOnPremClient()
        {
            string reportUrl = ConfigurationManager.AppSettings["PowerBI_Report_Url"];
            string apiVersion = ConfigurationManager.AppSettings["PowerBI_Api_Version"];

            reportApiBaseUrl = $"{reportUrl}/api/{apiVersion}";

            //http://pwcsubk/Reports/api/v2.0/Folders
        }

        public static async Task<CatalogItem> CreateFolder(string folderName, string folderDescription = null)
        {
            if(string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            string url = $"{reportApiBaseUrl}/Folders";
            CatalogItem item = new CatalogItem
            {
                Name = folderName,
                Description = folderDescription,
                Type = "Folder",
                Hidden = false,
            };
            CatalogItem newItem = await HttpHelper.Post(url, item);
            return newItem;
        }
    }
}
