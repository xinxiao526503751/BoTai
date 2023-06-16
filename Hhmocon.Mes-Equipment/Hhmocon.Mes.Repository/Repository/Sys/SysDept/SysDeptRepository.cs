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
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 部门仓储
    /// </summary>
    public class SysDeptRepository : ISysDeptRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;

        public SysDeptRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }


        /// <summary>
        /// 根据Name获取,没有则返回Null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_dept> GetByName(string name)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            List<sys_dept> data = conn.GetByOneFeildsSql<sys_dept>("dept_name", name);
            if (data.Count == 0)
            {
                return null;
            }

            return data;
        }

        /// <summary>
        /// 在User表中通过Dept_id获取user
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        public List<sys_user> GetUserByDeptIdInUserChart(string dept_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            List<sys_user> data = conn.GetByOneFeildsSql<sys_user>("dept_id", dept_id);
            if (data.Count == 0)
            {
                return null;
            }

            return data;
        }

        /// <summary>
        /// 将list [sys_dept]转换成list [TreeModel]
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode(List<sys_dept> list)
        {
            List<TreeModel> treeModels = new();
            foreach (sys_dept temp in list)
            {
                TreeModel treeModel = new();

                treeModel.id = temp.dept_id;
                treeModel.label = temp.dept_name;
                treeModel.parentId = temp.parent_dept_id;
                treeModel.NodeType = "sys_dept";

                treeModels.Add(treeModel);
            }
            return treeModels;
        }


        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            int flag = 0;
            int Pass_flag = 0;
            switch (chartName)
            {
                case "sale_order":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sale_order>("dept_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }

                    break;
                case "sys_dept"://是部门表自己的时候就要查有没有下级
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sys_dept>("parent_dept_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
                case "sys_user":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sys_user>("dept_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
                case "sys_user_dept":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sys_user_dept>("dept_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
            }

            if (Pass_flag == 0)
            {
                throw new Exception($"CheckChartIfExistsData出现未预设的表单{chartName}");
            }
            if (flag > 0)
            {
                referenceCharts.Add(chartName);
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteDepartment(string[] ids, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            try
            {
                foreach (string id in ids)
                {
                    sys_dept dept = _pikachuRepository.GetById<sys_dept>(id, tran: transaction, dbConnection: conn);
                    if (dept == null)
                    {
                        throw new Exception($"无效的部门id={id},或者请求删除的部门下存在子部门");
                    }

                    List<sale_order> sale_Orders = _pikachuRepository.GetByOneFeildsSql<sale_order>("dept_id", id, tran: transaction, dbConnection: conn);
                    foreach (sale_order temp in sale_Orders)
                    {
                        string[] s = new string[] { temp.sale_order_id };
                        _pikachuRepository.Delete_Mask<sale_order>(s, tran: transaction, dbConnection: conn);
                    }


                    //根据部门Id找到底下所有部门
                    string sql =
                        $"With Tree As(" +
                        $"Select* from sys_dept where dept_id = '{id}'" +
                        $" union all " +
                        $"Select* From sys_dept " +
                        $"Where parent_dept_id = '{id}'" + // 要查询的父 id
                        $"Union All " +                //union会自动压缩多个结果集合中的重复结果，union all将全部结果显示出来
                                                       //内连接父结果集和表，找到所有 parent_id = 父结果集中location_id的 数据
                        $" Select sys_dept.* From sys_dept, Tree" +//from 表名, 表名  其实就是内连接
                        $" Where sys_dept.parent_dept_id = Tree.dept_id)" +
                        $"Select* From Tree;";
                    List<sys_dept> sys_Depts = conn.Query<sys_dept>(sql, transaction: transaction).ToList();//_pikachuRepository.GetByOneFeildsSql<sys_dept>("dept_id", id, tran: transaction, dbConnection: conn);
                    foreach (sys_dept temp in sys_Depts)
                    {
                        string[] s = new string[] { temp.dept_id };
                        _pikachuRepository.Delete_Mask<sys_dept>(s, tran: transaction, dbConnection: conn);
                    }


                    List<sys_user> sale_Users = _pikachuRepository.GetByOneFeildsSql<sys_user>("dept_id", id, tran: transaction, dbConnection: conn);
                    foreach (sys_user temp in sale_Users)
                    {
                        string[] s = new string[] { temp.dept_id };
                        _pikachuRepository.Delete_Mask<sys_user>(s, tran: transaction, dbConnection: conn);
                    }


                    List<sys_user_dept> sys_User_Depts = _pikachuRepository.GetByOneFeildsSql<sys_user_dept>("dept_id", id, tran: transaction, dbConnection: conn);
                    foreach (sys_user_dept temp in sys_User_Depts)
                    {
                        string[] s = new string[] { temp.dept_id };
                        _pikachuRepository.Delete_Mask<sys_user_dept>(s, tran: transaction, dbConnection: conn);
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

        }

    }
}
