using Nutshell.Blog.Common;
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
        public ActionResult Index(string index)
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
            var cate = categoryService.LoadEntity(c => c.CategoryName.Equals(category, StringComparison.CurrentCultureIgnoreCase));
            if (cate == null)
            {
                throw new HttpException(404, "Not Found!");
                //return HttpNotFound();
            }
            var articles = articleService.LoadPageEntities(pageIndex, pageSize, out totalCount, a => a.State == 3 && a.SystemCategory_Id == cate.Cate_Id, a => a.Creation_Time, false)?.ToList();

            ViewBag.Account = GetCurrentAccount();

            ViewBag.Index = pageIndex;
            ViewBag.Page = 1 + (totalCount / pageSize);
            ViewBag.Total = totalCount;
            ViewBag.Categories = categoryService.GetCategories();
            ViewBag.Key = category;
            ViewBag.Name = cate.Cate_Name;
            return View(articles);
        }

        public ActionResult About()
        {
            return View();
        }

        public JsonResult Send()
        {
            var data = new { msg = "发送失败!" };

            if (EmailHelper.Send("1809636031@qq.com", "测试邮件", "<span style='color:red'>这是一封测试邮件，您能收到此邮件，说明您的邮箱参数设置正确，请勿回复此邮件。</span>"))
            {
                data = new { msg="发送成功！"};
            }

            return Json(data);
        }
    }
}