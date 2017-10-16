using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc
{
    public static class AutofacConfig
    {
        /// <summary>
        /// 调用autofac框架实现业务逻辑层以及数据仓储层的对象的创建
        /// 创建MVC控制器类的对象（调用控制器类有参构造函数，接管DefaultControllerFactory的工作）
        /// </summary>
        public static void Register()
        {
            // 实例化autofac的创建容器
            var builder = new ContainerBuilder();

            // 控制器类存放的程序集
            Assembly controllerAssembly = Assembly.Load("Nutshell.Blog.Mvc");
            builder.RegisterControllers(controllerAssembly);

            // 注册数据仓储层所在程序集中所有类的实例
            Assembly repositoryAssembly = Assembly.Load("Nutshell.Blog.Repository");
            // 创建该程序集中所有类的实例以此类的实现接口形式存储
            builder.RegisterTypes(repositoryAssembly.GetTypes()).AsImplementedInterfaces();

            Assembly servicersAssembly = Assembly.Load("Nutshell.Blog.Service");
            // 创建该程序集中所有类的实例以此类的实现接口形式存储
            builder.RegisterTypes(servicersAssembly.GetTypes()).AsImplementedInterfaces();

            // 创建autofac容器
            var container = builder.Build();

            // 将container对象缓存到HttpRuntime.cache中，并且是永久有效
            //CacheMgr.SetData(Keys.AutofacContainer, container);

            //Resolve方法可以从autofac容器中获取指定的IsysuserInfoSercies的具体实现类的实体对象
            //container.Resolve<IsysuserInfoSercies>();

            // 将控制器类的实例创建交给autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}