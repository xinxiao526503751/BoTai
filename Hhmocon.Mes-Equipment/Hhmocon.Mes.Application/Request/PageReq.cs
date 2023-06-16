using System.ComponentModel;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class PageReq
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        [DefaultValue(1)]
        public int page { get; set; }
        /// <summary>
        /// 每页数据条数
        /// </summary>
        [DefaultValue(10)]
        public int rows { get; set; }

        /// <summary>
        /// 组合条件
        /// </summary>
        [DefaultValue("")]
        public string key { get; set; }

        /// <summary>
        /// 排序规则  ASC  DESC  
        /// </summary>

        [DefaultValue("DESC")]
        public string sort { get; set; }  //asc  desc

        /// <summary>
        /// 排序字段 多个中间用“,”隔开
        /// </summary>
        [DefaultValue("create_time")]
        public string order { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public PageReq()
        {
            page = 1;
            rows = 10;
        }
    }
}
