using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 设备保养项
    /// </summary>
    public class UpkeepItem
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 保养项目编号
        /// </summary>
        public string UPKEEP_ITEM_ID { get; set; }
        /// <summary>
        /// 保养项目名称
        /// </summary>
        public string UPKEEP_ITEM_NAME { get; set; }
        /// <summary>
        /// 设备类型ID
        /// </summary>
        public string TYPE_ID { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string DES { get; set; }
        /// <summary>
        /// 软删除标志
        /// </summary>
        public int DELETE_MARK { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATED_TIME { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CREATED_BY { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UPDATE_TIME { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string? UPDATE_BY { get; set; }
    }
}
