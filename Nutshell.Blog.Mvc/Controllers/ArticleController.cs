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
using Nutshell.Blog.Common;

namespace Nutshell.Blog.Mvc.Controllers
{
    [CheckUserLogin]
    public class ArticleController : BaseController
    {
        public ArticleController(IArticleService artService, IUserService uService, IFavoritesService favoritesService, ICommentService commentService, ICustomCategoryService customCategoryService, ICategoryService categoryService)
        {
            userService = uService;
            articleService = artService;
            base.commentService = commentService;
            base.favoritesService = favoritesService;
            base.customCategoryService = customCategoryService;
            base.categoryService = categoryService;
        }

        // 写文章 / 编辑文章
        // 当 postid 为null 写文章
        // 不为null 判断此文章是否为此登录用户的
        // false 不能编辑 true 显示文章信息 可编辑
        public ActionResult Edit(int? postid)
        {
            ViewBag.IsEditMode = false;
            ViewBag.Title = "写文章";

            var user = GetCurrentAccount();

            ViewBag.CustomCategories = customCategoryService.LoadEntities(c => c.Author.User_Id == user.User_Id)?.ToList();
            ViewBag.SystemCategories = categoryService.GetCategories();
            if (postid.HasValue)
            {
                ViewBag.Title = "编辑文章";
                ViewBag.IsEditMode = true;
                var article = articleService.LoadEntity(a => a.Article_Id == postid.Value);
                if (article != null)
                {
                    if (article.Author_Id == user.User_Id)
                    {
                        return View(article);
                    }
                    else
                    {
                        return new JavaScriptResult() { Script = "<script>alert('你没有操作权限！');javascript:history.go(-1)</script>" };// JavaScript("");
                    }
                }
                else
                {
                    throw new HttpException(404, "Not Found!");
                }
            }
            return View();
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
            ViewBag.Page = PagerHelper.GetTotalPage(pageSize, totalCount);
            watch.Stop();
            // 搜索所需时间
            ViewBag.Time = watch.ElapsedMilliseconds;
            return View(list);
        }

        // 用户博客
        // GET:/username
        public ActionResult Blogs(string author, int? page = 1)
        {
            var user = userService.LoadEntity(u => u.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase));
            if (user != null)
            {
                var pageIndex = page.HasValue ? (page.Value <= 0 ? 1 : page.Value) : 1;
                var pageSize = PageSize;

                // 文章档案
                ViewBag.Archives = articleService.GetArchivesByUserId<Archives>(user.User_Id);
                // 文章分类
                ViewBag.Categories = articleService.GetCustomCategoriesByUserId<CustomCategories>(user.User_Id);
                ViewBag.UserInfo = user;

                // 上一页、下一页
                var totalCount = articleService.GetArticlesTotalCount(user.User_Id);
                var totalPage = PagerHelper.GetTotalPage(PageSize, totalCount);// totalCount % PageSize == 0 ? totalCount / PageSize : (totalCount + PageSize) / PageSize;
                if (pageIndex > 1 && pageIndex <= totalPage)
                {
                    ViewBag.HasPre = true;
                    ViewBag.Pre = pageIndex - 1;
                }
                if (pageIndex < totalPage)
                {
                    ViewBag.HasNext = true;
                    ViewBag.Next = pageIndex + 1;
                }
                if (pageIndex > totalPage)
                {
                    pageIndex = 1;
                }
                // TODO : 可以优化
                // 置顶文章只在第一页（置顶文章数量最多20）
                if (pageIndex <= 1)
                {
                    // 置顶文章
                    var topList = articleService.LoadEntities(a => a.Author_Id == user.User_Id && a.IsTop)
                        .OrderByDescending(a => a.Creation_Time);
                    ViewBag.TopList = topList;

                    // 其他文章数量 = 设定数量 - 置顶文章数量
                    pageSize -= topList.Count();
                    if (pageSize > 0)
                    {
                        // 按日期分组的文章 类型为 IEnumerable<IGrouping<DateTime, Article>>
                        var articles = articleService.LoadEntities(a => a.Author_Id == user.User_Id && a.State == 3 && !a.IsTop)
                            .OrderByDescending(a => a.Creation_Time)
                            .Take(pageSize)
                            .GroupBy(new Func<Article, DateTime>(a =>
                            {
                                var date = Convert.ToDateTime(a.Creation_Time.ToShortDateString());
                                return date;
                            }));
                        return View(articles);
                    }
                }
                // 其他非置顶文章（排序：置顶在前，跳过第一页）
                else
                {
                    // 按日期分组的文章 类型为 IEnumerable<IGrouping<DateTime, Article>>
                    var articles = articleService.LoadPageEntities(a => a.Author_Id == user.User_Id && a.State == 3, pageIndex, pageSize)
                        .GroupBy(new Func<Article, DateTime>(a =>
                        {
                            var date = Convert.ToDateTime(a.Creation_Time.ToShortDateString());
                            return date;
                        }));
                    return View(articles);
                }
                return View();
            }
            throw new HttpException(404, "Not Found!");
        }

