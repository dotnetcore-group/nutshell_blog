using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Core.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(IArticleService artService, IUserService uService)
        {
            userService = uService;
            articleService = artService;
        }

        [HttpGet]
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

        // GET:/username/p/1.html
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
            var user = article.Author;
            // 随笔档案
            ViewBag.Archives = articleService.GetArchivesByUserId<Archives>(user.User_Id);
            // 随笔分类
            ViewBag.Categories = articleService.GetCustomCategoriesByUserId<CustomCategories>(user.User_Id);

            // 上一篇 下一篇
            var articles = articleService.LoadEntities(a => a.Author_Id == article.Author_Id && a.Article_Id != id);
            ViewBag.Before = articles.OrderBy(a => a.Creation_Time).Where(a => a.Creation_Time >= article.Creation_Time).FirstOrDefault();
            ViewBag.Next = articles.OrderByDescending(a => a.Creation_Time).Where(a => a.Creation_Time <= article.Creation_Time).FirstOrDefault();

            ViewBag.UserInfo = user;
            return View(article);
        }

        // 用户博客
        // GET:/username
        public ActionResult Blogs(string author, int? page = 1)
        {
            var user = userService.LoadEntity(u => u.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase));
            if (user != null)
            {
                var pageIndex = page.HasValue ? (page.Value<=0? 1: page.Value) : 1;
                var pageSize = PageSize;
                ViewBag.UserInfo = user;

                // 置顶文章只在第一页（置顶文章数量最多20）
                if (pageIndex <= 1)
                {
                    // 置顶文章
                    var topList = articleService.LoadEntities(a => a.Author_Id == user.User_Id && a.IsTop)
                        .OrderByDescending(a => a.Creation_Time);
                    ViewBag.TopList = topList;

                    // 其他文章数量 = 设定数量 - 置顶文章数量
                    pageSize -= topList.Count();
                }

                // 文章档案
                ViewBag.Archives = articleService.GetArchivesByUserId<Archives>(user.User_Id);
                // 文章分类
                ViewBag.Categories = articleService.GetCustomCategoriesByUserId<CustomCategories>(user.User_Id);

                // 上一页、下一页
                var totalCount = articleService.GetArticlesTotalCount(user.User_Id);
                var totalPage = totalCount % PageSize ==0 ? totalCount /PageSize : (totalCount + PageSize)/PageSize;
                if (pageIndex > 1)
                {
                    ViewBag.HasPre = true;
                    ViewBag.Pre = pageIndex - 1;
                }
                if (pageIndex < totalPage)
                {
                    ViewBag.HasNext = true;
                    ViewBag.Next = pageIndex + 1;
                }

                // 设定的pagesize大小减去置顶文章数 为取其他文章的数量
                if (pageSize > 0)
                {
                    int count = 0;
                    // 按日期分组的文章 类型为 IEnumerable<IGrouping<DateTime, Article>>
                    var articles = articleService.LoadPageEntities(pageIndex, pageSize, out count, a => a.Author_Id == user.User_Id && !a.IsTop, a => a.Creation_Time, false)
                        .GroupBy(new Func<Article, DateTime>(a =>
                        {
                            var date = Convert.ToDateTime(a.Creation_Time.ToShortDateString());
                            return date;
                        }));
                    return View(articles);
                }
                return View();
            }
            return HttpNotFound();
        }

        [CheckUserLogin]
        public JsonResult Comment(string content)
        {
            return Json(content);
        }
    }
}