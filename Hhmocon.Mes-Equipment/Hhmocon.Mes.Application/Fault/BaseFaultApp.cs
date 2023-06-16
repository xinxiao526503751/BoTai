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
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 事件定义App层
    /// </summary>
    public class BaseFaultApp
    {
        private readonly IBaseFaultRepository _baseFaultRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;

        public BaseFaultApp(IBaseFaultRepository baseFaultRepository, PikachuRepository pikachuRepository, PikachuApp pikachuApp, IAuth auth)
        {
            _baseFaultRepository = baseFaultRepository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _auth = auth;
        }

        /// <summary>
        /// 增加事件定义
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_fault InsertBaseFault(base_fault data)
        {
            //取ID
            data.fault_id = CommonHelper.GetNextGUID();
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
        /// 事件定义页面的搜索框
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        public List<base_fault> SearchBar(FaultSearchBarRequest faultSearchBarRequest)
        {
            List<base_fault> base_Equipments = new();
            if (faultSearchBarRequest.Code == "" && faultSearchBarRequest.Name == "")
            {
                base_Equipments = _pikachuRepository.GetAll<base_fault>();
            }

            if (faultSearchBarRequest.ClassId == null)//如果没选类型
            {
                //如果没填code，只填了name
                if (faultSearchBarRequest.Code == "" && faultSearchBarRequest.Name != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_fault>()
                        .Where(c => c.fault_name.ToLower().Contains(faultSearchBarRequest.Name.ToLower()))
                        .ToList();
                }
                //如果没填Name，只填了Code
                if (faultSearchBarRequest.Name == "" && faultSearchBarRequest.Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_fault>()
                        .Where(c => c.fault_code.ToLower().Contains(faultSearchBarRequest.Code.ToLower()))
                        .ToList();
                }
                //如果Name，Code都填了
                if (faultSearchBarRequest.Name != "" && faultSearchBarRequest.Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_fault>()
                        .Where(c => c.fault_code.ToLower().Contains(faultSearchBarRequest.Code.ToLower())
                        &&
                        c.fault_name.ToLower().Contains(faultSearchBarRequest.Name.ToLower())
                        )
                        .ToList();
                }
            }
            else
            {
                //如果选中了分类
                //如果没填code，只填了name
                if (faultSearchBarRequest.Code == "" && faultSearchBarRequest.Name != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_fault>()
                        .Where(c =>
                        c.fault_class_id == faultSearchBarRequest.ClassId
                        &&
                        c.fault_name.ToLower().Contains(faultSearchBarRequest.Name.ToLower()))
                        .ToList();
                }
                //如果没填Name，只填了Code
                if (faultSearchBarRequest.Name == "" && faultSearchBarRequest.Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_fault>()
                        .Where(c =>
                        c.fault_class_id == faultSearchBarRequest.ClassId
                        &&
                        c.fault_code.ToLower().Contains(faultSearchBarRequest.Code.ToLower()))
                        .ToList();
                }
                //如果Name，Code都填了
                if (faultSearchBarRequest.Name != "" && faultSearchBarRequest.Code != "")
                {
                    base_Equipments = _pikachuRepository.GetAll<base_fault>()
                        .Where(c => c.fault_code.ToLower().Contains(faultSearchBarRequest.Code.ToLower())
                        &&
                        c.fault_name.ToLower().Contains(faultSearchBarRequest.Name.ToLower())
                        &&
                        c.fault_class_id == faultSearchBarRequest.ClassId
                        )
                        .ToList();
                }
            }

            //long、name不能为Null
            //|| c.equipment_long_name.ToLower().Contains(equipmentTypeSearchBarRequest.Name.ToLower())
            //|| c.equipment_short_name.ToLower().Contains(equipmentTypeSearchBarRequest.Name.ToLower()))

            return base_Equipments.Skip((faultSearchBarRequest.Page - 1) * faultSearchBarRequest.Rows)
                    .Take(faultSearchBarRequest.Rows).ToList();
        }

        /// <summary>
        /// 通过事件类型id获取挂载的事件
        /// </summary>
        /// <param name="fault_class_id"></param>
        /// <returns></returns>
        public List<base_fault> GetFaultByFaultClassId(string fault_class_id)
        {
            return _baseFaultRepository.GetFaultByFaultClassId(fault_class_id);
        }

        /// <summary>
        /// 将list [base_fault]转换成list [TreeModel]
        /// 在转化成nodes后和base_fault_class的nodes衔接时用
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNodeLinkWithClass(List<base_fault> list)
        {
            return _baseFaultRepository.ListElementToNodeLinkWithClass(list);
        }

        /// <summary>
        /// 删除事件定义，可批量删除
        /// 如果有事件记录，则不允许删掉
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(string[] ids)
        {
            return _baseFaultRepository.Delete(ids);
        }
    }
}
