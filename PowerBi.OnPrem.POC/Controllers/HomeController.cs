using PowerBi.OnPrem.Core;
using PowerBi.OnPrem.POC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PowerBi.OnPrem.POC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SampleDbContext context;
        public HomeController()
        {
            context = new SampleDbContext();
        }
        public async Task<ActionResult> Index()
        {
            var userInfo = (User as CustomPrincipal);
            var accesses = context.FolderAccesses.Where(a => a.UserId == userInfo.UserId).ToList();

            var tasks = accesses.Select(a => PowerBiOnPremClient.GetReportsInFolder(a.FolderId)).ToArray();

            await Task.WhenAll(tasks);

            var items = tasks.SelectMany(c => c.Result)
                            // .Where(c => !c.Hidden)
                            .Select(item =>
                            {
                                var vm = new CatalogItemViewModel
                                {
                                    Visible = !item.Hidden,
                                    CreatedDate = item.CreatedDate,
                                    Id = item.Id,
                                    IsSystemAdmin = userInfo.IsAdmin,
                                    Name = item.Name,
                                    Path = item.Path
                                };
                                vm.IsPersonalItem = accesses.Any(c => c.FolderName == vm.FolderName && c.CanEdit == true);
                                return vm;
                            }).ToList();

            Session.Add("MyReports", items.ToDictionary(c => c.Id));
            var folders = accesses.ToDictionary(c => c.FolderId, c => c.FolderName);
            ViewBag.Folders = folders;
            Session.Add("Folders", folders);
            var ItemsDict = accesses.ToDictionary(a => a.FolderName,
                                                  a => items.Where(c => c.FolderName == a.FolderName).ToList());
            return View(ItemsDict);
        }

        public async Task<ActionResult> Move(Guid id, Guid moveToFolderId)
        {
            //TODO:check if it is 
            var report = (Session["MyReports"] as Dictionary<Guid, CatalogItemViewModel>)[id];
            var folder = (Session["Folders"] as Dictionary<Guid, string>)[moveToFolderId];
            var hasMoved = await PowerBiOnPremClient.MoveReportToFolder($"/{folder}", report.Path);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Clone(Guid id, Guid cloneToFolderId, string newReportName)
        {
            //TODO:check if it is 
            var report = (Session["MyReports"] as Dictionary<Guid, CatalogItemViewModel>)[id];
            var folder = (Session["Folders"] as Dictionary<Guid, string>)[cloneToFolderId];
            var item = await PowerBiOnPremClient.CopyReportToFolder(newReportName, $"{newReportName}.pbix", folder, id);


            return RedirectToAction("Index");
        }

        public ActionResult ViewReport(Guid id)
        {
            var report = (Session["MyReports"] as Dictionary<Guid, CatalogItemViewModel>)[id];
            ViewBag.Message = "View Report";
            ViewBag.ReportName = report.Name;
            var embeddedUrl = ConfigurationManager.AppSettings["PowerBI_Report_Embedded_Url"];
            ViewBag.ReportUrl = string.Format(embeddedUrl, report.Path);

            return View();
        }

        public async Task<FileResult> Download(Guid id)
        {
            var report = (Session["MyReports"] as Dictionary<Guid, CatalogItemViewModel>)[id];
            var bytes = await PowerBiOnPremClient.DownloadReport(id);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, report.FileName);
        }

        public ActionResult NewFolder()
        {
            ViewBag.Message = "Create a New Folder For a New User";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewFolder(User user, string folderName)
        {
            var publicFolderId = Guid.Parse(ConfigurationManager.AppSettings["PowerBI_Public_FolderId"]);
            var publicFolderName = ConfigurationManager.AppSettings["PowerBI_Public_FolderName"];
            var createdFolder = await PowerBiOnPremClient.CreateFolder(folderName);
            user.FolderAccesses = new List<FolderAccess>
            {
                new FolderAccess
                {
                    FolderId = publicFolderId,
                    FolderName = publicFolderName,
                    CanEdit = user.IsAdmin? true: false
                },
                new FolderAccess
                {
                 FolderId = createdFolder.Id,
                 FolderName = folderName,
                 CanEdit = true
                },

            };
            context.Users.Add(user);
            context.SaveChanges();


            return View();
        }
    }
}