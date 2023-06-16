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

using AutoMapper;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// AutoMapper配置类:Profile
    /// </summary>
    public class AutoMapperConfigOfApp : Profile
    {
        /// <summary>
        /// 创建映射关系
        /// </summary>
        public AutoMapperConfigOfApp()
        {
            CreateMap<DealRecordRequest, DealRecordResponse>().ReverseMap();
            CreateMap<CreateUserRequest, sys_user>();

            //更新用户数据的时候
            CreateMap<UpdateUserRequest, sys_user>()
                //密码不能为""或null
                .ForMember(dest => dest.user_passwd, opt => opt.Condition(s => !string.IsNullOrEmpty(s.user_passwd)));


            CreateMap<base_warehouse, BaseWareHouseResponse>();
            CreateMap<base_warehouse_loc, BaseWareHouseLocResponse>();
            CreateMap<WareHouseLocRequest, base_warehouse_loc>().ReverseMap();
            CreateMap<warehouse_io_rec, WareHouceRecResponse>().ReverseMap();
            CreateMap<warehouse_io_rec, MoveRequest>().ReverseMap();

            CreateMap<sys_user, UserResponse>().ReverseMap();

            CreateMap<sys_user, GetUserByDeptIdResponse>();
        }
    }
}
