using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.MvcHelper
{
    public static class MvcHtmlExtend
    {
        /// <summary>
        /// 权限按钮
        /// </summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="id">控件Id</param>
        /// <param name="icon">控件icon图标class</param>
        /// <param name="text">控件的名称</param>
        /// <param name="perm">权限列表</param>
        /// <param name="keycode">操作码</param>
        /// <param name="hr">分割线</param>
        /// <returns>html</returns>
        public static MvcHtmlString ToolButton(this HtmlHelper helper, string id, string icon, string text, List<Permission> perm, string keycode)
        {
            if (perm.Where(a => a.KeyCode.Equals(keycode, StringComparison.CurrentCultureIgnoreCase)).Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"<a id={id} href=\"#\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'{icon}'\">{text}</a>");
                return new MvcHtmlString(sb.ToString());
            }
            else
            {
                return new MvcHtmlString("");
            }
        }
        /// <summary>
        /// 普通按钮
        /// </summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="id">控件Id</param>
        /// <param name="icon">控件icon图标class</param>
        /// <param name="text">控件的名称</param>
        /// <param name="hr">分割线</param>
        /// <returns>html</returns>
        public static MvcHtmlString ToolButton(this HtmlHelper helper, string id, string icon, string text, bool hr)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<a id=\"{0}\" style=\"float: left;\" class=\"l-btn l-btn-plain\">", id);
            sb.AppendFormat("<span class=\"l-btn-left\"><span class=\"l-btn-text {0}\" style=\"padding-left: 20px;\">", icon);
            sb.AppendFormat("{0}</span></span></a>", text);
            if (hr)
            {
                sb.Append("<div class=\"datagrid-btn-separator\"></div>");
            }
            return new MvcHtmlString(sb.ToString());

        }
    }
}
