using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nutshell.Blog.Model;
using System.Linq.Expressions;

namespace Nutshell.Blog.Mvc.Controllers
{
    [CheckUserLogin]
    public class UserController : BaseController
    {
        public UserController(IUserService userService, IArticleService articleService, IFavoritesService favoritesService, ICustomCategoryService customCategoryService)
        {
            base.favoritesService = favoritesService;
            base.userService = userService;
            base.articleService = articleService;
            base.customCategoryService = customCategoryService;
        }

        // 用户主页
        public ActionResult Me()
        {
            var account = GetCurrentAccount();
            var user = userService.LoadEntity(u => u.User_Id == account.User_Id);
            if (user != null)
            {
                return View(user);
            }
            return HttpNotFound();
        }

        public ActionResult Settings()
        {
            var user = userService.LoadEntity(u => u.User_Id == Account.User_Id);
            return View(user);
        }

        public ActionResult Favorite()
        {
            return View();
        }

        public ActionResult Blog()
        {
            var account = GetCurrentAccount();
            var categories = customCategoryService.LoadEntities(a => a.Author.User_Id == account.User_Id)?.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Category(string author, int CategoryId)
        {
            var category = customCategoryService.LoadEntity(c => c.Author.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase) && c.Id == CategoryId);
            if (category == null)
            {
                throw new HttpException(404, "Not Found!");
            }
            ViewBag.UserInfo = category.Author;
            // 文章档案
            ViewBag.Archives = articleService.GetArchivesByUserId<Archives>(category.Author.User_Id);
            // 文章分类
            ViewBag.Categories = articleService.GetCustomCategoriesByUserId<CustomCategories>(category.Author.User_Id);
            return View(category);
        }

        // 文章管理
        public ActionResult ArticleManage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteFavorite(int articleid)
        {
            var data = new { code = 1, msg = "删除失败，请重试！" };

            var favorite = favoritesService.LoadEntity(f => f.Article_Id == articleid && f.User_Id == Account.User_Id);
            if (favorite != null)
            {
                favoritesService.DeleleEntity(favorite);
                if (favoritesService.SaveChanges())
                {
                    data = new { code = 0, msg = "删除成功" };
                }
            }

            return Json(data);
        }

