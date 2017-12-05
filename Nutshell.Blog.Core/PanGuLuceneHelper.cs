/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Core
 * 文件名：PanGuLuceneHelper
 * 版本号：V1.0.0.0
 * 唯一标识：2529f4dd-761a-498e-96f4-f797614e78ba
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-21 14:00:51
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-21 14:00:51
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Nutshell.Blog.Core.Model;
using Nutshell.Blog.ViewModel.Article;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public class PanGuLuceneHelper
    {

        readonly ConcurrentQueue<IndexContent> queue = new ConcurrentQueue<IndexContent>();
        //static object myLock = new object();

        private PanGuLuceneHelper() { }

        private static PanGuLuceneHelper _instance = null;

        public static PanGuLuceneHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PanGuLuceneHelper();
                }
                return _instance;
            }
        }

        #region analyzer  
        private Analyzer _analyzer = null;
        /// <summary>  
        /// 分析器  
        /// </summary>  
        public Analyzer analyzer
        {
            get
            {
                if (_analyzer == null)
                {
                    _analyzer = new PanGuAnalyzer();//盘古分词分析器  
                    //_analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);//标准分析器  
                }
                return _analyzer;
            }
        }
        #endregion

        #region directory  
        private System.IO.DirectoryInfo _directory = null;
        /// <summary>  
        /// 索引在硬盘上的目录  
        /// </summary>  
        public System.IO.DirectoryInfo directory
        {
            get
            {
                if (_directory == null)
                {
                    string dirPath = ConfigurationManager.AppSettings["lucenedir"];
                    if (System.IO.Directory.Exists(dirPath) == false) _directory = System.IO.Directory.CreateDirectory(dirPath);
                    else _directory = new DirectoryInfo(dirPath);
                }
                return _directory;
            }
        }
        #endregion

        #region lucene.net directory  
        private Lucene.Net.Store.Directory _directory_luce = null;
        /// <summary>  
        /// Lucene.Net的目录-参数  
        /// </summary>  
        public Lucene.Net.Store.Directory directory_luce
        {
            get
            {
                if (_directory_luce == null) _directory_luce = FSDirectory.Open(directory, new NativeFSLockFactory());
                return _directory_luce;
            }
        }
        #endregion

        #region version  
        private static Lucene.Net.Util.Version _version = Lucene.Net.Util.Version.LUCENE_30;
        /// <summary>  
        /// 版本号枚举类  
        /// </summary>  
        public Lucene.Net.Util.Version version
        {
            get
            {
                return _version;
            }
        }
        #endregion

        /// <summary>
        /// 添加数据到队列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="author"></param>
        /// <param name="creation_time"></param>
        public void AddQueue(string id, string title, string content, string author_nick, string author_name, DateTime? creation_time)
        {
            IndexContent indexContent = new IndexContent
            {
                Id = id,
                Title = title,
                Content = content,
                Author_Nick = author_nick,
                Author_Name = author_name,
                Creation_Time = creation_time
            };
            queue.Enqueue(indexContent);
        }

        /// <summary>
        /// 开启线程，扫描队列
        /// </summary>
        public void StartThread()
        {
            Thread thread = new Thread(WriteIndexContent);
            thread.IsBackground = true;
            thread.Start();
        }

        void WriteIndexContent()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    IndexContent indexContent = null;
                    queue.TryDequeue(out indexContent);
                    if (indexContent != null)
                    {
                        CreateIndex(indexContent);
                    }
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// 盘古分词
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IList<string> PanGuSplitWord(string keyword)
        {
            var resList = new List<string>();
            using (StringReader reader = new StringReader(keyword))
            {
                using (TokenStream tokenStream = analyzer.TokenStream(keyword, reader))
                {
                    var hasNext = tokenStream.IncrementToken();
                    ITermAttribute termAttibute;
                    while (hasNext)
                    {
                        termAttibute = tokenStream.GetAttribute<ITermAttribute>();
                        resList.Add(termAttibute.Term);
                        hasNext = tokenStream.IncrementToken();
                    }
                }
            }
            analyzer.Close();
            return resList;
        }

        #region 创建索引
        bool CreateIndex(IndexContent indexContent)
        {
            var isExists = IndexReader.IndexExists(directory_luce);
            if (isExists)
            {
                if (IndexWriter.IsLocked(directory_luce))
                {
                    IndexWriter.Unlock(directory_luce);
                }
            }
            IndexWriter writer = null;
            try
            {
                //false表示追加（true表示删除之前的重新写入）
                writer = new IndexWriter(directory_luce, analyzer, !isExists, IndexWriter.MaxFieldLength.LIMITED);
                if (indexContent == null)
                {
                    return false;
                }
                Document document = new Document();

                document.Add(new Field("Id", indexContent.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("Title", indexContent.Title, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Content", indexContent.Content, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("CreationTime", indexContent.Creation_Time?.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("Author_Nick", indexContent.Author_Nick, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("Author_Name", indexContent.Author_Name, Field.Store.YES, Field.Index.NOT_ANALYZED));

                writer.AddDocument(document);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Optimize();
                    writer.Dispose();
                }
            }
            return true;
        }
        #endregion

        public string CreateHightLight(string keywords, string content)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<font color='#dd4b39'>", "</font>");
            PanGu.HighLight.Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new PanGu.Segment());
            highlighter.FragmentSize = 150;
            return highlighter.GetBestFragment(keywords, content);
        }

        public List<SearchArticleResult> Search(string keyword)
        {
            string[] fieds = { "Title", "Content" };
            QueryParser queryParser = null;
            queryParser = new MultiFieldQueryParser(version, fieds, analyzer);
            Query query = queryParser.Parse(keyword);

            IndexSearcher searcher = new IndexSearcher(directory_luce, true);
            TopDocs docs = searcher.Search(query, null, 100);
            if (docs == null || docs.TotalHits <= 0)
            {
                return null;
            }
            List<SearchArticleResult> articles = new List<SearchArticleResult>();

            var scoreDocs = docs.ScoreDocs;
            try
            {
                foreach (var scoreDoc in scoreDocs)
                {
                    var doc = searcher.Doc(scoreDoc.Doc);
                    var title = CreateHightLight(keyword, doc.Get("Title"));
                    var content = CreateHightLight(keyword, doc.Get("Content"));
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        title = doc.Get("Title");
                    }
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        var tem = doc.Get("Content");
                        content = tem.Length >= 190 ? tem.Substring(0, 190) : tem;
                    }
                    articles.Add(new SearchArticleResult
                    {
                        Id = doc.Get("Id"),
                        Creation_Time = doc.Get("CreationTime"),
                        Title = title,
                        Content = content,
                        Author_Nick = doc.Get("Author_Nick"),
                        Author_Name = doc.Get("Author_Name")
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return articles;
        }

        public List<SearchArticleResult> Search(string keyword, int PageIndex, int PageSize, out int TotalCount)
        {
            string[] fieds = { "Title", "Content" };
            QueryParser queryParser = null;
            queryParser = new MultiFieldQueryParser(version, fieds, analyzer);
            Query query = queryParser.Parse(keyword);

            TopScoreDocCollector collector = TopScoreDocCollector.Create(PageIndex * PageSize, false);
            IndexSearcher searcher = new IndexSearcher(directory_luce, true);
            searcher.Search(query, collector);
            if (collector == null || collector.TotalHits == 0)
            {
                TotalCount = 0;
                return null;
            }
            int start = PageSize * (PageIndex - 1);
            //结束数  
            int limit = PageSize;
            List<SearchArticleResult> articles = new List<SearchArticleResult>();
            ScoreDoc[] hits = collector.TopDocs(start, limit).ScoreDocs;
            TotalCount = collector.TotalHits;
            //var scoreDocs = docs.ScoreDocs;
            try
            {
                foreach (var scoreDoc in hits)
                {
                    var doc = searcher.Doc(scoreDoc.Doc);
                    var title = CreateHightLight(keyword, doc.Get("Title"));
                    var content = CreateHightLight(keyword, doc.Get("Content"));
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        title = doc.Get("Title");
                    }
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        var tem = doc.Get("Content");
                        content = tem.Length>=190? tem.Substring(0,190): tem;
                    }
                    articles.Add(new SearchArticleResult
                    {
                        Id = doc.Get("Id"),
                        Creation_Time = doc.Get("CreationTime"),
                        Title = title,
                        Content = content,
                        Author_Nick = doc.Get("Author_Nick"),
                        Author_Name = doc.Get("Author_Name")
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return articles;
        }

        #region 删除索引数据（根据id）  
        /// <summary>  
        /// 删除索引数据（根据id）  
        /// </summary>  
        /// <param name="id"></param>  
        /// <returns></returns>  
        public bool Delete(string id)
        {
            bool IsSuccess = false;
            Term term = new Term("Id", id);
            IndexWriter writer = new IndexWriter(directory_luce, analyzer, false, IndexWriter.MaxFieldLength.LIMITED);
            writer.DeleteDocuments(term);
            writer.Commit();
            IsSuccess = writer.HasDeletions();
            writer.Dispose();
            return IsSuccess;
        }
        #endregion

        #region 删除全部索引数据  
        /// <summary>  
        /// 删除全部索引数据  
        /// </summary>  
        /// <returns></returns>  
        public bool DeleteAll()
        {
            bool IsSuccess = true;

            var isExists = IndexReader.IndexExists(directory_luce);
            if (isExists)
            {
                if (IndexWriter.IsLocked(directory_luce))
                {
                    IndexWriter.Unlock(directory_luce);
                }
            }
            using (IndexWriter writer = new IndexWriter(directory_luce, new PanGuAnalyzer(), !isExists, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                //writer.DeleteDocuments(new Term("Content", "检查"));
                writer.DeleteAll();
            }

            return IsSuccess;
        }
        #endregion
    }
}