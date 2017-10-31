using Nutshell.Blog.IReposotory;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Repository
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public List<Module> GetMenuByPersonId(int personId)
        {
            string sql = "PROC_GET_UserMenus @User_Id=@User_Id";
            var sqlParameter = new SqlParameter("@User_Id", personId);
            return ExecuteSelectQuery<Module>(sql, sqlParameter);
        }
    }
}
