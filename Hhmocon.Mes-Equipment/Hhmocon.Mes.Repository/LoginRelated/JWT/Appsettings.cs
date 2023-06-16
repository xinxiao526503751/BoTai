using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        private static IConfiguration Configuration { get; set; }

        static Appsettings()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
           .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })//请注意要把当前appsetting.json 文件->右键->属性->复制到输出目录->始终复制
           .Build();

        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {
                string val = string.Empty;
                foreach (string section in sections)
                {
                    val += section + ":";
                }

                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
