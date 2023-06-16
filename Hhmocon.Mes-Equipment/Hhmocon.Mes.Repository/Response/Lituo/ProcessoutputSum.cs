using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 请求参数类
    /// </summary>
    public class ProcessOutputDate
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public List<string> product_name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }
    }

    /// <summary>
    ///返回参数
    /// </summary>
    public class ProcessOutputSum
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime processTime { get; set; }
        /// <summary>
        /// 下料工序加工数量
        /// </summary>
        public int process1Num;

        /// <summary>
        /// 雕刻工序加工数量
        /// </summary>
        public int process2Num;

        /// <summary>
        /// 贴边工序加工数量
        /// </summary>
        public int process3Num;

        /// <summary>
        /// 磨边工序加工数量
        /// </summary>
        public int process4Num;

        /// <summary>
        /// 打包加工数量
        /// </summary>
        public int process5Num;

        /// <summary>
        /// 总数
        /// </summary>
        public int processTotalNum;


    }

    public class processNameOrNum

    {
        public List<string> dateString;

        public List<string> process1nameOrNum;

        public List<string> process2nameOrNum;

        public List<string> process3nameOrNum;

        public List<string> process4nameOrNum;

        public List<string> process5nameOrNum;

        public List<int> total;

    }
}
