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

namespace Hhmocon.Mes.Application.Response
{
    /// <summary>
    /// defectivestatistic不合格产品统计页面的环状图
    /// </summary>
    public class GetProductsNumberByTimeDonotChartResponse
    {
        /// <summary>
        /// 不合格原因名称
        /// </summary>
        public string name;

        /// <summary>
        /// 不合格产品数
        /// </summary>
        public int value;

        /// <summary>
        /// 拥有的孩子的数量
        /// </summary>
        public int sonCount;
    }
}
