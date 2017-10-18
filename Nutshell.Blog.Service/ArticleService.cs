/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Service
 * 文件名：ArticleService
 * 版本号：V1.0.0.0
 * 唯一标识：463ff6cc-18a9-4716-933b-445d18662788
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 16:39:15
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 16:39:15
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.Core;
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Service
{
    public class ArticleService : BaseService<Article>, IArticleService
    {
        IArticleRepository currentRepository;
        public ArticleService(IArticleRepository repository)
        {
            currentRepository = repository;
            baseDal = repository;
        }

        public Article AddArticle(Article article)
        {
            var art = currentRepository.AddEntity(article);
            var res = currentRepository.SaveChanges();

            if (res)
            {
                LuceneIndexManager.GetInstance().AddQueue(art.Article_Id.ToString(), art.Title, art.Content, "作者", art.Creation_Time);
            }

            return art;
        }
    }
}
