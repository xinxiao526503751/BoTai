using Autofac;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.Quartz;
using Hhmocon.Mes.Util.SwaggerConfig;
using Hhmocon.Mes.WebApi.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;

namespace Hhmocon.Mes.WebApi
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string HhmoconMesAllowSpecificOrigins = "_hhmoconmesAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                ILogger<StartupLogger> service = provider.GetRequiredService<ILogger<StartupLogger>>();
                return new StartupLogger(service);
            });

            StartupLogger logger = services.BuildServiceProvider().GetRequiredService<StartupLogger>();

            services.AddMemoryCache();

            #region 添加timeJob服务
            //services.AddTimedJob();
            #endregion

            Type[] TypeofAutoMapper = {
                typeof(AutoMapperConfigOfApp),
                typeof(AutoMapperConfigOfRepository) };
            services.AddAutoMapper(TypeofAutoMapper);
            //添加依赖
            services.AddInfrastructure();

            services.AddControllers();

            //services.AddDistributedMemoryCache();
            //services.AddSession();

            services.AddHttpContextAccessor();

            #region 配置Kestrel启动和IIS宿主启动
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<IISServerOptions>(options =>
            {
                //解决大文件下载问题bug：18992
                options.AllowSynchronousIO = true;
            });
            #endregion

            #region Quartz
            //添加quartz服务
            services.AddJob();
            services.AddQuartzJobService();
            #endregion

            #region cors
            services.AddCors(options =>
            {
                options.AddPolicy(HhmoconMesAllowSpecificOrigins,
                builder =>
                {
                    builder
                    //.WithOrigins("http://")//("http://localhost:8080")  //实际以发布的端口为准
                    .AllowAnyOrigin()
                    //  .AllowAnyHeader()
                    .AllowAnyMethod()
                    //.AllowCredentials()
                    .WithHeaders(HeaderNames.ContentType, SysDefine.TOKEN_NAME.ToLower());
                });
            });
            #endregion

            #region swagger
            services.AddSwaggerGen(c =>
            {
                logger.LogInformation($"api doc basepath:{AppContext.BaseDirectory}");

                foreach (string name in Directory.GetFiles(AppContext.BaseDirectory, "*.*", SearchOption.AllDirectories).Where(f => Path.GetExtension(f).ToLower() == ".xml"))
                {
                    c.IncludeXmlComments(name, includeControllerXmlComments: true);//应用程序集xml，用于加载出备注信息等
                    logger.LogInformation($"find api file{name}");
                }
                //遍历ApiGroupNames所有枚举值生成不同组别的接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    GroupInfoAttribute info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    c.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    });
                });
                //生成"无分组"接口文档
                c.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "无分组"
                });

                //判断分组里放哪些接口
                c.DocInclusionPredicate((docName, apiDescription) =>
                {
                    //循环到NoGroup分组时
                    if (docName == "NoGroup")
                    {
                        //只要接口没加IApiDescriptionGroupNameProvider特性的都归于这个组
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        //分组名和IApiDescriptionGroupNameProvider标注的一致时，就收纳
                        return apiDescription.GroupName == docName;
                    }
                });



                c.OperationFilter<GlobalHttpHeaderOperationFilter>(); // 添加httpHeader参数

                #region 在swagger中添加JWT认证功能
                //添加  ID = Bearer的安全定义
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme()
                    {
                        //这里是对Swagger的Jwt授权页面的描述
                        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token,注意两者之间有空格",
                        Name = "Authorization",
                        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置（请求头）
                        Type = SecuritySchemeType.ApiKey,//显示授权的类型
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                #endregion

            });
            #endregion

            #region Json
            services.AddControllers().AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.PropertyNamingPolicy = null;
            }).AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion
        }


        /// <summary>
        /// Autofac的配置容器
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ContainerBuilder的populate方法可以将服务添加进builder中
            //也可以直接在builder中注册服务，交给autofac托管
            AutofacExt.InitAutofac(builder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            StaticFileOptions staticfile = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(AppContext.BaseDirectory),
                ServeUnknownFileTypes = true //设置.apk .nupkg .cs等后缀的文件不被限制,不安全
                //手动添加mime类型(媒体类型)
                //ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                //{
                //    { ".apk","application/vnd.android.package-archive"},
                //    { ".nupkg","application/zip"},
                //})
            };
            app.UseStaticFiles(staticfile);

            //app.UseSession();
            app.UseCookiePolicy();

            app.UseRouting();

            //app.UseTimedJob();

            app.UseAuthentication();

            app.UseCors(HhmoconMesAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //配置ServiceProvider
            //AutofacContainerModule.ConfigServiceProvider(app.ApplicationServices);

            app.UseSwagger();

            //生成接口文档的路径
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "无分组");
                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    GroupInfoAttribute info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    options.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                });
                options.RoutePrefix = "";
                // 路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，
                // 这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可

            });

        }
    }
}
