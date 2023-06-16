/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 订单工序进度回复类 力拓看板 指挥中心 左侧 订单工序进度
    /// </summary>
    public class OrderOperationProgressResponse
    {
        /// <summary>
        /// 订货单号
        /// </summary>
        public string DHDH;

        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name;

        /// <summary>
        /// 工序1下料的完成标志位
        /// </summary>
        public int process1_finish_flag;
        /// <summary>
        /// 工序2开孔的完成标志位
        /// </summary>
        public int process2_finish_flag;
        /// <summary>
        /// 工序3下料贴边的完成标志位
        /// </summary>
        public int process3_finish_flag;
        /// <summary>
        /// 工序4磨边的完成标志位
        /// </summary>
        public int process4_finish_flag;
        /// <summary>
        /// 工序5包装完成标志位
        /// </summary>
        public int process5_finish_flag;
    }
}
