
namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 描 述：分页参数
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int total
        {
            get
            {
                if (records > 0)
                {
                    return records % rows == 0 ? records / rows : records / rows + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }


    /// <summary>
    /// 统计个数
    /// </summary>
    public class CountData
    {
        /// <summary>
        /// 总记录条数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public long QualifieNumber { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public long UnqualifieNumber { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public dynamic Data { get; set; }

        public CountData()
        {
            Total = 0;
            QualifieNumber = 0;
            UnqualifieNumber = 0;
        }
    }

    public class PageData
    {
        /// <summary>
        /// 总记录条数
        /// </summary>
        public long Total { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public dynamic Data { get; set; }

        public PageData()
        {
            Total = 0;
        }
    }
}
