using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected IUserService userService;
        protected IArticleService articleService;

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
            return null;
        }
    }
}