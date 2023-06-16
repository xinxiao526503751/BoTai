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
    /// 事件定义仓储接口
    /// </summary>
    public interface IBaseFaultRepository
    {
        /// <summary>
        /// 通过事件类型id获取挂载的事件
        /// </summary>
        /// <param name="fault_class_id"></param>
        /// <returns>List[base_fault]</returns>
        public List<base_fault> GetFaultByFaultClassId(string fault_class_id);


        /// <summary>
        /// 将list [base_fault]转换成list [TreeModel]
        /// 在转化为Nodes之后和base_fault_class的nodes衔接时用
        /// 将base_fault转化为Nodes后parent_id为class_id
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNodeLinkWithClass(List<base_fault> list);

        /// <summary>
        /// 删除事件定义，可批量删除
        /// 如果有事件记录，则不允许删掉
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(string[] ids);
    }
}
