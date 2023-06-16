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

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 事件类型App层
    /// </summary>
    public class BaseFaultClassApp
    {
        private readonly IBaseFaultClassRepository _baseFaultClassRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public BaseFaultClassApp(IBaseFaultClassRepository baseFaultClassRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseFaultClassRepository = baseFaultClassRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 增加事件类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_fault_class InsertBaseFaultClass(base_fault_class data)
        {
            //取ID
            data.fault_class_id = CommonHelper.GetNextGUID();
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
        /// 将list [base_fault_class]转换成list [TreeModel]
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode(List<base_fault_class> list)
        {
            return _baseFaultClassRepository.ListElementToNode(list);
        }

        /// <summary>
        /// 删除事件类型，可批量删除
        /// 如果有事件定义，则不允许删掉
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(string[] ids)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            try
            {
                //获取所有用到 事件定义表的id 的表名
                List<string> chartNames = _pikachuRepository.GetAllChartNameHavingSameField("fault_class_id", tran: transaction, dbConnection: conn);
                List<string> chartExistsData = new();
                foreach (string id in ids)
                {
                    foreach (string chart in chartNames)
                    {
                        CheckChartIfExistsData(id, chart);
                    }
                }
                _pikachuRepository.Delete_Mask<base_fault_class>(ids, tran: transaction, dbConnection: conn);

                transaction.Commit();
                return true;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new(exception.Message);
            }
        }

        // <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(string id, string chartName)
        {
            switch (chartName)
            {
                case "base_fault_class":
                    {
                        List<base_fault_class> base_Fault_Classs = _pikachuRepository.GetByOneFeildsSql<base_fault_class>("fault_class_id", id).ToList();

                        if (base_Fault_Classs.Count() > 1)
                        {
                            throw new Exception("出现两个相同id的事件定义");
                        }
                    }

                    break;

                //事件通知配置表
                case "base_fault":
                    {
                        List<base_fault> base_Faults = _pikachuRepository.GetByOneFeildsSql<base_fault>("fault_id", id).ToList();
                        if (base_Faults.Count() > 0)
                        {
                            throw new Exception("此事件类型正在被事件定义使用");
                        }
                    }
                    break;


                    throw new Exception("CheckChartIfExistsData出现未预设的表单");
            }
        }


    }
}
