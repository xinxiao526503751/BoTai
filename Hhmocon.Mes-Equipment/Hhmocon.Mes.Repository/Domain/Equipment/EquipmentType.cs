using Hhmocon.Mes.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 设备类型
    /// </summary>
    [Table(TableName = "EquipmentType", KeyName = "ID", IsIdentity = false)]
    public class EquipmentType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 设备类型编码，唯一值（不可重复）
        /// </summary>
        public string EQU_TYPE_ID { get; set; }
        /// <summary>
        /// 设备类型名称，唯一值（不可重复）
        /// </summary>
        public string EQU_TYPE_NAME { get; set; }
        /// <summary>
        /// 软删除标志，默认值为0，表示未被删除
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
        public DateTime ? UPDATE_TIME { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string ? UPDATE_BY { get; set; }
    }
}
