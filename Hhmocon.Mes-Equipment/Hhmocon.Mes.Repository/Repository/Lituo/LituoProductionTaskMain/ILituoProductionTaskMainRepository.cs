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
    /// 生产任务主表仓储接口
    /// </summary>
    public interface ILituoProductionTaskMainRepository
    {
        /// <summary>
        /// 力拓指挥中心页面左上角订单进度的 已完成订单、正在加工订单、待加工订单、总订单数
        /// </summary>
        /// <returns></returns>
        public int[] TheNumberOfFourTypesOrder();

        /// <summary>
        /// 获取订单总数
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        public List<lituo_production_task_main> getTodayTaskOrders(string ProcessNbm);

        /// <summary>
        /// 获取已经完成的订单
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        public List<lituo_production_task_main> getfihTaskOrders(string ProcessNbm);

        /// <summary>
        /// 订单工序进度
        /// </summary>
        /// <returns></returns>
        public List<OrderOperationProgressResponse> OrderOperationProgress();


        /// <summary>
        /// 通过日期获取所有当天完成的订单数
        /// </summary>
        /// <returns></returns>
        public int GetProductionByDate(DateTime dateTime);

    }
}
