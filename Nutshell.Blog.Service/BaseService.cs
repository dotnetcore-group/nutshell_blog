/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Service
 * 文件名：BaseService
 * 版本号：V1.0.0.0
 * 唯一标识：295ebd28-759e-4320-a08d-aef09bbfc4e7
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 13:22:42
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 13:22:42
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.IService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Service
{
    public class BaseService<T> :IBaseService<T> where T : class, new()
    {
        protected IBaseRepository<T> baseDal;

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return baseDal.LoadEntities(whereLambda);
        }

        public T LoadEntity(Expression<Func<T, bool>> whereLambda)
        {
            return baseDal.LoadEntity(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> orderbyLambda, bool isAsc = true)
        {
            return baseDal.LoadPageEntities(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda, isAsc);
        }

        public T AddEntity(T entity)
        {
            return baseDal.AddEntity(entity);
        }

        public bool DeleleEntity(T entity)
        {
            return baseDal.DeleleEntity(entity);
        }

        public bool EditEntity(T entity)
        {
            return baseDal.EditEntity(entity);
        }

        public bool SaveChanges()
        {
            return baseDal.SaveChanges();
        }

        public List<TResult> ExecuteSelectQuery<TResult>(string sql, params SqlParameter[] param)
        {
            return baseDal.ExecuteSelectQuery<TResult>(sql, param);
        }

        public int ExecuteSql(string sql, params SqlParameter[] param)
        {
            return baseDal.ExecuteSql(sql, param);
        }
    }
}
