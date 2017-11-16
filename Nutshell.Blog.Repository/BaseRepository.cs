/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Repository
 * 文件名：BaseRepository
 * 版本号：V1.0.0.0
 * 唯一标识：f9c60af4-e501-456a-96b4-7fe783e04733
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 13:00:56
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 13:00:56
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        protected DbContext db
        {
            get
            {
                var context = CallContext.GetData("DbContext") as DbContext;
                if (context == null)
                {
                    context = new NutshellBlogContext();
                    CallContext.SetData("DbContext", context);
                }
                return context;
            }
        }

        readonly DbSet<T> _dbSet;

        public BaseRepository()
        {
            _dbSet = db.Set<T>();
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return _dbSet.Where(whereLambda);
        }

        public T LoadEntity(Expression<Func<T, bool>> whereLambda)
        {
            return _dbSet.FirstOrDefault(whereLambda) ?? null;
        }

        public IQueryable<T> LoadPageEntities<TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> orderbyLambda, bool isAsc = true)
        {
            var temp = _dbSet.Where(whereLambda);
            int skipCount = (pageIndex - 1) * pageSize;
            totalCount = temp.Count();
            return isAsc ? temp.OrderBy(orderbyLambda).Skip(skipCount).Take(pageSize) : temp.OrderByDescending(orderbyLambda).Skip(skipCount).Take(pageSize);
        }

        public T AddEntity(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public bool DeleleEntity(T entity)
        {
            db.Entry(entity).State = EntityState.Deleted;
            return true;
        }

        public bool EditEntity(T entity)
        {
            _dbSet.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public bool SaveChanges()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            return db.SaveChanges() > 0;
        }

        public List<TResult> ExecuteSelectQuery<TResult>(string sql, params SqlParameter[] param)
        {
            return db.Database.SqlQuery<TResult>(sql, param).ToList();
        }

        public int ExecuteSql(string sql, params SqlParameter[] param)
        {
            return param == null ? db.Database.ExecuteSqlCommand(sql) : db.Database.ExecuteSqlCommand(sql, param);
        }
    }
}
