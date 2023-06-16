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

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// 调拨请求类
    /// </summary>
    public class MoveRequest
    {
        public string iowarehouse_type_id { get; set; }
        public string material_code { get; set; }
        public string material_id { get; set; }

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string material_lot_no { get; set; }
        public string material_name { get; set; }
        public string new_warehouse_id { get; set; }
        public string new_warehouse_loc_id { get; set; }
        public string new_warehouse_loc_name { get; set; }
        public string new_warehouse_name { get; set; }
        public string old_warehouse_id { get; set; }
        public string old_warehouse_loc_id { get; set; }
        public string old_warehouse_loc_name { get; set; }
        public string old_warehouse_name { get; set; }
        public string op_type { get; set; }
        public string parm_id { get; set; }
        public string parm_name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }
    }
}
