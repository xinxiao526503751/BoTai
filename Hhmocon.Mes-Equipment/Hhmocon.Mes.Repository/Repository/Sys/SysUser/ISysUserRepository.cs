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
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface ISysUserRepository
    {
        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_user> GetByName(string name, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null);

        /// <summary>
        /// 将sys_user列表转化成Node
        /// parent_id=Dept_id;
        /// 用来在生成树时和部门Nodes衔接
        /// </summary>
        /// <returns></returns>
        public List<TreeModel> ListElementToNodeLinkWithDept(List<sys_user> list);

    }
}
