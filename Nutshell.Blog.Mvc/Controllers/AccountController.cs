using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUserService userService)
        {
            base.userService = userService;
        }

        // GET: Account
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignIn(UserLogin user)
        {
            string msg = "";
            var res = false;
            if (ModelState.IsValid)
            {
                var userinfo = userService.ValidationUser(user.UserName, user.Password, out msg);
                // 验证通过
                if (userinfo != null)
                {
                    // 将用户信息存入memcache
                    // 返回客户端一个session id
                    ValidatedUser(userinfo.ToAccount());
                    res = true;
                }
            }
            return Json(new { res, msg });
        }

        void ValidatedUser(Account account)
        {
            try
            {
                var sessionid = Guid.NewGuid().ToString();
                MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(account), DateTime.Now.AddMinutes(20));
                Response.Cookies[Keys.SessionId].Value = sessionid;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(UserRegister user)
        {
            var res = false;
            if (ModelState.IsValid)
            {
                var userInfo = userService.AddEntity(new Model.User
                {
                    Login_Name = user.UserName,
                    Login_Password = user.Password.Md5_Base64(),
                    Nickname = user.Nickname
                });
                res = userService.SaveChanges();
            }
            return Json(new { res, msg = res ? "注册成功！" : "注册失败！" });
        }

        [CheckUserLogin]
        public ActionResult Home(string author)
        {
            var userInfo = userService.LoadEntity(u => u.Login_Name.Equals(author, StringComparison.CurrentCultureIgnoreCase));
            if (userInfo != null)
            {
                ViewBag.User = author;
                ViewBag.Theme = userInfo.Theme.Resources;
                return View(userInfo);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public JsonResult NotExitesUserName()
        {
            string UserName = Request.Params["UserName"];
            if (string.IsNullOrEmpty(UserName))
            {
                return Json(false);
            }
            var user = userService.LoadEntity(u => u.Login_Name.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
            return user == null ? Json(true) : Json(false);
        }
        
        public ActionResult LoginOut()
        {
            var cookie = Request.Cookies[Keys.SessionId];
            var sessionid = cookie?.Value;
            if (sessionid != null)
            {
                MemcacheHelper.Delete(sessionid);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return Redirect("/");
        }
    }
}