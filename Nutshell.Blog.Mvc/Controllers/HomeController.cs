using Nutshell.Blog.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public HomeController(IArticleService articleService, IUserService userService)
        {
            base.userService = userService;
            base.articleService = articleService;
        }

        // GET: Home
        public ActionResult Index()
        {
            var articles = articleService.LoadEntities(a => true)?.ToList();
            ViewBag.Account = GetCurrentAccount();
            return View(articles);
        }
    }
}