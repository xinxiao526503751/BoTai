using System;

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// 库位请求类
    /// </summary>
    public class WareHouseLocRequest
    {
        /// <summary>
        /// 库位id
        /// </summary>
        public string warehouse_loc_id { get; set; }

        /// <summary>
        /// 所属仓库id
        /// </summary>
        public string warehouse_id { get; set; }

        /// <summary>
        /// 所属仓库name
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
        /// 排序顺序
        /// </summary>
        public int sort_no { get; set; }

        /// <summary>
        /// 最大数量
        /// </summary>
        public int max_num { get; set; }

        /// <summary>
        /// 安全库存量
        /// </summary>
        public int safety_num { get; set; }

        /// <summary>
        /// 当前数据量
        /// </summary>
        public int current_num { get; set; }

        /// <summary>
        /// 是否生产线(0:否 1：是)
        /// </summary>
        public int is_line { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }
    }
}
