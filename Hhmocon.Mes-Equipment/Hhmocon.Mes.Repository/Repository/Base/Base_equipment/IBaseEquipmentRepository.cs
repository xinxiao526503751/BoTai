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
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 设备仓储接口
    /// </summary>
    public interface IBaseEquipmentRepository
    {
        /// <summary>
        /// 根据location_id获取equipment
        /// </summary>
        /// <returns></returns>
        public List<base_equipment> GetByLocationId(string location_id);

        /// <summary>
        /// 通过Name获取单个
        /// </summary>
        /// <param name="equipment_name">设备名称</param>
        /// <returns></returns>
        public base_equipment GetByName(string equipment_name);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="checkReference"></param>
        /// <returns></returns>
        public bool Delete(string[] ids);

        /// <summary>
        /// 更新设备信息，需要同时更新子设备关联表
        /// </summary>
        /// <param name="obj">修改后的设备对象</param>
        /// <returns>bool值</returns>
        public bool Update(base_equipment obj);

        /// <summary>
        /// 通过设备Id获取第二层设备
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns>没有子设备会返回Null</returns>
        public List<base_equipment> GetSecondEquipById(string equipment_id);

        // <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName);
    }
}
