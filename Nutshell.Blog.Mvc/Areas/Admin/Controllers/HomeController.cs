using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{
    [CheckUserLogin]
    public class HomeController : BaseController
    {
        public HomeController(IModuleService moduleService)
        {
            base.moduleService = moduleService;
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            var account = GetCurrentAccount() ?? new Model.ViewModel.Account();
            return View(account);
        }

        public ActionResult Desktop()
        {
            return View();
        }

        public JsonResult GetMenu()
        {
            dynamic menu = null;
            var account = GetCurrentAccount();
            if (account != null)
            {
                menu = moduleService.GetMenuByPersonId(account.User_Id)
                    .Select(m => new { m.Id, m.Name, m.Url, m.Parent_Id , m.IsLast, m.Iconic});
            }

            return Json(new { menu }, JsonRequestBehavior.AllowGet);
        }
    }
}