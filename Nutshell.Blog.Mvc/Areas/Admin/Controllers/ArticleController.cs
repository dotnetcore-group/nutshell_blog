using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Controllers;
using Nutshell.Blog.Mvc.Hubs;
using Nutshell.Blog.Mvc.MvcHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{

    [CheckUserLogin]
    public class ArticleController : BaseController
    {
        public ArticleController(IArticleService articleService, IUserService userService, ICustomCategoryService customCategoryService, IMessageService messageService)
        {
            base.messageService = messageService;
            base.userService = userService;
            base.articleService = articleService;
            base.customCategoryService = customCategoryService;
        }

        [SupportFilter(Action = "BlogList")]
        public ActionResult BlogList()
        {
            return PartialView();
        }

        // 文章审核
        [SupportFilter(Action = "Examine")]
        public ActionResult ArticleExamine()
        {
            return PartialView();
        }

        // 预览
        [SupportFilter(Action = "Examine")]
        public ActionResult Preview(int id)
        {
            var article = articleService.LoadEntity(a => a.Article_Id == id);
            if (article == null)
            {
                throw new HttpException(404, "Not Found!");
                //return HttpNotFound();
            }
            return PartialView(article);
        }

        [SupportFilter(Action = "Examine")]
        [HttpPost]
        public JsonResult ExaminePass(int id)
        {
            var data = new { code = 1, msg = "审核失败，请重试！" };
            var article = articleService.LoadEntity(a => a.Article_Id == id);
            if (article != null)
            {
                article.State = (int)ArticleStateEnum.Published;
                articleService.EditEntity(article);
                var res = articleService.SaveChanges();
                if (res)
                {
                    SendExamineMessage(article);
                    data = new { code = 0, msg = "审核通过，已发布！" };
                }
            }
            else
            {
                data = new { code = 1, msg = "该文章不存在，请重试！" };
            }
            return Json(data);
        }

        [SupportFilter(Action = "Examine")]
        [HttpPost]
        public JsonResult ExamineOut(int id, string msg)
        {
            var data = new { code = 1, msg = "审核失败，请重试！" };
            var article = articleService.LoadEntity(a => a.Article_Id == id);
            if (article != null)
            {
                article.State = (int)ArticleStateEnum.NotPass;
                articleService.EditEntity(article);
                var res = articleService.SaveChanges();
                if (res)
                {
                    SendExamineMessage(article, msg);
                    data = new { code = 0, msg = "该文章不通过！" };
                }
            }
            else
            {
                data = new { code = 1, msg = "该文章不存在，请重试！" };
            }
            return Json(data);
        }

        private void SendExamineMessage(Article article, string msg = "")
        {
            var message = new Message();
            message.Recipient = article.Author;
            if (article.State == (int)ArticleStateEnum.Published)
            {
                message.Title = "文章通过审核";
                message.Content = $"您的文章 <a target='_blank' href='/{article.Author.Login_Name}/p/{article.Article_Id}.html'>{article.Title}</a> 通过了审核，已发布。";
            }
            else
            {
                message.Title = "文章未通过审核";
                message.Content = $"您的文章 <a target='_blank' href='/{article.Author.Login_Name}/p/{article.Article_Id}.html'>{article.Title}</a> 未通过了审核，原因为：{msg}。请修改后提交，点击<a target='_blank' href='/article/edit?postid={article.Article_Id}'>这里</a>修改。";
            }
            messageService.AddEntity(message);
            messageService.SaveChanges();
        }

        [SupportFilter(Action = "Delete")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = new { code = 1, msg = "删除失败，请重试！" };
            var account = GetCurrentAccount();
            var article = articleService.LoadEntity(a => a.Article_Id == id && a.Author_Id == account.User_Id);
            if (article != null)
            {
                article.State = (int)ArticleStateEnum.Deleted;
                articleService.EditEntity(article);
                if (articleService.SaveChanges())
                {
                    data = new { code = 0, msg = "文章已删除！" };
                }
            }
            return Json(data);
        }

        [HttpPost]
        [SupportFilter(Action = "BlogList")]
        public JsonResult GetBlogs()
        {
            var draw = Convert.ToInt32(Request["draw"] ?? "0");
            var start = Convert.ToInt32(Request["start"] ?? "1");
            var length = Convert.ToInt32(Request["length"] ?? "10");
            var type = Convert.ToInt32(Request["type"] ?? "0");

            int total;

            Expression<Func<Article, bool>> baseWhere = a => a.State != (int)ArticleStateEnum.Deleted && a.State != (int)ArticleStateEnum.Draft;

            //var temp = articleService.LoadPageEntities((start + length) / length, length, out total, a => a.State != (int)ArticleStateEnum.Deleted, a => a.Creation_Time, false);
            if (type != 0)
            {
                baseWhere = LinqExtensions.And(baseWhere, a => a.State == type);
                //temp = articleService.LoadPageEntities((start + length) / length, length, out total, a => a.State == type, a => a.Creation_Time, false);
            }
            var list = articleService.LoadPageEntities((start + length) / length, length, out total, baseWhere, a => a.Creation_Time, false).Select(a => new
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

        [HttpPost]
        public JsonResult GetArticles()
        {
            int draw = Convert.ToInt32(Request["draw"] ?? "0");
            int start = Convert.ToInt32(Request["start"] ?? "1");
            int length = Convert.ToInt32(Request["length"] ?? "10");
            int total;
            var list = articleService.LoadPageEntities((start + length) / length, length, out total, a => a.State == (int)ArticleStateEnum.NotAudited, a => a.Creation_Time, false).Select(a => new
            {
                a.Article_Id,
                a.Title,
                a.Creation_Time,
                a.State,
                a.SystemCategory.Cate_Name,
                a.Author.Nickname
            });
            return Json(new { data = list, draw, recordsTotal = total, recordsFiltered = total });
        }


    }
}