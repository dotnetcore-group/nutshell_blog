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
        public ActionResult All(string index)
        {
            int pageIndex = Convert.ToInt32(index ?? "1");
            int pageSize = 20;
            int totalCount = 0;

            var articles = articleService.LoadPageEntities(pageIndex, pageSize, out totalCount, a => true, a => a.Creation_Time)?.ToList();

            ViewBag.Account = GetCurrentAccount();

            ViewBag.Index = pageIndex;
            ViewBag.Page = 1 + (totalCount / pageSize);
            ViewBag.Total = totalCount;
            return View(articles);
        }
    }
}