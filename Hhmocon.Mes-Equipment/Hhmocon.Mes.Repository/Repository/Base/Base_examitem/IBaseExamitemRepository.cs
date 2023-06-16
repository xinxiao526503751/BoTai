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
using System.Data;

namespace Hhmocon.Mes.Repository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseExamitemRepository
    {
        /// <summary>
        /// 更新点检项目/保养项目信息
        /// 更新点检项目的同时要更新
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public bool Update(base_examitem obj, IDbConnection dbConnection = null);
    }
}
