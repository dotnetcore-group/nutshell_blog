using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class ArticleController : BaseController
    {
        int showWords = 800;
        public ArticleController(IArticleService artService)
        {
            articleService = artService;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Search(string words)
        {
            List<SearchArticleResult> list = null;
            //var wordList = PanGuLuceneHelper.Instance.PanGuSplitWord(words);
            list = PanGuLuceneHelper.Instance.Search(words);
            return Json(new { articles = list }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Search()
        {
            var words = Request["words"];
            if (words != null)
            {
                return Search(words);
            }
            return View();
        }

        [CheckUserLogin]
        public ActionResult Post()
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
            article.Author_Id = GetCurrentAccount()?.User_Id;
            article.Body = Server.HtmlEncode(article.Body);
            article.Content = article.Content.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Replace(" ", "");
            article.Introduction = article.Content.Length > 200 ? article.Content.Substring(0, 190) + "..." : article.Content;
            art = articleService.AddArticle(article) == null ? "no" : "ok";
            //}
            return Json(new { art });
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
            return View(article);
        }

        [CheckUserLogin]
        public JsonResult Comment(string content)
        {
            return Json(content);
        }
    }
}