using PowerBi.OnPrem.Core;
using PowerBi.OnPrem.POC.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult NewFolder()
        {
            ViewBag.Message = "Create a New Folder For a New User";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewFolder(User user, string folderName)
        {
            context.Users.Add(user);
            context.SaveChanges();

            await PowerBiOnPremClient.CreateFolder(folderName);

            return View();
        }
    }
}