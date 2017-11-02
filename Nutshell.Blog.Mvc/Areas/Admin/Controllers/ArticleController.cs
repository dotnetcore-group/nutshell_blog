using Nutshell.Blog.Common;
using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{

    [CheckUserLogin]
    public class ArticleController : BaseController
    {
        public ArticleController(IArticleService articleService, IUserService userService, ICustomCategoryService customCategoryService)
        {
            base.userService = userService;
            base.articleService = articleService;
            base.customCategoryService = customCategoryService;
        }

        // 写文章 / 编辑文章
        // 当 postid 为null 写文章
        // 不为null 判断此文章是否为此登录用户的
        // false 不能编辑 true 显示文章信息 可编辑
        public ActionResult Edit(int? postid)
        {
            var user = GetCurrentAccount();

            ViewBag.CustomCategories = customCategoryService.LoadEntities(c => c.Author.User_Id == user.User_Id)?.ToList();
            if (postid.HasValue)
            {
                var article = articleService.LoadEntity(a => a.Article_Id == postid.Value);
                if (article != null)
                {
                    if (article.Author_Id == user.User_Id)
                    {
                        return View(article);
                    }
                    else
                    {
                        return JavaScript("<script>alert('你没有操作权限！');javascript:history.go(-1)</script>");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View();
        }

        public ActionResult BlogList()
        {
            return View();
        }

        // 文章审核
        [SupportFilter(Action = "Examine")]
        public ActionResult ArticleExamine()
        {
            return View();
        }


        [SupportFilter(Action = "Examine")]
        [HttpPost]
        public JsonResult ExaminePass(int id)
        {
            var data = new { code = 1, msg = "审核失败，请重试！" };
            var article = articleService.LoadEntity(a => a.Article_Id == id);
            if (article != null)
            {
                article.State = (int)ArticleSateEnum.Published;
                articleService.EditEntity(article);
                var res = articleService.SaveChanges();
                if (res)
                {
                    data = new { code = 0, msg = "审核通过，已发布！" };
                }
            }
            else
            {
                data = new { code = 1, msg = "该文章不存在，请重试！" };
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetBlogs()
        {
            int draw = Convert.ToInt32(Request["draw"] ?? "0");
            int start = Convert.ToInt32(Request["start"] ?? "1");
            int length = Convert.ToInt32(Request["length"] ?? "10");
            var account = GetCurrentAccount();
            int total;
            var list = articleService.LoadPageEntities((start + length) / length, length, out total, a => a.Author_Id == account.User_Id, a => a.Creation_Time, false).Select(a => new
            {
                a.Article_Id,
                a.Title,
                a.Creation_Time,
                a.State,
                a.CustomCategory.CategoryName
            });
            return Json(new { data = list, draw, recordsTotal = total, recordsFiltered = total });
        }

        [HttpPost]
        public JsonResult GetArticles()
        {
            int draw = Convert.ToInt32(Request["draw"] ?? "0");
            int start = Convert.ToInt32(Request["start"] ?? "1");
            int length = Convert.ToInt32(Request["length"] ?? "10");
            int total;
            var list = articleService.LoadPageEntities((start + length) / length, length, out total, a => a.State != (int)ArticleSateEnum.Draft && a.State != (int)ArticleSateEnum.Deleted, a => a.Creation_Time, false).Select(a => new
            {
                a.Article_Id,
                a.Title,
                a.Creation_Time,
                a.State,
                a.CustomCategory.CategoryName,
                a.Author.Nickname
            });
            return Json(new { data = list, draw, recordsTotal = total, recordsFiltered = total });
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
        public JsonResult PostArticle(Article article)
        {
            var res = new { code = 1, msg = "保存失败" };

            var account = GetCurrentAccount();

            if (account != null)
            {
                var user = userService.LoadEntity(u => u.User_Id == account.User_Id);
                if (user != null)
                {
                    article.Author_Id = user.User_Id;
                    var customCate = customCategoryService.LoadEntity(c => c.Author.User_Id == user.User_Id && c.Id == article.CustomCategory_Id);
                    if (ModelState.IsValid && customCate != null)
                    {
                        article.Body = Server.HtmlEncode(article.Body);
                        article.Content = article.Content.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Replace(" ", "");
                        article.Introduction = article.Content.Length > 200 ? article.Content.Substring(0, 190) + "..." : article.Content;

                        if (articleService.AddArticle(article) != null)
                        {
                            res = new { code = 0, msg = "提交成功！" };
                        }
                    }
                }
            }
            return Json(res);
        }
    }
}