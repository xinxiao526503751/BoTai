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
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件类型仓储
    /// </summary>
    public class BaseFaultClassRepository : IBaseFaultClassRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<BaseFaultClassRepository> _logger;
        public BaseFaultClassRepository(PikachuRepository pikachuRepository, ILogger<BaseFaultClassRepository> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }

        /// <summary>
        /// 将list [base_fault_class]转换成list [TreeModel]
        /// 由于事件类型没有父子关系转化后Parent_id统一为null
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode(List<base_fault_class> list)
        {
            List<TreeModel> treeModels = new();
            foreach (base_fault_class temp in list)
            {
                TreeModel treeModel = new();

                treeModel.id = temp.fault_class_id;
                treeModel.label = temp.fault_class_name;
                treeModel.parentId = temp.fault_class_parentid;//事件类型只有一层，没有父子节点（狗东西又说有几层了、、by姜）

                treeModels.Add(treeModel);
            }
            return treeModels;
        }


    }
}