        // GET:/username/p/1.html
        [AllowAnonymous]
        public ActionResult Detail(string author, int? id)
        {
            if (!id.HasValue || string.IsNullOrEmpty(author))
            {
                throw new HttpException(404, "Not Found!");
                //return HttpNotFound();
            }
            var article = articleService.LoadEntity(a => a.Article_Id == id && a.Author.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase) && a.State == (int)ArticleStateEnum.Published);
            ViewBag.Author = author;
            if (article == null)
            {
                throw new HttpException(404, "Not Found!");
                //return HttpNotFound();
            }
            var user = article.Author;
            // 随笔档案
            ViewBag.Archives = articleService.GetArchivesByUserId<Archives>(user.User_Id);
            // 随笔分类
            ViewBag.Categories = articleService.GetCustomCategoriesByUserId<CustomCategories>(user.User_Id);
            ViewBag.UserInfo = user;

            // 上一篇 下一篇
            var articles = articleService.LoadEntities(a => a.Author_Id == article.Author_Id && a.State == (int)ArticleStateEnum.Published && a.Article_Id != id);
            ViewBag.Before = articles.OrderBy(a => a.Creation_Time).Where(a => a.Creation_Time >= article.Creation_Time).FirstOrDefault();
            ViewBag.Next = articles.OrderByDescending(a => a.Creation_Time).Where(a => a.Creation_Time <= article.Creation_Time).FirstOrDefault();
            var account = GetCurrentAccount();
            if (account != null)
            {
                ViewBag.HavaCollection = favoritesService.HaveCollection(account.User_Id, id.Value);
            }

            return View(article);
        }

        [HttpPost]
        public JsonResult Comment(string content, int? id)
        {
            var res = new JsonModel { code = 0, msg = "评论失败！" };
            var account = GetCurrentAccount();
            if (!id.HasValue)
            {
                return Json(res);
            }
            commentService.AddEntity(new Discussion
            {
                Article_Id = id.Value,
                Content = content,
                User_Id = account.User_Id
            });
            if (commentService.SaveChanges())
            {
                res.code = 0;
                res.msg = "评论成功！";
            }
            return Json(res);
        }

        [AllowAnonymous]
        public JsonResult GetComments(int? id)
        {
            var res = new JsonModel { code = 1, msg = "获取评论失败！" };
            if (!id.HasValue)
            {
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            var comments = commentService.LoadEntities(c => c.Article_Id == id.Value).OrderBy(c => c.Discussion_Time).Select(m => new
            {
                m.Id,
                m.Reply_Id,
                m.User.Nickname,
                userName = m.User.Login_Name,
                m.Content,
                parentNick = m.ParentDiscussion.User.Nickname,
                parentName = m.ParentDiscussion.User.Login_Name,
                m.Discussion_Time
            })?.ToList();
            res.code = 0;
            res.msg = "获取评论成功！";
            res.res = comments;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadImage()
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

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostArticle(Article article, int? articleid)
        {
            var res = new { code = 1, msg = "保存失败" };

            var account = GetCurrentAccount();

            if (account != null)
            {
                var user = userService.LoadEntity(u => u.User_Id == account.User_Id);
                if (user != null)
                {
                    var customCate = customCategoryService.LoadEntity(c => c.Author.User_Id == user.User_Id && c.Id == article.CustomCategory_Id);

                    article.Author_Id = user.User_Id;
                    if (ModelState.IsValid && customCate != null)
                    {
                        article.Title = Server.HtmlEncode(article.Title);
                        article.Body = Server.HtmlEncode(article.Body);
                        article.Content = article.Content.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Replace(" ", "");
                        article.Introduction = article.Content.Length > 200 ? article.Content.Substring(0, 190) + "..." : article.Content;
                        // 编辑文章
                        if (articleid.HasValue)
                        {
                            var newArticle = articleService.LoadEntity(a => a.Article_Id == articleid);
                            if (newArticle != null)
                            {
                                newArticle.Title = article.Title;
                                newArticle.Body = article.Body;
                                newArticle.Content = article.Content;
                                newArticle.Introduction = article.Introduction;
                                newArticle.SystemCategory_Id = article.SystemCategory_Id;
                                newArticle.CustomCategory_Id = article.CustomCategory_Id;
                            }

                            articleService.EditEntity(newArticle);
                        }
                        else
                        {
                            articleService.AddEntity(article);
                        }
                        if (articleService.SaveChanges())
                        {
                            res = new { code = 0, msg = "提交成功！" };
                        }
                    }

                }
            }
            return Json(res);
        }

        // 由草稿箱发布
        [HttpPost]
        public JsonResult Publish(int id)
        {
            var data = new { code=1, msg="发布失败！"};
            var article = articleService.LoadEntity(a=>a.Article_Id==id && a.Author_Id==Account.User_Id);
            if (article != null)
            {
                article.State = (int)ArticleStateEnum.NotAudited;
                articleService.EditEntity(article);
                if (articleService.SaveChanges())
                {
                    data = new { code = 0, msg = "发布成功，请等待审核！" };
                }
            }
            return Json(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostDraft(Article article)
        {
            var data = new JsonModel { code = 1, msg = "保存失败，请重试！" };
            var account = GetCurrentAccount();
            if (ModelState.IsValid)
            {
                article.Author_Id = account.User_Id;
                article.Title = Server.HtmlEncode(article.Title);
                article.Body = Server.HtmlEncode(article.Body);
                article.Content = article.Content.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Replace(" ", "");
                article.Introduction = article.Content.Length > 200 ? article.Content.Substring(0, 190) + "..." : article.Content;
                article.State = (int)ArticleStateEnum.Draft;
                articleService.AddEntity(article);
                if (articleService.SaveChanges())
                {
                    data.code = 0;
                    data.msg = "保存成功！";
                }
            }
            return Json(data);
        }
    }
}