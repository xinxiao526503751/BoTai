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
using Hhmocon.Mes.Repository.Domain;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// AutoMapper配置类:Profile
    /// </summary>
    public class AutoMapperConfigOfRepository : Profile
    {
        /// <summary>
        /// 创建映射关系
        /// </summary>
        public AutoMapperConfigOfRepository()
        {
            //user To User_dept关联表
            CreateMap<sys_user, sys_user_dept>()
                .ForMember(c => c.dept_id, v => v.MapFrom(a => a.dept_id))
                .ForMember(c => c.user_id, v => v.MapFrom(a => a.user_id))
                ;
            CreateMap<warehouse_io_rec, warehouse_io_rec>();
        }
    }
}
