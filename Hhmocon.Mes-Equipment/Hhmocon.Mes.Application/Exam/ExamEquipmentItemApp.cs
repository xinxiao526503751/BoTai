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

using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application.Exam
{
    public class ExamEquipmentItemApp
    {
        private readonly IExamEquipmentItemRepository _examEquipmentItemRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public ExamEquipmentItemApp(IExamEquipmentItemRepository examEquipmentItemRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _examEquipmentItemRepository = examEquipmentItemRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加设备-点检项关联
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string InsertExamEquipmentItem(eqp_exe eqp_Exe)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    List<exam_equipment_item> _base_Examitems_Item = _pikachuRepository.GetByTwoFeildsSql<exam_equipment_item>("equipment_id", eqp_Exe.equipment_id, "examitem_id", eqp_Exe.examitem_ids.ToList());
                    if (_base_Examitems_Item.Count != 0)
                    {
                        throw new Exception("点检项目重复添加");
                    }

                    foreach (string examitem_id in eqp_Exe.examitem_ids)
                    {
                        base_examitem _base_Examitems = _pikachuRepository.GetById<base_examitem>(examitem_id);
                        if (_base_Examitems == null)
                        {
                            throw new Exception("该点检信息不存在");
                        }

                        exam_equipment_item item = new();
                        item.examitem_std = _base_Examitems.examitem_std;
                        item.description = _base_Examitems.description;
                        item.method_type = _base_Examitems.method_type;
                        item.value_type = _base_Examitems.value_type;
                        item.examitem_name = _base_Examitems.examitem_name;
                        item.examitem_code = _base_Examitems.examitem_code;
                        item.equipment_id = eqp_Exe.equipment_id;
                        item.examitem_id = examitem_id;

                        item.method_type = eqp_Exe.method_type;

                        item.exam_equipment_item_id = CommonHelper.GetNextGUID();
                        item.modified_time = Time.Now;
                        item.create_time = DateTime.Now;
                        item.create_by = _auth.GetUserAccount(null);
                        item.create_by_name = _auth.GetUserName(null);
                        item.modified_by = _auth.GetUserAccount(null);
                        item.modified_by_name = _auth.GetUserName(null);
                        if (!_pikachuRepository.Insert(item, tran: transaction, dbConnection: conn))
                        {
                            return item.examitem_id;
                            throw new Exception("数据写入失败");

                        }
                    }
                    transaction.Commit();
                    return eqp_Exe.equipment_id;

                }


                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<exam_equipment_item> GetByEquipmentId(string equipment_id)
        {
            List<exam_equipment_item> data = _examEquipmentItemRepository.GetByEquipmentId(equipment_id);
            return data;
        }
    }
}
