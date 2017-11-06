using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Core.Filters;
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
            dynamic data = null;
            if (ModelState.IsValid)
            {
                var userinfo = userService.ValidationUser(user.UserName, user.Password, out msg);
                data = new { code = 1, msg = msg, url = "" };
                // 验证通过
                if (userinfo != null)
                {
                    // 将用户信息存入memcache
                    // 返回客户端一个session id
                    ValidatedUser(userinfo.ToAccount());
                    data = new { code = 0, msg = msg, url = "/admin/home/index" , userId = userinfo.User_Id};
                }
            }
            return Json(data);
        }

        void ValidatedUser(Account account)
        {
            try
            {
                var sessionid = Guid.NewGuid().ToString();
                MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(account), DateTime.Now.AddMinutes(20));
                Response.Cookies[Keys.SessionId].Value = sessionid;
                Response.Cookies[Keys.UserId].Value = account.User_Id.ToString();
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
                // 注册成功
                if (res)
                {
                    var sessionid = Guid.NewGuid().ToString();
                    MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(userInfo.ToAccount()), DateTime.Now.AddMinutes(20));
                    Response.Cookies[Keys.SessionId].Value = sessionid;
                }
            }
            return Json(new { res, msg = res ? "注册成功！" : "注册失败！" });
        }

        // 用户主页
        [CheckUserLogin]
        public ActionResult UserHome(string id)
        {
            var user_id = 0;
            if (int.TryParse(id, out user_id))
            {
                var user = userService.LoadEntity(u => u.User_Id == user_id);
                if (user != null)
                {
                    return View(user);
                }
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
        public JsonResult NotExitesNickname()
        {
            string Nickname = Request.Params["Nickname"];
            if (string.IsNullOrEmpty(Nickname))
            {
                return Json(false);
            }
            var user = userService.LoadEntity(u => u.Nickname.Equals(Nickname, StringComparison.CurrentCultureIgnoreCase));
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