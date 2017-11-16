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
        public HomeController(ICategoryService categoryService, IArticleService articleService, IUserService userService)
        {
            base.userService = userService;
            base.articleService = articleService;
            base.categoryService = categoryService;
        }

        // GET: Home
        public ActionResult All(string index)
        {
            int pageIndex = Convert.ToInt32(index ?? "1");
            int pageSize = PageSize;
            int totalCount = 0;
            var articles = articleService.LoadPageEntities(pageIndex, pageSize, out totalCount, a => a.State == 3, a => a.Creation_Time, false)?.ToList();

            ViewBag.Account = GetCurrentAccount();

            ViewBag.Index = pageIndex;
            ViewBag.Page = 1 + (totalCount / pageSize);
            ViewBag.Total = totalCount;
            ViewBag.Categories = categoryService.GetCategories();
            return View(articles);
        }

        public ActionResult Category(string category, string index)
        {
            int pageIndex = Convert.ToInt32(index ?? "1");
            int pageSize = PageSize;
            int totalCount = 0;
            var cate = categoryService.LoadEntity(c=>c.CategoryName.Equals(category, StringComparison.CurrentCultureIgnoreCase));
            if (cate == null)
            {
                throw new HttpException(404, "Not Found!");
                //return HttpNotFound();
            }
            var articles = articleService.LoadPageEntities(pageIndex, pageSize, out totalCount, a => a.State == 3 && a.SystemCategory_Id==cate.Cate_Id, a => a.Creation_Time, false)?.ToList();

            ViewBag.Account = GetCurrentAccount();

            ViewBag.Index = pageIndex;
            ViewBag.Page = 1 + (totalCount / pageSize);
            ViewBag.Total = totalCount;
            ViewBag.Categories = categoryService.GetCategories();
            ViewBag.Key = category;
            ViewBag.Name = cate.Cate_Name;
            return View(articles);
        }
    }
}