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

using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    public class BaseEquipmentApp
    {
        private readonly IBaseEquipmentRepository _baseEquipmentRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;
        public BaseEquipmentApp(IBaseEquipmentRepository baseEquipmentRepository, PikachuRepository pikachuRepository, PikachuApp pikachuApp, IAuth auth)
        {
            _baseEquipmentRepository = baseEquipmentRepository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _auth = auth;
        }

        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_equipment InsertEquipment(base_equipment data)
        {
            //取ID
            data.equipment_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.tested_time = "1970-01-01 00:00:00".ToDate();
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过Name获取单个
        /// </summary>
        /// <param name="equipment_name">设备名称</param>
        /// <returns></returns>
        public base_equipment GetByName(string equipment_name)
        {
            return _baseEquipmentRepository.GetByName(equipment_name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">被删除的设备id，支持多选</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(string[] ids)
        {
            return _baseEquipmentRepository.Delete(ids);
        }

        /// <summary>
        /// 更新设备信息，需要同时更新子设备关联表
        /// </summary>
        /// <param name="obj">修改后的设备对象</param>
        /// <returns>bool值</returns>
        public bool Update(base_equipment obj)
        {
            return _baseEquipmentRepository.Update(obj);
        }

        /// <summary>
        /// 通过设备Id获取第二层设备
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns>没有子设备会返回Null</returns>
        public List<base_equipment> GetSecondEquipById(string equipment_id)
        {
            return _baseEquipmentRepository.GetSecondEquipById(equipment_id);
        }


        /// <summary>
        /// 根据位置id获取位置下挂载的设备
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public List<base_equipment> GetByLocationId(string location_id)
        {
            return _baseEquipmentRepository.GetByLocationId(location_id);
        }

        /// <summary>
        /// 设备定义页面的搜索框
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        public List<base_equipment> SearchBar(EquipmentSearchBarRequest equipmentSearchBarRequest)
        {
            List<base_equipment> base_Equipments = new();
            if (equipmentSearchBarRequest.Location_Id == null)
            {
                return _pikachuRepository.GetAll<base_equipment>()
                    .Where(c => c.equipment_name.ToLower().Contains(equipmentSearchBarRequest.Name.ToLower())
                    || c.equipment_long_name.ToLower().Contains(equipmentSearchBarRequest.Name.ToLower())
                    || c.equipment_short_name.ToLower().Contains(equipmentSearchBarRequest.Name.ToLower()))
                    .Skip((equipmentSearchBarRequest.Page - 1) * equipmentSearchBarRequest.Rows)
                    .Take(equipmentSearchBarRequest.Rows).ToList();
            }
            //如果地点id不为null，以传入的部门id为根找到连锁的地点
            List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(equipmentSearchBarRequest.Location_Id);
            //遍历地点，根据地点id和传入的name在sys_user表查找用户
            foreach (base_location temp in base_Locations)
            {
                List<base_equipment> temp_users = _pikachuRepository.GetAll<base_equipment>()
                    .Where(c =>
                    c.location_id == temp.location_id
                    &&
                    c.equipment_name.ToLower().Contains(equipmentSearchBarRequest.Name.ToLower())
                    || c.equipment_long_name.ToLower().Contains(equipmentSearchBarRequest.Name.ToLower())
                    || c.equipment_short_name.ToLower().Contains(equipmentSearchBarRequest.Name.ToLower())).ToList();


                if (temp_users.Count == 0)
                {
                    continue;
                }

                base_Equipments.AddRange(temp_users);
            }
            return base_Equipments.Skip((equipmentSearchBarRequest.Page - 1) * equipmentSearchBarRequest.Rows)
                    .Take(equipmentSearchBarRequest.Rows).ToList();
        }

        /// <summary>
        /// 检查是否有表已经用到equipment_id = 参数id的数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _baseEquipmentRepository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }
    }
}
