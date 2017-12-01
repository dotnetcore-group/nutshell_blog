using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IModuleService : IBaseService<Module>
    {
        List<Module> GetMenuByPersonId(long personId);

        List<Module> GetModuleList(long? parentId);
    }
}
