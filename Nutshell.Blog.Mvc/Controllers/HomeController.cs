using Nutshell.Blog.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUserService userService)
        {
            base.userService = userService;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}