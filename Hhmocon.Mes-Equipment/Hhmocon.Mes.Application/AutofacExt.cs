
using Autofac;
using Autofac.Extras.Quartz;
using Hhmocon.Mes.Cache;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using IContainer = Autofac.IContainer;

namespace Hhmocon.Mes.Application
{
    public static class AutofacExt
    {
        private static readonly IContainer _container;


        /// <summary>
        /// 使用容器的三部曲:实例化一个容器、注册、获取服务
        /// </summary>
        /// <param name="builder"></param>
        public static void InitAutofac(ContainerBuilder builder)
        {

            //注册数据库基础操作和工作单元
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());//注册整个APP.DLL
            builder.RegisterType(typeof(CacheContext)).As(typeof(ICacheContext));
            builder.RegisterType(typeof(HttpContextAccessor)).As(typeof(IHttpContextAccessor));

            //注入授权
            //builder.RegisterType(typeof(UserAuthApp)).As(typeof(IUserAuth))
            builder.RegisterType(typeof(Repository.LoginRelated.SSO.LocalAuth)).As(typeof(IAuth));
            builder.RegisterType<PikachuRepository>();

            builder.RegisterType(typeof(BaseMaterialRepository)).As(typeof(IBaseMaterialRepository));
            //builder.RegisterType<SqlHelper>();



            builder.RegisterInstance(
                 new JobSchedule(jobType: typeof(QuartzJobRunner), cronExpression: "0/5 * * * * ?")
            );

            InitDependency(builder);//注入所有继承了IDependency的接口



            builder.RegisterModule(new QuartzAutofacFactoryModule());//quartz使用autofac注入，Ijob使用原生的容器(怎么注入都行，容器随便用)
        }



        /// <summary>
        /// 注入所有继承了IDependency接口
        /// </summary>
        /// <param name="builder"></param>
        private static void InitDependency(ContainerBuilder builder)
        {
            Type baseType = typeof(IDependency);
            //获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
            List<CompilationLibrary> compilationLibrary = DependencyContext.Default
                .CompileLibraries
                .Where(x => !x.Serviceable
                & x.Type == "project")
                .ToList();
            //var count1 = compilationLibrary.Count;
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (CompilationLibrary _compilation in compilationLibrary)
            {
                try
                {
                    assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }

            builder.RegisterAssemblyTypes(assemblyList.ToArray())//注册一组程序集

                //指定注册的条件
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)//必须是基类的子类或接口且不是抽象
                .AsSelf()//注册自身
                .AsImplementedInterfaces()//注册继承的接口

                //注册声明周期
                .InstancePerLifetimeScope();//每次请求都创建新实例
        }


    }
}