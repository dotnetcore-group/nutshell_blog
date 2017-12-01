using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Controllers;
using Nutshell.Blog.Mvc.MvcHelper;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUserService userService)
        {
            base.userService = userService;
        }

        // GET: Account
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.Return = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SignIn(UserLogin user)
        {
            string returnurl = Request["returnUrl"] ?? "/";
            string msg = "";
            dynamic data = null;
            if (ModelState.IsValid)
            {
                var code = user.ValidCode;
                if (Session[Keys.ValidCode] == null || !Session[Keys.ValidCode].ToString().Equals(code, StringComparison.CurrentCultureIgnoreCase))
                {
                    data = new { code = 1, msg = "验证码错误", url = "" };
                    Session[Keys.ValidCode] = null;
                    return Json(data);
                }
                var userinfo = userService.ValidationUser(user.UserName, user.Password, out msg);
                data = new { code = 1, msg = msg, url = "" };
                // 验证通过
                if (userinfo != null)
                {
                    if (!userinfo.IsValid)
                    {
                        Session[Keys.ValidCode] = null;
                        Session["_email"] = userinfo.Email;
                        data = new { code = 4, msg = "您的账号绑定的邮箱未验证，请先通过验证。", url = "/account/valid" };
                    }
                    else
                    {
                        // 将用户信息存入memcache
                        // 返回客户端一个session id
                        ValidatedUser(userinfo.ToAccount());
                        data = new { code = 0, msg = msg, url = returnurl };
                    }
                }

            }
            return Json(data);
        }

        void ValidatedUser(Account account)
        {
            try
            {
                var sessionid = Guid.NewGuid().ToString();
                MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(account), DateTime.Now.AddHours(1));
                Response.Cookies[Keys.SessionId].Value = sessionid;
                Response.Cookies[Keys.SessionId].Expires = DateTime.Now.AddHours(1);
                Response.Cookies[Keys.SessionId].HttpOnly = true;
                //Response.Cookies[Keys.UserId].Value = account.User_Id.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Register(string returnUrl)
        {
            ViewBag.Return = returnUrl;
            return View();
        }

        public ActionResult Valid(string confirmatio)
        {
            ViewBag.Model = confirmatio;
            var email = Session["_email"]?.ToString();
            if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(confirmatio))
            {
                var id = Guid.NewGuid().ToString("N");
                var user = userService.LoadEntity(u => u.Email.Equals(email));
                if (SendValidEmail(email, id) && user != null)
                {
                    MemcacheHelper.Set(id, SerializerHelper.SerializeToString(user), DateTime.Now.AddHours(1));
                }
                ViewBag.Model = id;
                ViewBag.Email = email;
            }
            return View();
        }

        // 验证邮箱
        [HttpPost]
        public JsonResult ValidEmail(string confirmatio)
        {
            var data = new { code = 1, msg = "验证失败，请重试！", error = "" };
            if (!string.IsNullOrEmpty(confirmatio))
            {
                var obj = MemcacheHelper.Get(confirmatio);
                if (obj != null)
                {
                    var user = SerializerHelper.DeserializeToObject<User>(obj.ToString());
                    if (user != null)
                    {
                        // 邮箱验证成功
                        user.IsValid = true;
                        userService.EditEntity(user);
                        if (userService.SaveChanges())
                        {
                            MemcacheHelper.Delete(confirmatio);
                            var sessionid = Guid.NewGuid().ToString();
                            MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(user.ToAccount()), DateTime.Now.AddHours(1));
                            Response.Cookies[Keys.SessionId].Value = sessionid;
                            Response.Cookies[Keys.SessionId].Expires = DateTime.Now.AddHours(1);
                            data = new { code = 0, msg = "成功绑定邮箱。", error = "" };
                        }
                    }
                    else
                    {
                        data = new { code = 1, msg = "验证失败，请重试！", error = "用户不存在。" };
                    }
                }
                else
                {
                    data = new { code = 1, msg = "验证失败，请重试！", error = "验证信息过期。" };
                }
            }
            return Json(data);
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgetPassword(ForgetPwd forgetPwd)
        {
            var res = 1;
            var msg = "";
            var url = "";
            if (!ModelState.IsValid)
            {
                return Json(new { code=1,msg="输入的数据有误，请重新输入。"});
            }
            if (string.IsNullOrEmpty(forgetPwd.Code))
            {
                msg = "输入验证码";
            }
            else
            {
                if(Session[Keys.ValidCode] == null || !Session[Keys.ValidCode].ToString().Equals(forgetPwd.Code, StringComparison.CurrentCultureIgnoreCase))
                {
                    msg = "验证码错误";
                    Session[Keys.ValidCode] = null;
                }
                else
                {
                    var user = userService.LoadEntity(u => u.Login_Name.Equals(forgetPwd.UserName, StringComparison.CurrentCultureIgnoreCase));
                    if (user != null)
                    {
                        res = 0;
                        url = $"ForgetPasswordWay?email={user.Email}";
                    }
                    else
                    {
                        msg = "账号不存在";
                    }
                }
            }
            var data = new { code = res, msg = msg, res = url };
            return Json(data);
        }

        [HttpGet]
        public ActionResult ForgetPasswordWay(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public JsonResult ForgetPasswordEmail(string email)
        {
            var data = new { code = 1, msg = "邮件发送失败，请重试。" };
            var user = userService.LoadEntity(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            if (user != null)
            {
                var id = Guid.NewGuid().ToString("N");
                var link = GlobalConfig.UrlPrefix + $"/account/resetpassword?active={id}";
                var templetpath = Server.MapPath("~/Templates/RetrievePassword.txt");
                NameValueCollection collection = new NameValueCollection();
                var timespan = DateTime.Now.AddMinutes(10);
                collection.Add("ename", user.Nickname);
                collection.Add("link", link);
                collection.Add("expired", timespan.ToString("yyyy-MM-dd HH:mm:ss"));
                var body = TemplateHelper.BuildByFile(templetpath, collection);
                if (EmailHelper.Send(user.Email, "找回您的账户密码", body))
                {
                    MemcacheHelper.Set(id, SerializerHelper.SerializeToString(user), timespan);
                    data = new { code = 0, msg = "重置密码链接已经发送到您的邮箱中了，请注意查收。" };
                }
            }

            return Json(data);
        }

        public ActionResult ResetPassword(string active)
        {
            if (!string.IsNullOrEmpty(active))
            {
                var obj = MemcacheHelper.Get(active);
                if (obj != null)
                {
                    var user = SerializerHelper.DeserializeToObject<User>(obj.ToString());
                    if (user != null)
                    {
                        ViewBag.Active = active;
                        return View();
                    }
                }
            }
            ViewBag.Error = "重置密码链接失效，请重新找回密码";
            return View("ForgetPassword");
        }

        [HttpPost]
        public JsonResult ResetPassword(string active, ResetPwd resetPwd)
        {
            var data = new { code = 1, msg = "修改失败" };
            if (!string.IsNullOrEmpty(active) && ModelState.IsValid)
            {
                if (Session[Keys.ValidCode] == null || !Session[Keys.ValidCode].ToString().Equals(resetPwd.Code, StringComparison.CurrentCultureIgnoreCase))
                {
                    data = new { code = 1, msg = "验证码错误" };
                    Session[Keys.ValidCode] = null;
                    return Json(data);
                }
                var obj = MemcacheHelper.Get(active);
                if (obj != null)
                {
                    var user = SerializerHelper.DeserializeToObject<User>(obj.ToString());
                    if (user != null)
                    {
                        user.Login_Password = resetPwd.Password.Md5_32();
                        userService.EditEntity(user);
                        if (userService.SaveChanges())
                        {
                            data = new { code = 0, msg = "修改成功，请牢记新密码。" };
                            MemcacheHelper.Set(active, null, DateTime.Now.AddHours(-1));
                        }
                    }
                }
            }
            return Json(data);
        }

        public FileResult GetValidCode()
        {
            var code = ValidCodeHelper.CreateValidateCode(6);
            Session[Keys.ValidCode] = code;
            byte[] buffer = ValidCodeHelper.CreateValidateGraphic(code);//把验证码画到画布
            return File(buffer, "image/jpeg");
        }

        [HttpPost]
        [CheckUserLogin]
        public JsonResult ChangePwd(string oldPwd, string newPwd)
        {
            string msg = string.Empty;
            var user = userService.ChangePassword(Account.User_Id, oldPwd, newPwd, out msg);

            var data = new { code = 1, msg = msg };
            if (user != null)
            {
                var sessionid = Guid.NewGuid().ToString();
                MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(user.ToAccount()), DateTime.Now.AddHours(1));
                Response.Cookies[Keys.SessionId].Value = sessionid;
                Response.Cookies[Keys.SessionId].Expires = DateTime.Now.AddHours(1);
                data = new { code = 0, msg = "修改成功！" };
            }

            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(UserRegister user)
        {
            string returnurl = Request["returnUrl"] ?? "/";
            var res = new JsonModel { code = 1, msg = "注册失败！" };
            if (ModelState.IsValid)
            {
                var userInfo = userService.AddEntity(new User
                {
                    Login_Name = user.UserName,
                    Login_Password = user.Password.Md5_Base64(),
                    Nickname = user.Nickname,
                    Email = user.Email
                });
                // 注册成功
                if (userService.SaveChanges())
                {
                    Session["_email"] = userInfo.Email;
                    res.code = 0;
                    res.msg = "注册成功，请登录邮箱完成验证。";
                    //var id = Guid.NewGuid().ToString("N");
                    //if (SendValidEmail(user.Email, id))
                    //{
                    //    res.code = 0;
                    //    res.res = returnurl;
                    //    res.msg = "注册成功！";
                    //    MemcacheHelper.Set(id, SerializerHelper.SerializeToString(userInfo), DateTime.Now.AddHours(1));
                    //}
                    //else
                    //{
                    //    res.msg = "邮件发送失败！";
                    //}
                }
            }
            return Json(res);
        }

        private bool SendValidEmail(string email, string id)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var user = userService.LoadEntity(u => u.Email.Equals(email));
            if (user == null)
            {
                return false;
            }
            Session["_email"] = email;
            var href = GlobalConfig.UrlPrefix + $"/account/valid?confirmatio={id}";
            var templetpath = Server.MapPath("~/Templates/ValidEmail.txt");
            NameValueCollection collection = new NameValueCollection();
            collection.Add("ename", user.Nickname);
            collection.Add("link", href);
            var body = TemplateHelper.BuildByFile(templetpath, collection);
            return EmailHelper.Send(email, "请激活你在果壳园的注册邮箱", body);
        }

        // 重新发送验证邮件
        [HttpPost]
        public JsonResult ReSendValidEmail()
        {
            var data = new { code = 1, msg = "发送失败！" };
            var email = Session["_email"]?.ToString();
            if (!string.IsNullOrEmpty(email))
            {
                var userinfo = userService.LoadEntity(u => u.Email.Equals(email));
                if (userinfo != null)
                {
                    var id = Guid.NewGuid().ToString("N");
                    if (SendValidEmail(email, id))
                    {
                        MemcacheHelper.Set(id, SerializerHelper.SerializeToString(userinfo), DateTime.Now.AddHours(1));
                        data = new { code = 0, msg = "邮件发送成功！" };
                    }
                }
            }
            return Json(data);
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
        [HttpPost]
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
        [HttpPost]
        public JsonResult NotExitesEmail()
        {
            string Email = Request.Params["Email"];
            if (string.IsNullOrEmpty(Email))
            {
                return Json(false);
            }
            var user = userService.LoadEntity(u => u.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase));
            return user == null ? Json(true) : Json(false);
        }
    }
}