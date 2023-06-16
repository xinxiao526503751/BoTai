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

using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 设备子设备App层
    /// </summary>
    public class BaseEquipmentSubApp
    {
        private readonly IBaseEquipmentSubRepository _baseEquipmentSubRepository;
        private readonly PikachuRepository _pikachuRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public BaseEquipmentSubApp(IBaseEquipmentSubRepository baseEquipmentSubRepository, PikachuRepository pikachuRepository)
        {
            _baseEquipmentSubRepository = baseEquipmentSubRepository;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 向关联表中添加子设备信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertSubEquipment(string parent_id, List<string> sub_id)
        {
            return _baseEquipmentSubRepository.InsertSubEquipment(parent_id, sub_id);
        }

        /// <summary>
        /// 根据id借助关联表查询到所有子节点
        /// </summary>
        /// <param name="root_id">根节点id</param>
        /// <returns>能根据root_id找到根或枝就返回list，找不到根就返回null</returns>
        public List<base_equipment_sub> GetRootAndBrunch(string root_id)
        {
            List<base_equipment_sub> subs = _baseEquipmentSubRepository.GetRootAndBrunch(root_id);

            return subs;
        }

        /// <summary>
        /// 获取所有没有子节点的设备
        /// </summary>
        /// <returns></returns>
        public List<base_equipment> GetAllNoSonEquipment()
        {
            return _baseEquipmentSubRepository.GetAllNoSonEquipment();
        }

        /// <summary>
        /// 删除设备关联
        /// </summary>
        /// <param name="id">主设备id</param>
        /// <param name="sub_id">子设备id</param>
        /// <returns></returns>
        public bool Delete(string id, string sub_id)
        {
            return _baseEquipmentSubRepository.Delete(id, sub_id);
        }

        /// <summary>
        /// 将 节点列表转化为树
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        public List<TreeModel> ListToTreeModel(List<TreeModel> Nodes, string parent_id = null)
        {
            return _baseEquipmentSubRepository.ListToTreeModel(Nodes, parent_id);
        }
    }
}
