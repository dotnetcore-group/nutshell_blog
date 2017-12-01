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
    public class ModuleService : BaseService<Module>, IModuleService
    {
        IModuleRepository moduleRepository;
        public ModuleService(IModuleRepository moduleRepository)
        {
            this.moduleRepository = moduleRepository;
        }

        public List<Module> GetMenuByPersonId(long personId)
        {
            return moduleRepository.GetMenuByPersonId(personId);
        }

        public List<Module> GetModuleList(long? parentId)
        {
            if (!parentId.HasValue)
            {
                parentId = 0;
            }
            return moduleRepository.LoadEntities(m => m.Parent_Id==parentId).OrderBy(m=>m.Sort)?.ToList() ;
        }
    }
}
