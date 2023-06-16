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

using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Response
{
    /// <summary>
    /// 事件记录统计页面的Response类
    /// </summary>
    public class FaultRecordStaticsResponse
    {
        /// <summary>
        /// 异常类型种类
        /// </summary>
        public List<string> DataName;
        /// <summary>
        /// 连接源
        /// </summary>
        public List<string> LinksSource;
        /// <summary>
        /// 连接目标
        /// </summary>
        public List<string> LinksTarget;

        /// <summary>
        /// 0
        /// </summary>
        public List<int> value;
    }
}
