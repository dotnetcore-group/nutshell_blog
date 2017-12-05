using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.MvcHelper;
using Nutshell.Blog.ViewModel;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    [CheckUserLogin]
    public class CategoryController : BaseController
    {
        public CategoryController(ICustomCategoryService customCategoryService,IUserService userService)
        {
            base.userService = userService;
            base.customCategoryService = customCategoryService;
        }

        [HttpPost]
        public JsonResult EditCategory(int id, string CategoryName)
        {
            var account = GetCurrentAccount();
            var data = new JsonModel { code = 1, msg = " 保存失败，请重试！" };
            var category = customCategoryService.LoadEntity(c => c.Id == id && c.Author.User_Id == account.User_Id);

            if (category != null && !string.IsNullOrEmpty(CategoryName))
            {
                category.CategoryName = CategoryName;
                customCategoryService.EditEntity(category);
                if (customCategoryService.SaveChanges())
                {
                    data.code = 0;
                    data.msg = "保存成功";
                }
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetCategories()
        {
            var data = new JsonModel() { code = 1, msg = "获取失败！" };
            var account = GetCurrentAccount();

            data.res = customCategoryService.GetCustomCategoriesByUserId(account.User_Id);

            if (data.res != null)
            {
                data.code = 0;
                data.msg = "";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult AddCategory(string name)
        {
            var data = new JsonModel { code = 1, msg = "添加失败，请重试！" };
            var account = GetCurrentAccount();
            var user = userService.LoadEntity(u => u.User_Id == account.User_Id);
            if (user != null && !string.IsNullOrEmpty(name))
            {
                customCategoryService.AddEntity(new CustomCategory { Author = user, CategoryName = name });
                if (customCategoryService.SaveChanges())
                {
                    data.code = 0;
                    data.msg = "添加成功";
                }
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult DelCategory(int id)
        {
            var data = new JsonModel { code = 1, msg = "删除失败，请重试！" };
            var account = GetCurrentAccount();
            var category = customCategoryService.LoadEntity(c => c.Id == id && account.User_Id == c.Author.User_Id);
            if (category != null)
            {
                if (category.Articles.Count <= 0)
                {
                    customCategoryService.DeleleEntity(category);
                    if (customCategoryService.SaveChanges())
                    {
                        data.code = 0;
                        data.msg = "删除成功！";
                    }
                }
                else
                {
                    data.msg = "该分类下有文章，不能删除！";
                }
            }
            return Json(data);
        }
    }
}