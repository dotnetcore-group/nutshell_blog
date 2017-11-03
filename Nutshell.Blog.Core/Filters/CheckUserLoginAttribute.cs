using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Core.Filters
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class CheckUserLoginAttribute : ActionFilterAttribute
    {
        protected Account Account = null;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
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
            var cookie = request.Cookies[Keys.SessionId];
            if (cookie != null)
            {
                var sessionid = request.Cookies[Keys.SessionId]?.Value;
                if (sessionid != null)
                {
                    var obj = MemcacheHelper.Get(sessionid);
                    if (obj != null)
                    {
                        var account = SerializerHelper.DeserializeToObject<Account>(obj.ToString());
                        if (account != null)
                        {
                            // 滑动过期时间
                            cookie.Expires = DateTime.Now.AddMinutes(20);
                            response.Cookies.Add(cookie);
                            MemcacheHelper.Set(sessionid, obj, DateTime.Now.AddMinutes(20));
                            Account = account;
                            return;
                        }
                    }
                }
            }

            response.StatusCode = 401;
            if (request.IsAjaxRequest())
            {
                filterContext.Result = new AjaxUnauthorizedResult();
                return;
            }
            filterContext.Result = new RedirectResult("/account/signin");
            base.OnActionExecuting(filterContext);
        }
    }
    class AjaxUnauthorizedResult : JsonResult
    {
        public AjaxUnauthorizedResult()
        {
            Data = new { code = 1, msg = "未登录或者登录状态失效，请重新登录！" };
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
    }
}