using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{
    public class PowerController : BaseController
    {
        public PowerController(IModuleService mService, IModuleOperateService moduleOperateService, IRoleService roleService)
        {
            moduleService = mService;
            base.moduleOperateService = moduleOperateService;
            base.roleService = roleService;
        }

        // GET: Admin/Power
        [SupportFilter(Action = "Module")]
        public ActionResult Module()
        {
            ViewBag.Perm = GetPermission();
            return View();
        }

        [SupportFilter(Action = "RoleManage")]
        public ActionResult RoleManage()
        {
            return View();
        }

        [SupportFilter(Action = "Create")]
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [SupportFilter(Action="Create")]
        public JsonResult AddRole(Role role)
        {
            var result = new { code = 1, msg = "添加失败！" };
            if (ModelState.IsValid)
            {
                var account = GetCurrentAccount();
                role.CreateUser = account.Nickname;
                roleService.AddEntity(role);
                var res = roleService.SaveChanges();
                if (res)
                {
                    result = new { code = 0, msg = "添加成功！" };
                }
            }
            return Json(result);
        }

        [SupportFilter(Action = "RoleManage")]
        public JsonResult GetRoleList()
        {
            var data = roleService.LoadEntities(r => true).Select(r=>new {
                Id = r.Role_Id,
                Name = r.Role_Name,
                Description = r.Description,
                CreateTime = r.CreateTime,
                CreatePerson = r.CreateUser
            });

            return Json(new { data}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SupportFilter(Action = "Module")]
        public JsonResult GetModuleList(int? id)
        {
            List<dynamic> result = new List<dynamic>();
            var list = moduleService.GetModuleList(0).Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
                ParentId = r.Parent_Id,
                Url = r.Url,
                Iconic = r.Iconic,
                Sort = r.Sort,
                Remark = r.Remark,
                Enable = r.Enable,
                CreatePerson = r.CreateUser,
                CreateTime = r.CreateTime,
                IsLast = r.IsLast
            });
            foreach (var item in list)
            {
                var children = moduleService.GetModuleList(item.Id).Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    ParentId = r.Parent_Id,
                    Url = r.Url,
                    Iconic = r.Iconic,
                    Sort = r.Sort,
                    Remark = r.Remark,
                    Enable = r.Enable,
                    CreatePerson = r.CreateUser,
                    CreateTime = r.CreateTime,
                    IsLast = r.IsLast
                });
                result.Add(new { item.Id, item.Name, item.ParentId, item.Url, item.Sort, item.Remark, item.Enable, item.CreatePerson, item.IsLast, item.CreateTime, item.Iconic, children });
            }

            return Json(result);
        }

        [HttpPost]
        [SupportFilter(Action = "Module")]
        public JsonResult GetOptListByModule(int? module_id)
        {
            if (!module_id.HasValue)
            {
                module_id = 0;
            }
            var operates = moduleOperateService.GetModuleOperates((int)module_id).Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
                KeyCode = r.KeyCode,
                ModuleId = r.Module.Id,
                IsValid = r.IsValid,
                Sort = r.Sort
            });
            return Json(operates);
        }

        [SupportFilter(Action = "Create")]
        public JsonResult CreateModule()
        {
            return null;
        }

        [SupportFilter(Action = "Edit")]
        public JsonResult EditModule()
        {
            return null;
        }

        [SupportFilter(Action = "Delete")]
        public JsonResult DeleteModule(int id)
        {
            return Json(id);
        }
    }
}