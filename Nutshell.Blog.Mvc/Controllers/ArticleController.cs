using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    [CheckUserLogin]
    public class ArticleController : BaseController
    {
        public ArticleController(IArticleService artService, IUserService uService)
        {
            userService = uService;
            articleService = artService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Search(string q)
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"] ?? "1");
            var pageSize = 10;
            var totalCount = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // 页码
            ViewBag.Index = pageIndex;
            if (string.IsNullOrWhiteSpace(q))
            {
                ViewBag.Count = 0;
                ViewBag.Time = 0;
                ViewBag.Page = 1;
                return View();
            }

            List<SearchArticleResult> list = null;
            try
            {
                list = PanGuLuceneHelper.Instance.Search(q, pageIndex, pageSize, out totalCount);
            }
            catch (Exception e)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }

            // 结果数
            ViewBag.Count = totalCount;
            // 搜索词
            ViewBag.Query = q;
            // 总页数
            ViewBag.Page = 1 + (totalCount / pageSize);
            watch.Stop();
            // 搜索所需时间
            ViewBag.Time = watch.ElapsedMilliseconds;
            return View(list);
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostArticle(Article article)
        {
            var art = "";
            //if (ModelState.IsValid)
            //{
            var account = GetCurrentAccount();

            if (account != null)
            {
                var user = userService.LoadEntity(u => u.User_Id == account.User_Id);
                if (user != null)
                {
                    article.Author_Id = user.User_Id;
                    article.Body = Server.HtmlEncode(article.Body);
                    article.Content = article.Content.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Replace(" ", "");
                    article.Introduction = article.Content.Length > 200 ? article.Content.Substring(0, 190) + "..." : article.Content;
                    article.Author = user;
                    art = articleService.AddArticle(article) == null ? "no" : "ok";
                    //}
                    return Json(new { art });
                }
            }
            return Json(new { art = "no" });
        }

        [AllowAnonymous]
        public ActionResult Detail(string author, int? id)
        {
            if (!id.HasValue || string.IsNullOrEmpty(author))
            {
                return HttpNotFound();
            }
            var article = articleService.LoadEntity(a => a.Article_Id == id && a.Author.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase));
            ViewBag.Author = author;
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserInfo = article.Author;
            return View(article);
        }


        // 用户博客
        [AllowAnonymous]
        public ActionResult Blogs(string author)
        {
            var user = userService.LoadEntity(u => u.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase));
            if (user != null)
            {
                ViewBag.UserInfo = user;

                // IEnumerable<IGrouping<DateTime, Article>>
                var articles = articleService.LoadEntities(a => a.Author.User_Id == user.User_Id)
                    .OrderByDescending(a => a.Creation_Time)
                    .GroupBy(new Func<Article, DateTime>(a =>
                    {
                        var date = Convert.ToDateTime(a.Creation_Time.ToShortDateString());
                        return date;
                    }));
                return View(articles);
            }
            return HttpNotFound();
        }

        public JsonResult Comment(string content)
        {
            return Json(content);
        }
    }
}