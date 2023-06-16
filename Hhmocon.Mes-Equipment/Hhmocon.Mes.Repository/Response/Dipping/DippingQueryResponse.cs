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

using System;

namespace Hhmocon.Mes.Repository.Response.Dipping
{
    /// <summary>
    /// 变量查询的回复类
    /// </summary>
    public class DippingQueryResponse
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string VariableCode;

        /// <summary>
        /// 变量值
        /// </summary>
        public string Value;

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime;
    }
}
