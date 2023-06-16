using Hhmocon.Mes.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 设备具体信息
    /// </summary>
    [Table(TableName = "EquipmentInfo", KeyName = "ID", IsIdentity = false)]
    public class EquipmentInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EQU_ID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EQU_NAME { get; set; }
        /// <summary>
        /// 设备类型编码，是约束于EquipmentType表的逻辑上的外键
        /// </summary>
        public string TYPE_ID { get; set; }
        /// <summary>
        /// 设备规格
        /// </summary>
        public string EQU_SPEC { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public string EQU_STATUS { get; set; }
        /// <summary>
        /// 所属车间
        /// </summary>
        public string WORK_SHOP { get; set; }
        /// <summary>
        /// 安装地点
        /// </summary>
        public string INSTALL_SITE { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string HEAD { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string PHONE_NO { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string MANUFACTURER { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string VENDOR { get; set; }
        /// <summary>
        /// 购入时间
        /// </summary>
        public string PUR_TIME { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        public string ENABLE_TIME { get; set; }
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
        public DateTime? UPDATE_TIME { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string? UPDATE_BY { get; set; }
    }
}
