using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using System.IO;

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
    }

}
