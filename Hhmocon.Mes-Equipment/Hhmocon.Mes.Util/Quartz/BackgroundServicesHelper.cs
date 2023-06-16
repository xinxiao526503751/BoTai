/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Hhmocon.Mes.Util.Quartz
{
    public static class BackgroundServicesHelper
    {
        /// <summary>
        /// 反射取得所有的业务逻辑类
        /// </summary>
        private static Type[] GetAllChildClass(Type baseType)
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            //取得继承了某个类的所有子类
            .SelectMany(a => a.GetTypes().Where(t => t.BaseType == baseType))
            .ToArray();
            //取得实现了某个接口的类
            //.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISecurity))))  .ToArray();

            return types;
        }


        public static Type[] GetAllBackgroundService()
        {
            return GetAllChildClass(typeof(BackgroundService));
        }

        /// <summary>
        /// 自动增加后台任务.所有继承自BackgroundService的类都会自动运行
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            Type[] backtypes = GetAllBackgroundService();

            foreach (Type backtype in backtypes)
            {
                services.AddTransient(typeof(IHostedService), backtype);
            }
            return services;
        }


    }
}
