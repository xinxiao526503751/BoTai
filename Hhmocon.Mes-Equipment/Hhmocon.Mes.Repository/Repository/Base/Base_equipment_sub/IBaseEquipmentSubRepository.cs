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
    /// 设备子设备仓储接口
    /// </summary>
    public interface IBaseEquipmentSubRepository
    {
        /// <summary>
        /// 向关联表中添加子设备信息
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="sub_id"></param>
        /// <returns></returns>
        public bool InsertSubEquipment(string parent_id, List<string> sub_id);

        /// <summary>
        /// 根据id借助关联表查询到所有子节点
        /// </summary>
        /// <param name="root_id">根节点id</param>
        /// <returns>能根据root_id找到根或枝就返回list，找不到根就返回null</returns>
        public List<base_equipment_sub> GetRootAndBrunch(string root_id);

        /// <summary>
        /// 获取所有没有子节点的设备
        /// </summary>
        /// <returns></returns>
        public List<base_equipment> GetAllNoSonEquipment();

        /// <summary>
        /// 删除设备关联
        /// </summary>
        /// <param name="id">主设备id</param>
        /// <param name="sub_id">子设备id</param>
        /// <returns></returns>
        public bool Delete(string id, string sub_id);

        /// <summary>
        /// 通过equipment_sub_id获取
        /// </summary>
        /// <param name="equipment_sub_id"></param>
        /// <returns></returns>
        public base_equipment_sub GetByEquipmentSubId(string equipment_sub_id);

        /// <summary>
        /// 将 节点列表转化为树
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        public List<TreeModel> ListToTreeModel(List<TreeModel> Nodes, string parent_id = null);
    }
}
