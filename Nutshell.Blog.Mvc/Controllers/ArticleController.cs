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
    [AllowAnonymous]
    public class ArticleController : BaseController
    {
        int showWords = 800;
        public ArticleController(IArticleService artService)
        {
            articleService = artService;
        }

        [HttpPost]
        public JsonResult Search(string words)
        {
            List<SearchArticleResult> list = null;
            list = SearchArticles(words);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            var words = Request["words"];
            if (words != null)
            {
                return Search(words);
            }
            return View();
        }

        List<SearchArticleResult> SearchArticles(string keywords)
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(LuceneNetHelper.IndexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            PhraseQuery query = new PhraseQuery();//查询条件
            PhraseQuery titleQuery = new PhraseQuery();//标题查询条件
            List<string> lstkw = LuceneNetHelper.PanGuSplitWord(keywords);//对用户输入的搜索条件进行拆分。
            foreach (string word in lstkw)//先用空格，让用户去分词，空格分隔的就是词“xx ”
            {
                query.Add(new Term("Content", word));//contains("Content",word)
                titleQuery.Add(new Term("Title", word));
            }
            query.Slop = 100;//两个词的距离大于100（经验值）就不放入搜索结果，因为距离太远相关度就不高了

            BooleanQuery bq = new BooleanQuery();
            //Occur.Should 表示 Or , Must 表示 and 运算
            bq.Add(query, Occur.SHOULD);
            bq.Add(titleQuery, Occur.SHOULD);


            TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);//盛放查询结果的容器
            searcher.Search(bq, null, collector);//使用query这个查询条件进行搜索，搜索结果放入collector
            //collector.GetTotalHits()总的结果条数

            int recCount = collector.TotalHits;//总的结果条数

            ScoreDoc[] docs = collector.TopDocs(0, recCount).ScoreDocs;//从查询结果中取出第m条到第n条的数据

            List<SearchArticleResult> list = new List<SearchArticleResult>();
            string content = string.Empty;
            string title = string.Empty;

            SearchArticleResult result = new SearchArticleResult();
            for (int i = 0; i < docs.Length; i++)//遍历查询结果
            {
                int docId = docs[i].Doc;//拿到文档的id，因为Document可能非常占内存（思考DataSet和DataReader的区别）
                //所以查询结果中只有id，具体内容需要二次查询
                Document doc = searcher.Doc(docId);//根据id查询内容。放进去的是Document，查出来的还是Document

                result.Id = Convert.ToInt32(doc.Get("Id"));
                content = doc.Get("Content");//只有 Field.Store.YES的字段才能用Get查出来
                content = content.Length > showWords ? content.Substring(0, showWords) + "......" : content;
                result.Content = LuceneNetHelper.CreateHightLight(keywords, content);//将搜索的关键字高亮显示。
                title = doc.Get("Title");
                foreach (string word in lstkw)
                {
                    title = title.Replace(word, "<span style='color:red;'>" + word + "</span>");
                }
                result.Title = title;
                result.Creation_Time = Convert.ToDateTime(doc.Get("CreationTime"));
                list.Add(result);
            }
            return list;
        }

        [CheckUserLogin]
        public ActionResult Post()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CheckUserLogin]
        public JsonResult PostArticle(Article article)
        {
            var art = "";
            //if (ModelState.IsValid)
            //{
            article.Author_Id = GetCurrentAccount()?.User_Id;
            article.Content = Server.HtmlEncode(article.Content);
            art = articleService.AddArticle(article) == null ? "no" : "ok";
            //}
            return Json(new { art });
        }

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
    }
}