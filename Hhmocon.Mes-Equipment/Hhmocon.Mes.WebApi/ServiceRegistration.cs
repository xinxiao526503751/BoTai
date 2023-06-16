using Hhmocon.Mes.Application.Equipment.Check;
using Hhmocon.Mes.Application.Equipment.EquInfo;
using Hhmocon.Mes.Application.Equipment.EquType;
using Hhmocon.Mes.Application.Equipment.InstallSites;
using Hhmocon.Mes.Application.Equipment.Maintain;
using Hhmocon.Mes.Application.Equipment.Upkeep;
using Hhmocon.Mes.Application.Equipment.WorkPlace;
using Hhmocon.Mes.Database.SqlCreate;
using Hhmocon.Mes.Repository.Repository.Equipment.Check;
using Hhmocon.Mes.Repository.Repository.Equipment.EquInfo;
using Hhmocon.Mes.Repository.Repository.Equipment.EquType;
using Hhmocon.Mes.Repository.Repository.Equipment.InstallSites;
using Hhmocon.Mes.Repository.Repository.Equipment.Maintain;
using Hhmocon.Mes.Repository.Repository.Equipment.Upkeep;
using Hhmocon.Mes.Repository.Repository.Equipment.WorkPlace;
using Microsoft.Extensions.DependencyInjection;

namespace Hhmocon.Mes.WebApi
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// 依赖注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddInfrastructure(this IServiceCollection services)//在starup里实现
        {
            services.AddScoped<ISqlHelper, SqlHelper>();
            services.AddScoped<IWorkPlaceRepository, WorkPlaceRepository>();
            services.AddScoped<IWorkPlaceApp, WorkPlaceApp>();
            services.AddScoped<IInstallSiteRepository, InstallSiteRepository>();
            services.AddScoped<IInstallSiteApp, InstallSiteApp>();
            services.AddScoped<IEquTypeRepository, EquTypeRepository>();
            services.AddScoped<IEquTypeApp, EquTypeApp>();
            services.AddScoped<IEquInfoRepository, EquInfoRepository>();
            services.AddScoped<IEquInfoApp, EquInfoApp>();
            services.AddScoped<ICheckItemRepository, CheckItemRepository>();
            services.AddScoped<ICheckItemApp, CheckItemApp>();
            services.AddScoped<ICheckPlanRepository, CheckPlanRepository>();
            services.AddScoped<ICheckPlanApp, CheckPlanApp>();
            services.AddScoped<IEquCheckItemRepository, EquCheckItemRepository>();
            services.AddScoped<IEquCheckItemApp, EquCheckItemApp>();
            services.AddScoped<IUpkeepItemRepository, UpkeepItemRepository>();
            services.AddScoped<IUpkeepItemApp, UpkeepItemApp>();
            services.AddScoped<IUpkeepPlanRepository, UpkeepPlanRepository>();
            services.AddScoped<IUpkeepPlanApp, UpkeepPlanApp>();
            services.AddScoped<IEquUpkeepItemRepository, EquUpkeepItemRepository>();
            services.AddScoped<IEquUpkeepItemApp, EquUpkeepItemApp>();
            services.AddScoped<IMaintainItemRepository, MaintainItemRepository>();
            services.AddScoped<IMaintainItemApp, MaintainItemApp>();
            services.AddScoped<IEquMaintainItemRepository, EquMaintainItemRepository>();
            services.AddScoped<IEquMaintainItemApp, EquMaintainItemApp>();
        }
    }
}
