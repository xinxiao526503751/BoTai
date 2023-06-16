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
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository.Domain;

namespace Hhmocon.Mes.Application.AutoMapper
{
    /// <summary>
    /// AutoMapper自定义值解析器,用来source的某些属性运算后赋给destination的某属性的场景
    /// </summary>
    public class TestResolver : IValueResolver<base_warehouse, BaseWareHouseResponse, string>
    {
        private readonly PikachuApp _pikachuApp;
        public TestResolver(PikachuApp pikachuApp)
        {
            _pikachuApp = pikachuApp;
        }

        public string Resolve(base_warehouse source, BaseWareHouseResponse destination, string member, ResolutionContext context)
        {
            return _pikachuApp.GetById<base_location>(source.location_id).location_name;
        }


    }
}
