using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.HelpModel
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class QueryModel
    {
        /// <summary>
        /// 查询属性1
        /// </summary>
        public string Q1 { get; set; }
        /// <summary>
        /// 查询属性2
        /// </summary>
        public string  Q2 { get; set; }
        /// <summary>
        /// 查询Id
        /// </summary>
        public string QId { get; set; }
        /// <summary>
        /// 查询分页页数
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 查询分页每页数据量
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 判断标志
        /// </summary>
        public int flag { get; set; }
    }
}
