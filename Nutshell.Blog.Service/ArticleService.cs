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
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
                //LuceneIndexManager.GetInstance().AddQueue(art.Article_Id.ToString(), art.Title, art.Content, "作者", art.Creation_Time);
                PanGuLuceneHelper.Instance.AddQueue(art.Article_Id.ToString(), art.Title, art.Content, art.Author.Nickname, art.Author.Login_Name, art.Creation_Time);
            }

            return art;
        }

        public List<Archives> GetArchivesByUserId<Archives>(long UserId)
        {
            return currentRepository.ExecuteSelectQuery<Archives>("[dbo].[PROC_GET_ARCHIVES] @User_Id=@User_Id", new System.Data.SqlClient.SqlParameter("@User_Id", UserId));
        }

        public int GetArticlesTotalCount(long UserId)
        {
            string sql = "SELECT COUNT(*) AS [Count] FROM [dbo].[article] WHERE Author_Id=@Author_Id AND State=3";
            SqlParameter parameter = new SqlParameter("@Author_Id", UserId);
            return currentRepository.ExecuteSelectQuery<int>(sql, parameter).FirstOrDefault();
        }

        public List<CustomCategories> GetCustomCategoriesByUserId<CustomCategories>(long UserId)
        {
            return currentRepository.ExecuteSelectQuery<CustomCategories>("PROC_GET_CustomCategories @User_Id=@User_Id", new SqlParameter("@User_Id", UserId));
        }

        public List<Article> LoadPageEntities(Expression<Func<Article,bool>> where, int pageIndex, int pageSize)
        {
            var articles = currentRepository.LoadEntities(where).OrderByDescending(a => a.Creation_Time).OrderByDescending(a => a.IsTop).Skip((pageIndex - 1) * pageSize).Take(pageSize)?.ToList();
            return articles;
        }
        
    }
}
