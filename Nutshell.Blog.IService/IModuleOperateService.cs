using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IModuleOperateService : IBaseService<ModuleOperate>
    {
        List<ModuleOperate> GetModuleOperates(int module_id);
    }
}
