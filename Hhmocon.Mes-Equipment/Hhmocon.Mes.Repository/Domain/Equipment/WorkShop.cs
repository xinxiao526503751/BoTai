using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 车间地点
    /// </summary>
    public class WorkShop
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 地点Id
        /// </summary>
        public string PlaceId { get; set; }
        /// <summary>
        /// 地点名称
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// 上级地点Id
        /// </summary>
        public string? ParentPlaceId { get; set; }
        /// <summary>
        /// 禁用标志
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
