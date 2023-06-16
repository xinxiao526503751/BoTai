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
    public class BaseEquipmentTypeApp
    {
        private readonly IBaseEquipmentTypeRepository _baseEquipmentTypeRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly BaseEquipmentSubApp _baseEquipmentSubApp;
        private readonly IAuth _auth;
        public BaseEquipmentTypeApp(IBaseEquipmentTypeRepository baseEquipmentTypeRepository, PikachuRepository pikachuRepository, PikachuApp pikachuApp, BaseEquipmentSubApp baseEquipmentSubApp, IAuth auth)
        {
            _baseEquipmentTypeRepository = baseEquipmentTypeRepository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _baseEquipmentSubApp = baseEquipmentSubApp;
            _auth = auth;
        }

        /// <summary>
        /// 添加设备类型信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_equipment_type InsertEquipment(base_equipment_type data)
        {
            //取ID
            data.equipment_type_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
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
        /// 设备类型页面的搜索框,根据type找type
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        public List<base_equipment_type> SearchBar(EquipmentTypeSearchBarRequest equipmentTypeSearchBarRequest)
        {

            List<base_equipment_type> base_Equipments = new();
            if (equipmentTypeSearchBarRequest.Type_Code == "" && equipmentTypeSearchBarRequest.Type_Name == "")
            {
                base_Equipments = _pikachuRepository.GetAll<base_equipment_type>();
            }

            if (equipmentTypeSearchBarRequest.EquipmentType_Id == null)//如果没选类型
            {
                //如果没填code，只填了name
                if (equipmentTypeSearchBarRequest.Type_Code == "" && equipmentTypeSearchBarRequest.Type_Name != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_type>()
                        .Where(c => c.equipment_type_name.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Name.ToLower()))
                        .ToList();
                }
                //如果没填Name，只填了Code
                if (equipmentTypeSearchBarRequest.Type_Name == "" && equipmentTypeSearchBarRequest.Type_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_type>()
                        .Where(c => c.equipment_type_code.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Code.ToLower()))
                        .ToList();
                }
                //如果Name，Code都填了
                if (equipmentTypeSearchBarRequest.Type_Name != "" && equipmentTypeSearchBarRequest.Type_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_type>()
                        .Where(c => c.equipment_type_code.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Code.ToLower())
                        &&
                        c.equipment_type_name.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Name.ToLower())
                        )
                        .ToList();
                }
            }
            else
            {
                //如果选中了类型
                //如果没填code，只填了name
                if (equipmentTypeSearchBarRequest.Type_Code == "" && equipmentTypeSearchBarRequest.Type_Name != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_type>()
                        .Where(c =>
                        c.equipment_type_id == equipmentTypeSearchBarRequest.EquipmentType_Id
                        &&
                        c.equipment_type_name.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Name.ToLower()))
                        .ToList();
                }
                //如果没填Name，只填了Code
                if (equipmentTypeSearchBarRequest.Type_Name == "" && equipmentTypeSearchBarRequest.Type_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_type>()
                        .Where(c =>
                        c.equipment_type_id == equipmentTypeSearchBarRequest.EquipmentType_Id
                        &&
                        c.equipment_type_code.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Code.ToLower()))
                        .ToList();
                }
                //如果Name，Code都填了
                if (equipmentTypeSearchBarRequest.Type_Name != "" && equipmentTypeSearchBarRequest.Type_Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_equipment_type>()
                        .Where(c => c.equipment_type_code.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Code.ToLower())
                        &&
                        c.equipment_type_name.ToLower().Contains(equipmentTypeSearchBarRequest.Type_Name.ToLower())
                        &&
                        c.equipment_type_id == equipmentTypeSearchBarRequest.EquipmentType_Id
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
