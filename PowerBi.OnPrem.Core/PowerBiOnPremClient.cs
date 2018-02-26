using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PowerBi.OnPrem.Core
{
    public static class PowerBiOnPremClient
    {
        private static string reportApiBaseUrl;
        static PowerBiOnPremClient()
        {
            string reportUrl = ConfigurationManager.AppSettings["PowerBI_Report_Url"];
            string apiVersion = ConfigurationManager.AppSettings["PowerBI_Api_Version"];

            reportApiBaseUrl = $"{reportUrl}/api/{apiVersion}";
        }

        public static async Task<CatalogItem> CreateReport(string name, string folderName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            string url = $"{reportApiBaseUrl}/PowerBIReports";
            CatalogItem item = new CatalogItem
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = null,
                Type = CatalogItemType.PowerBIReport.ToString(),
                Hidden = false,
                Path = $"/{folderName}/{name}",
                Size = -1,
                Content = "",
                IsFavorite = false,
                ModifiedDate = DateTimeOffset.Now,
                CreatedDate = DateTimeOffset.Now
            };
            CatalogItem newItem = await HttpHelper.Post(url, item);
            return newItem;
        }

        public static async Task<CatalogItem> CreateFolder(string folderName, string folderDescription = null)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            string url = $"{reportApiBaseUrl}/CatalogItems";
            CatalogItem item = new CatalogItem
            {
                Id = Guid.NewGuid(),
                Name = folderName,
                Description = folderDescription,
                Type = CatalogItemType.Folder.ToString(),
                Hidden = false,
                Path = $"/{folderName}",
                Size = -1,
                Content = "",
                IsFavorite = false,
                ModifiedDate = DateTimeOffset.Now,
                CreatedDate = DateTimeOffset.Now
            };
            CatalogItem newItem = await HttpHelper.Post(url, item);
            return newItem;
        }

        public static async Task<List<CatalogItem>> GetReportsInFolder(Guid folderId)
        {
            if (folderId == Guid.Empty)
            {
                throw new ArgumentException(nameof(folderId));
            }

            string url = $"{reportApiBaseUrl}/Folders({folderId})/CatalogItems";

            var jObject = await HttpHelper.Get<JObject>(url);
            var items = JsonConvert.DeserializeObject<List<CatalogItem>>(jObject.SelectToken("$.value").ToString());
            return items;
        }

        public static async Task<byte[]> DownloadReport(Guid reportId)
        {
            if (reportId == Guid.Empty)
            {
                throw new ArgumentException(nameof(reportId));
            }

            string url = $"{reportApiBaseUrl}/CatalogItems({reportId})/Content/$value";

            var reportBytes = await HttpHelper.Get<byte[]>(url, ReponseContent.Bytes);
            return reportBytes;
        }

        public static async Task<bool> MoveReportToFolder(string folderPath, string reportPath)
        {
            if (string.IsNullOrEmpty(reportPath))
            {
                throw new ArgumentException(nameof(reportPath));
            }

            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentException(nameof(folderPath));
            }

            string url = $"{reportApiBaseUrl}/CatalogItems/Model.MoveItems";

            var moveItemRequest = new MoveItemsRequest
            {
                CatalogItemPaths = new[] { reportPath },
                TargetPath = folderPath
            };

            var response = await HttpHelper.Post<MoveItemsRequest, MoveItemsResponse>(url, moveItemRequest);

            if (response.HasErrors)
            {
                throw new Exception(string.Concat(response.FailedOperations));
            }
            return true;
        }

        public static async Task<CatalogItem> UploadReport(Guid reportId, byte[] bytes, string name, string fileName)
        {
            if (reportId == Guid.Empty)
            {
                throw new ArgumentException(nameof(reportId));
            }

            string url = $"{reportApiBaseUrl}/PowerBIReports({reportId})/Model.Upload";

            var item = await HttpHelper.UploadFile<CatalogItem>(url, bytes, name, fileName);
            return item;
        }

        public static async Task<CatalogItem> CopyReportToFolder(string newReportName, string fileName, string destinationFolderName, Guid existingReportId)
        {
            if (string.IsNullOrEmpty(newReportName) || string.IsNullOrEmpty(fileName)
                || string.IsNullOrEmpty(destinationFolderName) || existingReportId == Guid.Empty)
            {
                throw new ArgumentException();
            }

            //Create report
            var newReportTask = CreateReport(newReportName, destinationFolderName);

            //Download report
            var reportBytesTask = DownloadReport(existingReportId);

            await Task.WhenAll(newReportTask, reportBytesTask);

            //Upload report
            var item = await UploadReport(newReportTask.Result.Id, reportBytesTask.Result, newReportName, fileName);

            return item;
        }

        public static async Task<bool> DeleteReport(Guid reportId)
        {
            if (reportId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(reportId));
            }

            string url = $"{reportApiBaseUrl}/PowerBIReports({reportId})";
            var deleted = await HttpHelper.Delete(url);

            return deleted;
        }

        public static async Task<CatalogItem> AddNewReport(string reportName, string fileName, string folderName, byte[] reportBytes)
        {
            //create report item
            var newReport = await CreateReport(reportName, folderName);

            //upload report
            var item = await UploadReport(newReport.Id, reportBytes, reportName, fileName);
            return item;
        }

    }
}
