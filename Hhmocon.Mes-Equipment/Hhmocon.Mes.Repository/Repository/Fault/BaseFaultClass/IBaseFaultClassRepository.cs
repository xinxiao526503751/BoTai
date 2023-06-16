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

using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件类型仓储接口
    /// </summary>
    public interface IBaseFaultClassRepository
    {
        /// <summary>
        /// 将list [base_fault_class]转换成list [TreeModel]
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode(List<base_fault_class> list);
    }
}