        // 文章列表
        [HttpPost]
        public JsonResult GetBlogs()
        {
            var data = new JsonModel() { code = 1, msg = "加载数据失败，请重试！" };
            var account = GetCurrentAccount();

            var state = Convert.ToInt32(Request["state"] ?? "0");
            var categoryid = Convert.ToInt32(Request["categoryid"] ?? "0");
            var pageIndex = Convert.ToInt32(Request["pageindex"] ?? "1");
            var pageSize = Convert.ToInt32(Request["pagesize"] ?? "20");

            //var temp = articleService.LoadEntities(a => a.Author_Id == account.User_Id && a.State != (int)ArticleStateEnum.Deleted && a.State != (int)ArticleStateEnum    .Draft);

            Expression<Func<Article, bool>> baseWhere = a => a.Author_Id == account.User_Id && a.State != (int)ArticleStateEnum.Deleted && a.State != (int)ArticleStateEnum.Draft;

            if (state != 0)
            {
                //temp = temp.Where(a => a.State == state);
                Expression<Func<Article, bool>> expression = a => a.State == state;

                baseWhere = LinqExtensions.And(baseWhere, expression);
            }
            if (categoryid != 0)
            {
                //temp = temp.Where(a => a.CustomCategory_Id == categoryid);
                Expression<Func<Article, bool>> expression = a => a.CustomCategory_Id == categoryid;

                baseWhere = LinqExtensions.And(baseWhere, expression);

            }

            var list = articleService.LoadPageEntities(baseWhere, pageIndex, pageSize).Select(a => new
            {
                a.Article_Id,
                Author = a.Author.Login_Name,
                a.Title,
                a.Creation_Time,
                a.State,
                a.IsTop
            });
            var count = articleService.LoadEntities(baseWhere).Count();
            if (list != null)
            {
                data.code = 0;
                data.msg = "";
                data.res = list;
                data.total = count;
                data.index = pageIndex;
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetDrafts()
        {
            var data = new JsonModel() { code = 1, msg = "获取数据失败，请重试！" };
            var account = GetCurrentAccount();

            data.res = articleService.LoadEntities(a => a.Author_Id == account.User_Id && a.State == (int)ArticleStateEnum.Draft).Select(a => new
            {
                a.Article_Id,
                a.Title,
                a.Creation_Time
            });

            if (data.res != null)
            {
                data.code = 0;
                data.msg = "获取数据成功";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetDeleted()
        {
            var data = new JsonModel() { code = 0, msg = "获取数据失败，请重试！" };
            data.res = articleService.LoadEntities(a => a.Author_Id == Account.User_Id && a.State == (int)ArticleStateEnum.Deleted).Select(a => new
            {
                a.Article_Id,
                a.Title,
                a.Creation_Time
            });
            if (data.res != null)
            {
                data.code = 0;
                data.msg = "获取数据成功";
            }
            return Json(data);
        }

        // 删除文章
        [HttpPost]
        public JsonResult DeleteBlog(int id)
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

        // 文章置顶
        [HttpPost]
        public JsonResult Top(int id)
        {
            var data = new JsonModel() { code = 1, msg = "设置置顶失败，请重试！" };
            var account = GetCurrentAccount();
            var article = articleService.LoadEntity(a => a.Article_Id == id && a.Author_Id == account.User_Id);
            var count = articleService.LoadEntities(a => a.IsTop && a.Author_Id == account.User_Id).Count();
            if (article != null && count < 20)
            {
                if (!article.IsTop && article.State == (int)ArticleStateEnum.Published)
                {
                    article.IsTop = true;
                    articleService.EditEntity(article);
                    if (articleService.SaveChanges())
                    {
                        data.code = 0;
                        data.msg = "置顶成功！";
                    }
                }
                else
                {
                    data.msg = "已经是置顶或者该文章当前状态不能设置置顶！";
                }
            }
            else
            {
                data.msg = "文章不存在或者置顶文章数量已经到了最大值（最多20篇）";
            }
            return Json(data);
        }

        // 取消置顶
        [HttpPost]
        public JsonResult UnTop(int id)
        {
            var data = new JsonModel() { code = 1, msg = "取消置顶失败，请重试！" };
            var account = GetCurrentAccount();
            var article = articleService.LoadEntity(a => a.Article_Id == id && a.Author_Id == account.User_Id);
            if (article != null)
            {
                if (article.IsTop)
                {
                    article.IsTop = false;
                    articleService.EditEntity(article);
                    if (articleService.SaveChanges())
                    {
                        data.code = 0;
                        data.msg = "取消置顶成功！";
                    }
                }
                else
                {
                    data.msg = "该文章已经不是置顶了，不要重复操作！";
                }
            }
            else
            {
                data.msg = "该文章不存在！";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetFavorites()
        {
            var account = GetCurrentAccount();
            var list = favoritesService.LoadEntities(f => f.User_Id == account.User_Id).OrderByDescending(f => f.Collection_Time).Select(f => new
            {
                f.Article_Id,
                author = f.Article.Author.Login_Name,
                f.Remark,
                f.Title,
                f.Collection_Time
            });
            if (list == null)
            {
                return Json(new JsonModel { code = 1 });
            }
            return Json(new JsonModel { code = 0, res = list, total = list.Count() });
        }

        [HttpPost]
        public JsonResult SetPhoto(int x, int y, int w, int h, string imgsrc)
        {
            var data = new { code = 1, msg = "保存头像失败，请重试！", url = "" };
            var url = ImageHelper.CutAvatar(imgsrc, x, y, w, h);
            if (!string.IsNullOrEmpty(url))
            {
                var user = userService.LoadEntity(u => u.User_Id == Account.User_Id);
                if (user != null)
                {
                    user.Photo = url;
                    userService.EditEntity(user);
                    if (userService.SaveChanges())
                    {
                        data = new { code = 0, msg = "保存成功！", url = url };
                    }
                }
            }
            return Json(data);
        }

        [AllowAnonymous]
        public ActionResult LoginOut()
        {
            var cookie = Request.Cookies[Keys.SessionId];
            var sessionid = cookie?.Value;
            if (sessionid != null)
            {
                MemcacheHelper.Delete(sessionid);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return Redirect("/");
        }
    }
}