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
    /// 仓库记录Response类
    /// </summary>
    public class WareHouceRecResponse
    {

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string material_lot_no { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string material_code { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int rec_qty { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouse_code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string warehouse_name { get; set; }


        /// <summary>
        /// 库位编码
        /// </summary>
        public string warehouse_loc_code { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string warehouse_loc_name { get; set; }

        /// <summary>
        /// 记录生成时间(出、入库时间)
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 记录操作人id
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 记录操作人name
        /// </summary>
        public string create_by_name { get; set; }



        /// <summary>
        /// 记录id
        /// </summary>
        public string warehouse_io_rec_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 出入库类型ID
        /// </summary>
        public string iowarehouse_type_id { get; set; }

        /// <summary>
        /// 仓库ID（关联仓库）
        /// </summary>
        public string warehouse_id { get; set; }

        /// <summary>
        /// 库位ID（关联库位）
        /// </summary>
        public string warehouse_loc_id { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string barcode { get; set; }

        /// <summary>
        /// 操作动作（get:领料；return:退料；pull:物料拉动发货；load:投料；code;打码；out;出库；in;入库；move:调拨
        /// </summary>
        public string op_type { get; set; }

        /// <summary>
        /// 单位ID(出入库单位)
        /// </summary>
        public string parm_id { get; set; }

        /// <summary>
        /// 仓库name
        /// </summary>
        public string parm_name { get; set; }

    }
}
