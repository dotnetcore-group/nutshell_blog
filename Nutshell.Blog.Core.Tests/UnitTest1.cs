using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using System.IO;
using Nutshell.Blog.Model;
using System.Linq;

namespace Nutshell.Blog.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLucene()
        {
            var path = @"F:\lucenedir";
            using (FSDirectory directory = FSDirectory.Open(new DirectoryInfo(path), new NativeFSLockFactory()))
            {
                var isExists = IndexReader.IndexExists(directory);
                if (isExists)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        IndexWriter.Unlock(directory);
                    }
                }
                using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExists, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    //writer.DeleteDocuments(new Term("Content", "检查"));
                    writer.DeleteAll();
                }
            }
        }

        [TestMethod]
        public void TestWriteIndex()
        {
            
            //using (Blog.Model.NutshellBlogContext db = new Blog.Model.NutshellBlogContext())
            //{
            //    var articles = db.Article.Where(a => true).ToList();
            //    foreach (var articel in articles)
            //    {
            //        PanGuLuceneHelper.Instance.AddQueue(articel.Article_Id.ToString(), articel.Title, articel.Content, artice, articel.Creation_Time);
            //    }
            //}
            //PanGuLuceneHelper.Instance.StartThread();
        }

        [TestMethod]
        public void TestDeleteIndex()
        {
            var res = PanGuLuceneHelper.Instance.DeleteAll();
            Assert.AreEqual(true, res);
        }
    }

}
