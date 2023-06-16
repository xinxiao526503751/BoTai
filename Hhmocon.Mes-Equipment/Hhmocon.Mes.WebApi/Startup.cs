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

            #region ���timeJob����
            //services.AddTimedJob();
            #endregion

            Type[] TypeofAutoMapper = {
                typeof(AutoMapperConfigOfApp),
                typeof(AutoMapperConfigOfRepository) };
            services.AddAutoMapper(TypeofAutoMapper);
            //�������
            services.AddInfrastructure();

            services.AddControllers();

            //services.AddDistributedMemoryCache();
            //services.AddSession();

            services.AddHttpContextAccessor();

            #region ����Kestrel������IIS��������
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<IISServerOptions>(options =>
            {
                //������ļ���������bug��18992
                options.AllowSynchronousIO = true;
            });
            #endregion

            #region Quartz
            //���quartz����
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
                    //.WithOrigins("http://")//("http://localhost:8080")  //ʵ���Է����Ķ˿�Ϊ׼
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
                    c.IncludeXmlComments(name, includeControllerXmlComments: true);//Ӧ�ó���xml�����ڼ��س���ע��Ϣ��
                    logger.LogInformation($"find api file{name}");
                }
                //����ApiGroupNames����ö��ֵ���ɲ�ͬ���Ľӿ��ĵ���Skip(1)����ΪEnum��һ��FieldInfo�����õ�һ��Intֵ
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //��ȡö��ֵ�ϵ�����
                    GroupInfoAttribute info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    c.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    });
                });
                //����"�޷���"�ӿ��ĵ�
                c.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "�޷���"
                });

                //�жϷ��������Щ�ӿ�
                c.DocInclusionPredicate((docName, apiDescription) =>
                {
                    //ѭ����NoGroup����ʱ
                    if (docName == "NoGroup")
                    {
                        //ֻҪ�ӿ�û��IApiDescriptionGroupNameProvider���ԵĶ����������
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        //��������IApiDescriptionGroupNameProvider��ע��һ��ʱ��������
                        return apiDescription.GroupName == docName;
                    }
                });



                c.OperationFilter<GlobalHttpHeaderOperationFilter>(); // ���httpHeader����

                #region ��swagger�����JWT��֤����
                //���  ID = Bearer�İ�ȫ����
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme()
                    {
                        //�����Ƕ�Swagger��Jwt��Ȩҳ�������
                        Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token,ע������֮���пո�",
                        Name = "Authorization",
                        In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ�ã�����ͷ��
                        Type = SecuritySchemeType.ApiKey,//��ʾ��Ȩ������
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
                //����ѭ������
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion
        }


        /// <summary>
        /// Autofac����������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ContainerBuilder��populate�������Խ�������ӽ�builder��
            //Ҳ����ֱ����builder��ע����񣬽���autofac�й�
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
                ServeUnknownFileTypes = true //����.apk .nupkg .cs�Ⱥ�׺���ļ���������,����ȫ
                //�ֶ����mime����(ý������)
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

            //����ServiceProvider
            //AutofacContainerModule.ConfigServiceProvider(app.ApplicationServices);

            app.UseSwagger();

            //���ɽӿ��ĵ���·��
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "�޷���");
                //����ApiGroupNames����ö��ֵ���ɽӿ��ĵ���Skip(1)����ΪEnum��һ��FieldInfo�����õ�һ��Intֵ
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //��ȡö��ֵ�ϵ�����
                    GroupInfoAttribute info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    options.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                });
                options.RoutePrefix = "";
                // ·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�
                // ���ʱ��ȥlaunchSettings.json�а�"launchUrl": "swagger/index.html"ȥ���� Ȼ��ֱ�ӷ���localhost:8001/index.html����

            });

        }
    }
}
