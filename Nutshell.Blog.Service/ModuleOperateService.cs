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
    public class ModuleOperateService : BaseService<ModuleOperate>, IModuleOperateService
    {
        IModuleOperateRepository moduleOperateRepository;
        public ModuleOperateService(IModuleOperateRepository moduleOperateRepository)
        {
            this.moduleOperateRepository = moduleOperateRepository;
            baseDal = moduleOperateRepository;
        }

        public List<ModuleOperate> GetModuleOperates(int module_id)
        {
            return moduleOperateRepository.LoadEntities(m => m.Module.Id == module_id).OrderBy(m => m.Sort)?.ToList();
        }
    }
}
