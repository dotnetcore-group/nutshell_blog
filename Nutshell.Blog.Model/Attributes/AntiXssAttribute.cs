/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model.Attributes
 * 文件名：AntiXssAttribute
 * 版本号：V1.0.0.0
 * 唯一标识：b0e25a0a-8181-4bb4-b74d-656ef665b165
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-26 12:44:24
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-26 12:44:24
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nutshell.Blog.Model.Attributes
{
    public class AntiXssAttribute: ValidationAttribute, IMetadataAware
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //对于XSS攻击，只需要对string类型进行验证就可以了
            var str = value as string;
            if (!string.IsNullOrWhiteSpace(str) &&
                validationContext.ObjectInstance != null && !
                string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                str = Sanitizer.GetSafeHtmlFragment(str);
                PropertyInfo pi = validationContext.ObjectType.GetProperty(validationContext.MemberName,
                    BindingFlags.Public | BindingFlags.Instance);
                pi.SetValue(validationContext.ObjectInstance, str);
            }
            //由于这个类的目的并不是为了验证，所以返回验证成功
            return ValidationResult.Success;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            //实际上AllowHtmlAttribute也是实现了接口IMetadataAware，在OnMetadataCreated
            //中使用了如下的代码
            metadata.RequestValidationEnabled = false;
        }
    }
}
