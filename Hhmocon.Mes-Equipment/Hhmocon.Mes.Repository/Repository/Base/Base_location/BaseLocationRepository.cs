using Dapper;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository.Repository
{
    public class BaseLocationRepository : IBaseLocationRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pikachuRepository"></param>
        public BaseLocationRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
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
                case "base_equipment":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_equipment>("location_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }

                    break;
                case "sys_dept":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sys_dept>("location_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
                //查自己的时候，找有没有子地点
                case "base_location":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_location>("location_parentid", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
                case "plan_process":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<plan_process>("location_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
                case "base_warehouse":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_warehouse>("location_id", id).Count() > 0)
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
        /// 删除地点
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteLocation(string[] ids, IDbConnection dbConnection = null)
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
                    base_location location = _pikachuRepository.GetById<base_location>(id, tran: transaction, dbConnection: conn);
                    if (location == null)
                    {
                        throw new Exception($"无效的地点id={id}");
                    }
                    string[] dele = { location.location_id };
                    _pikachuRepository.Delete_Mask<base_location>(dele, tran: transaction, dbConnection: conn);

                    List<base_equipment> base_Equipments = _pikachuRepository.GetByOneFeildsSql<base_equipment>("location_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_equipment base_Equipment in base_Equipments)
                    {
                        string[] s = new string[] { base_Equipment.equipment_id };
                        _pikachuRepository.Delete_Mask<base_equipment>(s, tran: transaction, dbConnection: conn);
                    }

                    List<sys_dept> sys_Depts = _pikachuRepository.GetByOneFeildsSql<sys_dept>("location_id", id, tran: transaction, dbConnection: conn);
                    foreach (sys_dept temp in sys_Depts)
                    {
                        string[] s = new string[] { temp.dept_id };
                        _pikachuRepository.Delete_Mask<sys_dept>(s, tran: transaction, dbConnection: conn);
                    }

                    //根据部门Id找到底下所有部门
                    string sql =
                        $"With Tree As(" +
                        $"Select* from base_location where location_id = '{id}'" +
                        $" union all " +
                        $"Select* From base_location " +
                        $"Where location_parentid = '{id}'" + // 要查询的父 id
                        $"Union All " +                //union会自动压缩多个结果集合中的重复结果，union all将全部结果显示出来
                                                       //内连接父结果集和表，找到所有 parent_id = 父结果集中location_id的 数据
                        $" Select base_location.* From base_location, Tree" +//from 表名, 表名  其实就是内连接
                        $" Where base_location.location_parentid = Tree.location_id)" +
                        $"Select* From Tree;";
                    List<base_location> base_Locations = conn.Query<base_location>(sql, transaction: transaction).ToList();
                    foreach (base_location temp in base_Locations)
                    {
                        string[] s = new string[] { temp.location_id };
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
