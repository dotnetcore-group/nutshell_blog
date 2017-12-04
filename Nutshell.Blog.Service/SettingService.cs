using Nutshell.Blog.Common;
using Nutshell.Blog.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nutshell.Blog.Model;
using System.Linq.Expressions;
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.Core;

namespace Nutshell.Blog.Service
{
    public class SettingService : BaseService<Settings>, ISettingService
    {
        private ISettingRepository settingRepository;
        public SettingService(ISettingRepository settingRepository)
        {
            this.settingRepository = settingRepository;
            baseDal = settingRepository;
        }

        public string GetStringValue(string key)
        {
            var value = MemcacheHelper.Get(key)?.ToString();
            if (value == null)
            {
                value = settingRepository.LoadEntity(s => s.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))?.Value;
                MemcacheHelper.Set(key, value, DateTime.Now.AddHours(1));
            }
            return value;
        }

        public bool GetBooleanValue(string key, bool defValue)
        {
            var vaule = GetStringValue(key);
            if (string.IsNullOrEmpty(vaule))
            {
                return defValue;
            }
            return Convert.ToBoolean(vaule);
        }

        public int GetInt32Value(string key)
        {
            var vaule = GetStringValue(key);
            if (string.IsNullOrEmpty(vaule))
            {
                return 0;
            }
            return Convert.ToInt32(vaule);
        }
    }
}
