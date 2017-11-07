using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected IUserService userService;
        protected IArticleService articleService;
        protected IModuleService moduleService;
        protected IModuleOperateService moduleOperateService;
        protected ICustomCategoryService customCategoryService;
        protected IRoleService roleService;
        protected ICategoryService categoryService;
        protected IFavoritesService favoritesService;

        protected int PageSize
        {
            get
            {
                var pageSize = 0;
                var setting = ConfigurationManager.AppSettings["pageSize"];
                if (!int.TryParse(setting, out pageSize))
                {
                    pageSize = 20;
                }
                return pageSize;
            }
        }

        /// <summary>
        /// 获取当前登陆人的账户信息
        /// </summary>
        /// <returns>账户信息</returns>
        protected Account GetCurrentAccount()
        {
            if (Request.Cookies[Keys.SessionId] != null)
            {
                string sessionId = Request.Cookies[Keys.SessionId].Value;//接收从Cookie中传递过来的Memcache的key
                if (sessionId != null)
                {
                    object obj = MemcacheHelper.Get(sessionId);//根据key从Memcache中获取用户的信息
                    return obj == null ? null : SerializerHelper.DeserializeToObject<Account>(obj.ToString());
                }
            }
            Redirect("/account/signin");
            return null;
        }

        protected List<Permission> GetPermission()
        {
            string filePath = HttpContext.Request.FilePath;
            var account = GetCurrentAccount();
            List<Permission> perm = (List<Permission>)Session[filePath+ account.User_Id];
            return perm;
        }
    }
}