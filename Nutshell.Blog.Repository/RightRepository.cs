using Nutshell.Blog.Model;
using Nutshell.Blog.IReposotory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nutshell.Blog.Model.ViewModel;
using System.Data.SqlClient;

namespace Nutshell.Blog.Repository
{
    public class RightRepository : BaseRepository<Right>, IRightRepository
    {
        public List<Permission> GetPermission(int userId, string controller)
        {
            string sql = "[P_Sys_GetRightOperate] @User_Id=@User_Id,@Controller=@Controller";
            SqlParameter[] sqlParameter = new SqlParameter[] {
                new SqlParameter("@User_Id",userId),
                new SqlParameter("@Controller", controller)
            };
            return ExecuteSelectQuery<Permission>(sql, sqlParameter);
        }
    }
}
