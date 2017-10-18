/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Core
 * 文件名：LuceneIndexManager
 * 版本号：V1.0.0.0
 * 唯一标识：5396b0d8-3b76-46eb-acf3-ee2e65b3b5fe
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 16:01:12
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 16:01:12
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Nutshell.Blog.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public class LuceneIndexManager
    {
        readonly static LuceneIndexManager indexManager = new LuceneIndexManager();
        public static LuceneIndexManager GetInstance()
        {
            return indexManager;
        }
        readonly Queue<IndexContent> queue = new Queue<IndexContent>();

        /// <summary>
        /// 添加数据到队列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="author"></param>
        /// <param name="creation_time"></param>
        public void AddQueue(string id, string title, string content, string author, DateTime? creation_time)
        {
            IndexContent indexContent = new IndexContent
            {
                Id = id,
                Title = title,
                Content = content,
                Author = author,
                Creation_Time = creation_time,
                LuceneType = LuceneType.Add
            };

            queue.Enqueue(indexContent);
        }

        public void DeleteQuene(string id)
        {
            queue.Enqueue(new IndexContent
            {
                Id = id,
                LuceneType = LuceneType.Delete
            });
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
                    CreateIndexContent();
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }

        void CreateIndexContent()
        {
            using (FSDirectory directory = FSDirectory.Open(new DirectoryInfo(LuceneNetHelper.IndexPath), new NativeFSLockFactory()))
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
                    while (queue.Count > 0)
                    {
                        var indexContent = queue.Dequeue();
                        writer.DeleteDocuments(new Term("Id", indexContent.Id));
                        if (indexContent.LuceneType == LuceneType.Delete)
                        {
                            continue;
                        }
                        var document = new Document();
                        document.Add(new Field("Id", indexContent.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Title", indexContent.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("Content", indexContent.Content, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("Authot", indexContent.Author, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("CreationTime", indexContent.Creation_Time?.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                        writer.AddDocument(document);
                    }
                }
            }
        }
    }
}
