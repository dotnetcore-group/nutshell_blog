using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Filters;
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

        // 写文章 / 编辑文章
        // 当 postid 为null 写文章
        // 不为null 判断此文章是否为此登录用户的
        // false 不能编辑 true 显示文章信息 可编辑
        [CheckUserLogin]
        public ActionResult Edit(int? postid)
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [CheckUserLogin]
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
                var pageIndex = page.HasValue ? page : 1;
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
                
                // 设定的pagesize大小减去置顶文章数 为取其他文章的数量
                if (pageSize > 0)
                {
                    int count = 0;
                    // 按日期分组的文章 类型为 IEnumerable<IGrouping<DateTime, Article>>
                    var articles = articleService.LoadPageEntities(1, pageSize, out count, a => a.Author_Id == user.User_Id && !a.IsTop, a => a.Creation_Time, false)
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

        public ActionResult UploadImage()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var saveUrl = $"/upload/images/{date}/";
            //文件保存目录路径
            string savePath = Server.MapPath(saveUrl);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;

            //HttpPostedFileBase imgFile = Request.Files["imgFile"];
            HttpPostedFileBase imgFile = Request.Files[0];
            if (imgFile == null)
            {
                return Json(new { error = 1, message = "请选择文件！" });
            }

            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Json(new { error = 1, message = "上传文件大小超过限制！" });
            }
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(extTable["image"].ToString().Split(','), fileExt.Substring(1).ToLower()) < 0)
            {
                return Json(new { error = 1, message = $"上传文件扩展名是不允许的扩展名！\n只允许{extTable["image"].ToString()}。" });
            }


            string newFileName = Guid.NewGuid().ToString("N") + fileExt;
            string filePath = savePath + newFileName;

            imgFile.SaveAs(filePath);

            string fileUrl = Path.Combine(saveUrl, newFileName);

            return Json(new { error = 0, url = fileUrl });
        }
    }
}