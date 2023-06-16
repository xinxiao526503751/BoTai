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
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 产品缺陷仓储接口
    /// </summary>
    public interface IBaseDefectiveProductRepository
    {
        /// <summary>
        /// 通过时间范围和不合格原因找产品
        /// </summary>
        public List<base_defective_product> GetByTimeScopeAndReason(DateTime StartTime, DateTime EndTime, base_defective_reason base_Defective_Reason);

    }
}
