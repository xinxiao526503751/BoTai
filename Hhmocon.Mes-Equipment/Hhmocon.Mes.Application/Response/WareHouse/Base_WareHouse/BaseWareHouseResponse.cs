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

namespace Hhmocon.Mes.Application.Response
{
    /// <summary>
    /// 仓库定义页面Response类
    /// </summary>
    public class BaseWareHouseResponse
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouse_code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string warehouse_name { get; set; }

        /// <summary>
        /// 仓库类型名称
        /// </summary>
        public string warehouse_type_name { get; set; }

        /// <summary>
        /// 地点id，前端做回显需要
        /// </summary>
        public string location_id { get; set; }

        /// <summary>
        /// 地点名称
        /// </summary>
        public string location_name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 创建人name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人name
        /// </summary>
        public string modified_by_name { get; set; }




        /// <summary>
        /// 仓库id
        /// </summary>
        public string warehouse_id { get; set; }

        /// <summary>
        /// 仓库类型id
        /// </summary>
        public string warehouse_type_id { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

    }
}
