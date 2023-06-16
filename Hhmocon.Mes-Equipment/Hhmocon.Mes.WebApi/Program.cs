using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace Hhmocon.Mes.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("XmlConfig/nlog.config");
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //改用Autofac来实现依赖注入
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                })
                .UseNLog();
    }
}
