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

using System;

namespace Hhmocon.Mes.Repository.Response
{
    /// <summary>
    /// 工艺路线Response类
    /// </summary>
    public class ProcessRouteResponse
    {
        /// <summary>
        /// 工艺路线id
        /// </summary>
        public string process_route_id;
        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string process_route_code;

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string process_route_name;

        /// <summary>
        /// 物料编码
        /// </summary>
        public string material_code;

        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name;

        /// <summary>
        /// 是否主工艺路线
        /// </summary>
        public int is_master;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }


    }
}
