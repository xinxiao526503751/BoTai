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

using Hhmocon.Mes.Application;
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
    /// 设备子设备仓储
    /// </summary>
    public class BaseEquipmentSubRepository : IBaseEquipmentSubRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<base_equipment_sub> _logger;

        public BaseEquipmentSubRepository(PikachuRepository pikachuRepository, ILogger<base_equipment_sub> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }

        /// <summary>
        /// 向关联表中添加子设备信息
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="sub_id"></param>
        /// <returns></returns>
        public bool InsertSubEquipment(string parent_id, List<string> sub_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            base_equipment parent_equipment = _pikachuRepository.GetById<base_equipment>(parent_id);
            int iret = 0;

            //查找所有子设备关联
            List<base_equipment_sub> base_Equipment_Subs = _pikachuRepository.GetByOneFeildsSql<base_equipment_sub>("equipment_id", parent_id);
            List<string> sub_ids = new();
            foreach (base_equipment_sub temp in base_Equipment_Subs)
            {
                sub_ids.Add(temp.equipment_sub_id);
            }
             
            if (sub_ids == null)
            {
                throw new Exception("未传递需要添加的子设备");
            }
            //检查有无重复添加子设备
            foreach (string temp in sub_ids)
            {
                foreach (string temp1 in sub_id)
                {
                    if (temp == temp1)
                    {
                        base_equipment base_Equipment = new();
                        base_Equipment = _pikachuRepository.GetById<base_equipment>(temp);
                        throw new Exception($"{base_Equipment.equipment_name}是被重复添加的子设备");
                    }
                }
            }

            foreach (string temp in sub_id)
            {
                base_equipment_sub data = new();
                base_equipment sub_equipment = _pikachuRepository.GetById<base_equipment>(temp);
                if (sub_equipment == null)//如果传进来的不是设备id就忽略
                {
                    continue;
                }

                //取ID
                data.id = CommonHelper.GetNextGUID();
                data.modified_time = Time.Now;
                data.create_time = DateTime.Now;

                data.equipment_id = parent_id;
                data.equipment_name = parent_equipment.equipment_name;
                data.equipment_code = parent_equipment.equipment_code;
                data.process_id = parent_equipment.process_id;

                data.equipment_sub_id = temp;
                data.equipment_sub_type_id = sub_equipment.equipment_type_id;
                data.equipment_sub_name = sub_equipment.equipment_name;
                data.equipment_sub_process = sub_equipment.process_name;
                data.equipment_sub_process_id = sub_equipment.process_id;

                try
                {
                    iret = conn.Insert(data, transaction);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    return false;
                }

            }
            transaction.Commit();
            return iret > 0;
        }

        /// <summary>
        /// 根据id借助关联表查询到所有子节点,包含root
        /// 注意，关联表搞二叉树查询不能有三角关系
        /// </summary>
        /// <param name="root_id">根节点id</param>
        /// <returns>能根据root_id找到根或枝就返回list，找不到根就返回null</returns>
        public List<base_equipment_sub> GetRootAndBrunch(string root_id)
        {
            List<base_equipment_sub> subs = new();

            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //获取节点id的关联信息
            List<base_equipment_sub> root = new();
            root = conn.GetByOneFeildsSql<base_equipment_sub>("equipment_id", root_id);
            if (root.Count == 0)
            {
                return null;
            }

            subs.AddRange(root);
            foreach (base_equipment_sub temp in root)
            {
                List<base_equipment_sub> subs_temp = GetRootAndBrunch(temp.equipment_sub_id);
                if (subs_temp == null)
                {
                    continue;
                }

                subs.AddRange(subs_temp);
            }
            return subs;
        }

        /// <summary>
        /// 通过equipment_sub_id获取
        /// </summary>
        /// <param name="equipment_sub_id"></param>
        /// <returns></returns>
        public base_equipment_sub GetByEquipmentSubId(string equipment_sub_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            base_equipment_sub sub = conn.GetByOneFeildsSql<base_equipment_sub>("equipment_sub_id", equipment_sub_id).FirstOrDefault();
            return sub;
        }

        /// <summary>
        /// 获取所有没有子节点的设备
        /// </summary>
        /// <returns></returns>
        public List<base_equipment> GetAllNoSonEquipment()
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //获取所有设备
            List<base_equipment> equipments = conn.GetAll<base_equipment>().ToList();
            //获取所有关联表
            List<base_equipment_sub> subs = conn.GetAll<base_equipment_sub>().ToList();
            //遍历关联表
            foreach (base_equipment_sub temp in subs)
            {
                base_equipment equip_temp = equipments.Where(c => c.equipment_id.Equals(temp.equipment_id)).FirstOrDefault();
                //移除关联表中有记录的设备
                equipments.Remove(equip_temp);
            }
            return equipments;
        }

        /// <summary>
        /// 删除设备关联
        /// </summary>
        /// <param name="id">主设备id</param>
        /// <param name="sub_id">子设备id</param>
        /// <returns></returns>
        public bool Delete(string id, string sub_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();

            try
            {
                List<string> sub_id_list = new();
                sub_id_list.Add(sub_id);
                List<base_equipment_sub> subs = conn.GetByTwoFeildsSql<base_equipment_sub>("equipment_id", id, "equipment_sub_id", sub_id_list, tran: transaction).ToList();
                foreach (base_equipment_sub temp in subs)
                {
                    temp.delete_mark = 1;
                    conn.Update(temp, tran: transaction);
                }

                transaction.Commit();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                transaction.Rollback();
                throw new Exception(exception.InnerException?.Message ?? exception.Message);
            }
        }


        /// <summary>
        /// 将 节点列表转化为树
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        public List<TreeModel> ListToTreeModel(List<TreeModel> Nodes, string parent_id = null)
        {
            List<TreeModel> treeModels = new();

            //补充children_node
            foreach (TreeModel temp in Nodes)
            {
                if (string.IsNullOrEmpty(temp.parentId))
                {
                    continue;
                }
                else
                {
                    foreach (TreeModel temp2 in Nodes)
                    {

                        if (temp.parentId.Equals(temp2.id))
                        {
                            if (temp2.children == null)
                            {
                                temp2.children = new();
                            }

                            temp2.children.Add(temp);
                        }

                    }
                }
            }

            foreach (TreeModel temp in Nodes)
            {
                //树里面是只放根的.根的父节点默认为null,但也有可能为参数parent_id
                if (string.IsNullOrEmpty(temp.parentId) || temp.id.Equals(parent_id))
                {
                    treeModels.Add(temp);
                }
            }
            return treeModels;
        }
    }
}
