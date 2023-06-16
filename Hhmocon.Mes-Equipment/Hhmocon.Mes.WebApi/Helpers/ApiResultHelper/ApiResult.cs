using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hhmocon.Mes.WebApi.Helpers.ApiResultHelper
{
    /// <summary>
    /// 返回值帮助实体类
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据长度
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
