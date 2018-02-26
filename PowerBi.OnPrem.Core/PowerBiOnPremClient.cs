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

        private static async Task<CatalogItem> CreateCatalogItem(string name, string description, CatalogItemType catalogItemType, string path)
        {
           

            string url = $"{reportApiBaseUrl}/CatalogItems";
            CatalogItem item = new CatalogItem
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Type = catalogItemType.ToString(),
                Hidden = false,
                Path = path,
                Size = -1,
                Content = "",
                IsFavorite = false,
                ModifiedDate = DateTimeOffset.Now,
                CreatedDate = DateTimeOffset.Now
            };
            CatalogItem newItem = await HttpHelper.Post(url, item);
            return newItem;
        }

        public static async Task<CatalogItem> CreateReport(string name, string folderName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await CreateCatalogItem(name, null, CatalogItemType.Report, $"/{folderName}/{name}");
        }

        public static async Task<CatalogItem> CreateFolder(string folderName, string folderDescription = null)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            return await CreateCatalogItem(folderName, folderDescription, CatalogItemType.Folder, $"/{folderName}");
        }

        public static async Task<List<CatalogItem>> GetReportsInFolder(Guid folderId)
        {
            if (folderId == Guid.Empty)
            {
                throw new ArgumentException(nameof(folderId));
            }

            string url = $"{reportApiBaseUrl}/Folders({folderId})/CatalogItems";

            var items = await HttpHelper.Get<List<CatalogItem>>(url);
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
            //Create report
            var newReport = await CreateReport(newReportName, destinationFolderName);

            //Download report
            var reportBytes = await DownloadReport(existingReportId);

            //Upload report
           var item = await UploadReport(newReport.Id, reportBytes, newReportName, fileName);

            return item;
        }

    }
}
