using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.MvcHelper;
using Nutshell.Blog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    [CheckUserLogin]
    public class FavoriteController : BaseController
    {
        public FavoriteController(IFavoritesService favoritesService, IArticleService articleService)
        {
            base.favoritesService = favoritesService;
            base.articleService = articleService;
        }

        public ActionResult Add(int articleId)
        {
            var article = articleService.LoadEntity(a => a.Article_Id == articleId);
            var favorite = favoritesService.LoadEntity(f => f.Article_Id == articleId && f.User_Id == Account.User_Id);
            if (favorite != null)
            {
                ViewBag.Added = true;
            }
            return View(article);
        }

        public ActionResult Edit(int articleId)
        {
            var favorite = favoritesService.LoadEntity(f => f.Article_Id == articleId && f.User_Id == Account.User_Id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }

        // 添加收藏
        [HttpPost]
        public JsonResult PostFavorite(Favorites favorite)
        {
            var json = new JsonModel { code = 0, msg = "收藏失败，刷新后重试！" };
            var account = GetCurrentAccount();
            var temp = favoritesService.LoadEntity(f => f.Article_Id == favorite.Article_Id && f.User_Id == account.User_Id);
            if (temp != null)
            {
                json.msg = "该文章已经收藏过了，不能重复收藏！";
                return Json(json);
            }
            if (ModelState.IsValid)
            {
                favorite.User_Id = account.User_Id;
                favoritesService.AddEntity(favorite);
                if (favoritesService.SaveChanges())
                {
                    json.msg = "收藏成功！";
                }
            }
            return Json(json);
        }
    }
}