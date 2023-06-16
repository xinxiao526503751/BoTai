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

using Dapper;
using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 设备仓储
    /// </summary>
    public class BaseEquipmentRepository : IBaseEquipmentRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<BaseEquipmentRepository> _logger;

        public BaseEquipmentRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository, ILogger<BaseEquipmentRepository> logger)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }

        /// <summary>
        /// 根据location_id获取挂载在位置下的设备
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public List<base_equipment> GetByLocationId(string location_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            string sql = _sqlHelper.GetByLocationId<base_equipment>(location_id);
            List<base_equipment> data = _pikachuRepository.GetbySql<base_equipment>(sql);

            return data;
        }

        /// <summary>
        /// 通过Name获取单个
        /// </summary>
        /// <param name="equipment_name"></param>
        /// <returns></returns>
        public base_equipment GetByName(string equipment_name)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            base_equipment data = conn.GetByOneFeildsSql<base_equipment>("equipment_name", equipment_name).FirstOrDefault();
            return data;
        }

        // <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            int flag = 0;
            switch (chartName)
            {
                case "base_equipment":
                    {
                        List<base_equipment> equipment = _pikachuRepository.GetByOneFeildsSql<base_equipment>("equipment_id", id).ToList();

                        if (equipment.Count() > 1)
                        {
                            throw new Exception("出现两个相同id的设备");
                        }
                        flag++;
                    }

                    break;
                //设备关联表中 ，如果是父设备就要报错
                case "base_equipment_sub":
                    {
                        List<base_equipment_sub> base_Equipment_Subs = _pikachuRepository.GetByOneFeildsSql<base_equipment_sub>("equipment_id", id);
                        if (base_Equipment_Subs.Count() > 0)
                        {
                            throw new Exception("此设备含有子设备");
                        }
                        flag++;
                    }
                    break;
                //设备点检项目表
                case "exam_equipment_item":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<exam_equipment_item>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被设备点检项引用");
                        }
                        flag++;
                    }
                    break;
                //点检计划——设备关联表
                case "exam_plan_method_equipment":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<exam_plan_method_equipment>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被点检计划引用");
                        }
                        flag++;
                    }
                    break;
                //点检计划----点检项目关联表
                case "exam_plan_method_item":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<exam_plan_method_item>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被点检计划--点检项目关联引用");
                        }
                        flag++;
                    }
                    break;
                //生产报工表
                case "exam_plan_rec":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<exam_plan_rec>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被生产报工引用");
                        }
                        flag++;
                    }
                    break;
                //事件记录表
                case "fault_record":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<fault_record>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被事件记录引用");
                        }
                        flag++;
                    }
                    break;
                //计划工单表
                case "plan_work":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<plan_work>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被计划工单引用");
                        }
                        flag++;
                    }
                    break;
                //报工记录表
                case "plan_work_rpt":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<plan_work_rpt>("equipment_id", id).Count() > 0)
                        {
                            throw new Exception("此设备正在被报工记录引用");
                        }
                        flag++;
                    }
                    break;

            }
            if (flag > 0)
            {
                referenceCharts.Add(chartName);
            }
            else
            {
                throw new Exception("CheckChartIfExistsData出现未预设的表单");
            }
        }

        /// <summary>
        /// 删除设备同时，删除子设备关联表中数据
        /// </summary>
        /// <param name="ids">需要删除的设备id，支持多选</param>
        /// true,检查是否有引用，有的话报错，没的话删除</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(string[] ids)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction dbTransaction = conn.BeginTransaction();
            try
            {
                //删除设备
                List<base_equipment> listdata = conn.GetByIds<base_equipment>(ids, tran: dbTransaction).ToList();
                if (listdata.Count == 0)
                {
                    return false;
                }

                foreach (base_equipment temp in listdata)
                {
                    temp.delete_mark = 1;
                    conn.Update(temp, tran: dbTransaction);
                }
                //删除关联表数据
                foreach (string temp in ids)
                {
                    string sql = $"select * from base_equipment_sub where equipment_id = '{temp}' or equipment_sub_id = '{temp}'";

                    List<base_equipment_sub> base_Equipment_Sub = conn.Query<base_equipment_sub>(sql, temp, transaction: dbTransaction).ToList();
                    if (base_Equipment_Sub != null)
                    {
                        foreach (base_equipment_sub sub_temp in base_Equipment_Sub)
                        {
                            sub_temp.delete_mark = 1;
                            conn.Update(sub_temp, tran: dbTransaction);
                        }
                    }
                }
                //foreach (string temp in ids)
                //{
                //    List<base_equipment_sub> base_Equipment_Sub = conn.GetByOneFeildsSql<base_equipment_sub>("equipment_sub_id", temp, tran: dbTransaction);
                //    if (base_Equipment_Sub != null)
                //    {
                //        foreach (base_equipment_sub sub_temp in base_Equipment_Sub)
                //        {
                //            sub_temp.delete_mark = 1;
                //            conn.Update(sub_temp, tran: dbTransaction);
                //        }
                //    }
                //}
                dbTransaction.Commit();
                return true;
            }
            catch (Exception exception)
            {
                dbTransaction.Rollback();
                _logger.LogError(exception.Message);
                throw new Exception(exception.InnerException?.Message ?? exception.Message);
            }
        }

        /// <summary>
        /// 更新设备信息，需要同时更新子设备关联表
        /// </summary>
        /// <param name="obj">修改后的设备对象</param>
        /// <returns>bool值</returns>
        public bool Update(base_equipment obj)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            try
            {
                //找到需要被修改的设备
                base_equipment equipment = conn.GetById<base_equipment>(obj.equipment_id, tran: transaction);
                if (equipment == null)
                {
                    throw new Exception("找不到id对应的设备");
                }

                obj.equipment_code = equipment.equipment_code;//锁死code
                obj.create_time = equipment.create_time;//锁死创建时间
                obj.modified_time = Time.Now; //给定修改时间
                                              //修改设备
                conn.Update(obj, tran: transaction);
                //设备id在关联表中有可能作为主设备
                List<base_equipment_sub> rele_equipments = conn.GetByOneFeildsSql<base_equipment_sub>("id", obj.equipment_id, tran: transaction);
                if (rele_equipments.Count != 0)
                {
                    //更新关联表的主设备数据
                    foreach (base_equipment_sub temp in rele_equipments)
                    {
                        temp.equipment_code = obj.equipment_code;
                        temp.equipment_name = obj.equipment_name;
                        temp.process_id = obj.process_id;
                        temp.modified_time = obj.modified_time;
                        conn.Update(temp, tran: transaction);
                    }
                }
                //设备id在关联表中有可能作为从设备
                List<base_equipment_sub> rele_equipments_sub = conn.GetByOneFeildsSql<base_equipment_sub>("equipment_sub_id", obj.equipment_id, tran: transaction);
                if (rele_equipments_sub.Count != 0)
                {
                    //更新关联表的主设备数据
                    foreach (base_equipment_sub temp in rele_equipments_sub)
                    {
                        temp.equipment_sub_type_id = obj.equipment_type_id;
                        temp.modified_time = obj.modified_time;
                        conn.Update(temp, tran: transaction);
                    }
                }
                transaction.Commit();
                return true;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                _logger.LogError(exception.Message);
                return false;
            }
        }

        /// <summary>
        /// 通过设备Id获取第二层设备
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns>没有子设备会返回Null</returns>
        public List<base_equipment> GetSecondEquipById(string equipment_id)
        {
            List<base_equipment> data = new();
            using IDbConnection conn = SqlServerDbHelper.GetConn();

            List<base_equipment_sub> subs = conn.GetByOneFeildsSql<base_equipment_sub>("id", equipment_id);
            if (subs.Count == 0)//关联表中没有数据代表设备没有子设备
            {
                return null;
            }

            foreach (base_equipment_sub temp in subs)
            {
                base_equipment equip = _pikachuRepository.GetById<base_equipment>(temp.equipment_sub_id);//找到子设备
                data.Add(equip);
            }
            return data;
        }




    }
}
