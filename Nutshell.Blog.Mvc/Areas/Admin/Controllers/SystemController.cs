using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{
    public class SystemController : Controller
    {
        // GET: Admin/System/Settings
        public ActionResult Settings()
        {
            return View();
        }
    }
}