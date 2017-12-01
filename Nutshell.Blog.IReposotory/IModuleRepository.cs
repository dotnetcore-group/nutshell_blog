using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IReposotory
{
    public interface IModuleRepository : IBaseRepository<Module>
    {
        List<Module> GetMenuByPersonId(long personId);

        //IQueryable<Module> GetModuleList();
    }
}
