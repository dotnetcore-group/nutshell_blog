using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IBaseService<T> where T : class, new()
    {
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

        T LoadEntity(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> LoadPageEntities<TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> orderbyLambda, bool isAsc = true);

        T AddEntity(T entity);

        bool DeleleEntity(T entity);

        bool EditEntity(T entity);

        bool SaveChanges();

        List<TResult> ExecuteSelectQuery<TResult>(string sql, params SqlParameter[] param);

        int ExecuteSql(string sql, params SqlParameter[] param);
    }
}
