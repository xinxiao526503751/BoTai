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

namespace Hhmocon.Mes.Util.SwaggerConfig
{
    public enum ApiGroupNames
    {

        /// <summary>
        /// 登录接口
        /// </summary>
        [GroupInfo(Title = "手持机接口", Description = "用于手持机", Version = "20220115")]
        Android,


        /// <summary>
        /// 登录接口
        /// </summary>
        [GroupInfo(Title = "登录接口", Description = "用于登录", Version = "20210914")]
        Login,

        /// <summary>
        /// 仓库接口
        /// </summary>
        [GroupInfo(Title = "仓库接口", Description = "仓库相关功能", Version = "20210914")]
        WareHouse,

        /// <summary>
        /// 设备接口
        /// </summary>
        [GroupInfo(Title = "设备接口", Description = "设备相关功能", Version = "20210914")]
        Equipment,

        /// <summary>
        /// 事件接口
        /// </summary>
        [GroupInfo(Title = "事件接口", Description = "事件相关功能", Version = "20210914")]
        Fault,

        /// <summary>
        /// 点检接口
        /// </summary>
        [GroupInfo(Title = "点检接口", Description = "点检相关功能", Version = "20210914")]
        Exam,

        /// <summary>
        /// 系统权限接口
        /// </summary>
        [GroupInfo(Title = "系统权限接口", Description = "系统权限接口", Version = "20210914")]
        Sys,

        /// <summary>
        /// 订单管理接口
        /// </summary>
        [GroupInfo(Title = "订单接口", Description = "订单管理接口", Version = "20210914")]
        Plan,

        /// <summary>
        /// 排产调度接口
        /// </summary>
        [GroupInfo(Title = "订单排产接口", Description = "排产调度接口", Version = "20210914")]
        SaleOrder,

        /// <summary>
        /// 工艺路线接口
        /// </summary>
        [GroupInfo(Title = "工艺路线", Description = "工艺路线接口", Version = "20210914")]
        ProcessRoute,

        /// <summary>
        /// 工艺文档接口
        /// </summary>
        [GroupInfo(Title = "工艺文档", Description = "工艺文档接口", Version = "20210914")]
        IOFile,

        /// <summary>
        /// 力拓大屏接口
        /// </summary>
        [GroupInfo(Title = "力拓大屏", Description = "力拓大屏接口接口", Version = "20210914")]
        Lituo,

        /// <summary>
        /// 微信消息推送接口
        /// </summary>
        [GroupInfo(Title = "微信相关", Description = "微信", Version = "20210914")]
        WeiXin,

        [GroupInfo(Title = "质量相关", Description = "质量", Version = "20210914")]
        Defective,
        /// <summary>
        /// 设备类型接口
        /// </summary>
        [GroupInfo(Title = "设备类型接口", Description = "设备类型", Version = "20230427")]
        EquType,
        /// <summary>
        /// 设备信息接口
        /// </summary>
        [GroupInfo(Title = "设备信息接口", Description = "设备信息", Version = "20230427")]
        EquInformation,
        /// <summary>
        /// 设备点检接口
        /// </summary>
        [GroupInfo(Title = "设备点检接口", Description = "设备点检", Version = "20230427")]
        EquCheck,
        /// <summary>
        /// 设备保养接口
        /// </summary>
        [GroupInfo(Title = "设备保养接口", Description = "设备保养", Version = "20230427")]
        EquUpkeep,
        /// <summary>
        /// 设备保养接口
        /// </summary>
        [GroupInfo(Title = "设备维修接口", Description = "设备维修", Version = "20230427")]
        EquMaintenance,
    }
}
