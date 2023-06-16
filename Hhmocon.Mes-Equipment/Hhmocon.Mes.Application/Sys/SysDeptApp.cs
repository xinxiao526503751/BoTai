
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 部门App
    /// </summary>
    public class SysDeptApp
    {
        private readonly ISysDeptRepository _sysDeptRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public SysDeptApp(ISysDeptRepository sysDeptRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _sysDeptRepository = sysDeptRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加部门数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Insert(sys_dept data)
        {
            //查重
            sys_dept getByCode = _pikachuRepository.GetByCode<sys_dept>(data.dept_code);
            List<sys_dept> getByName = _sysDeptRepository.GetByName(data.dept_name);

            if (getByName != null || getByCode != null)
            {
                //return false;
                throw new Exception("部门名字和编码不能重复");
            }

            //取ID
            data.dept_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            if (_pikachuRepository.Insert(data))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_dept> GetByName(string name)
        {
            List<sys_dept> sys_Depts = _sysDeptRepository.GetByName(name);
            return sys_Depts;
        }

        /// <summary>
        /// 在User表中通过dept_id获取user
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        public List<sys_user> GetUserByDeptIdInUserChart(string dept_id)
        {
            List<sys_user> sys_User = _sysDeptRepository.GetUserByDeptIdInUserChart(dept_id);
            return sys_User;
        }


        /// <summary>
        /// 将list [sys_dept]转换成list [TreeModel]
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode(List<sys_dept> list)
        {
            return _sysDeptRepository.ListElementToNode(list);
        }

        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _sysDeptRepository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }


        /// <summary>
        /// 动态泛型
        /// </summary>
        public void DynamicGeneric(string chartName, string id)
        {
            //根据表名获取类型
            Type type = _pikachuRepository.typen(chartName);
            //创建成功后虽然是object对象，但编译后var会被解析成对应的对象
            //var classType = type.Assembly.CreateInstance(type.FullName);

            //MethodInfo mi = GetType().GetMethod("method").MakeGenericMethod(new Type[] { t1, t2 });

            //获取所有程序集
            IEnumerable<Assembly> allAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load);
            //找到需要的程序集
            Assembly GoalAssemblies = allAssemblies.Where(m => m.FullName.Contains(".Application")).FirstOrDefault();
            Type utilType = GoalAssemblies.GetTypes().Where(t => t.Name == "PikachuApp").FirstOrDefault();

            MethodInfo method = utilType.GetMethod("GetByOneFeildsSql");
            sale_order sale_Order = new();
            type = sale_Order.GetType();
            method.MakeGenericMethod(new Type[] { type });
            object[] parameters = new object[] { "dept_id", id };
            object a = method.Invoke(utilType, parameters);
            //_pikachuApp.GetByOneFeildsSql<>("dept_id",);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        public void deleteDepartment(string[] ids, IDbConnection dbConnection = null)
        {
            _sysDeptRepository.DeleteDepartment(ids, dbConnection);
        }
    }
}
