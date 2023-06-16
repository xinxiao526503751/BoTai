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

using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseEquipmentClassApp
    {
        private readonly IBaseEquipmentClassRepository _baseEquipmentClassRepository;
        private readonly PikachuRepository _pikachuRepository;
        public BaseEquipmentClassApp(IBaseEquipmentClassRepository baseEquipmentClassRepository, PikachuRepository pikachuRepository)
        {
            _baseEquipmentClassRepository = baseEquipmentClassRepository;
            _pikachuRepository = pikachuRepository;
        }
        /// <summary>
        /// 添加设备分类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_equipment_class InsertEquipment(base_equipment_class data)
        {
            //取ID
            data.equipment_class_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
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
        /// 设备类型页面的搜索框,根据type找type
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        public List<base_equipment_class> SearchBar(EquipmentClassSearchBarRequest equipmentTypeSearchBarRequest)
        {

            List<base_equipment_class> base_Equipments = new();
            if (equipmentTypeSearchBarRequest.Class_Code == "" && equipmentTypeSearchBarRequest.Class_Name == "")
            {
                base_Equipments = _pikachuRepository.GetAll<base_equipment_class>();
            }

            if (equipmentTypeSearchBarRequest.EquipmentClass_Id == null)//如果没选类型
            {
                //如果没填code，只填了name
                if (equipmentTypeSearchBarRequest.Class_Code == "" && equipmentTypeSearchBarRequest.Class_Name != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_class>()
                        .Where(c => c.equipment_class_name.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Name.ToLower()))
                        .ToList();
                }
                //如果没填Name，只填了Code
                if (equipmentTypeSearchBarRequest.Class_Name == "" && equipmentTypeSearchBarRequest.Class_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_class>()
                        .Where(c => c.equipment_class_code.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Code.ToLower()))
                        .ToList();
                }
                //如果Name，Code都填了
                if (equipmentTypeSearchBarRequest.Class_Name != "" && equipmentTypeSearchBarRequest.Class_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_class>()
                        .Where(c => c.equipment_class_code.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Code.ToLower())
                        &&
                        c.equipment_class_name.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Name.ToLower())
                        )
                        .ToList();
                }
            }
            else
            {
                //如果选中了分类
                //如果没填code，只填了name
                if (equipmentTypeSearchBarRequest.Class_Code == "" && equipmentTypeSearchBarRequest.Class_Name != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_class>()
                        .Where(c =>
                        c.equipment_class_id == equipmentTypeSearchBarRequest.EquipmentClass_Id
                        &&
                        c.equipment_class_name.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Name.ToLower()))
                        .ToList();
                }
                //如果没填Name，只填了Code
                if (equipmentTypeSearchBarRequest.Class_Name == "" && equipmentTypeSearchBarRequest.Class_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_class>()
                        .Where(c =>
                        c.equipment_class_id == equipmentTypeSearchBarRequest.EquipmentClass_Id
                        &&
                        c.equipment_class_code.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Code.ToLower()))
                        .ToList();
                }
                //如果Name，Code都填了
                if (equipmentTypeSearchBarRequest.Class_Name != "" && equipmentTypeSearchBarRequest.Class_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_class>()
                        .Where(c => c.equipment_class_code.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Code.ToLower())
                        &&
                        c.equipment_class_name.ToLower().Contains(equipmentTypeSearchBarRequest.Class_Name.ToLower())
                        &&
                        c.equipment_class_id == equipmentTypeSearchBarRequest.EquipmentClass_Id
                        )
                        .ToList();
                }
            }

            //long、name不能为Null
            //|| c.equipment_long_name.ToLower().Contains(equipmentTypeSearchBarRequest.Name.ToLower())
            //|| c.equipment_short_name.ToLower().Contains(equipmentTypeSearchBarRequest.Name.ToLower()))

            return base_Equipments.Skip((equipmentTypeSearchBarRequest.Page - 1) * equipmentTypeSearchBarRequest.Rows)
                    .Take(equipmentTypeSearchBarRequest.Rows).ToList();
        }
    }
}
