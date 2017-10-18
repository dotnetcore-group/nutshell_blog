using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Filters
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class CheckUserLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            // 判断是否跳过登录验证
            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
            {
                return;
            }
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
            {
                return;
            }

            var sessionid = request.Cookies[Keys.SessionId]?.Value;
            if (sessionid != null)
            {
                var obj = MemcacheHelper.Get(sessionid);
                if (obj != null)
                {
                    var account = SerializerHelper.DeserializeToObject<Account>(obj.ToString());
                    if (account != null)
                    {
                        return;
                    }
                }
            }

            filterContext.Result = new RedirectResult("/account/signin");
        }
    }
}