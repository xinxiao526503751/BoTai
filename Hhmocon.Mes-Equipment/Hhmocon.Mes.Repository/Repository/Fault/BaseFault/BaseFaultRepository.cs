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
    /// 事件定义仓储
    /// </summary>
    public class BaseFaultRepository : IBaseFaultRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<BaseFaultRepository> _logger;
        public BaseFaultRepository(PikachuRepository pikachuRepository, ILogger<BaseFaultRepository> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }

        /// <summary>
        /// 通过事件类型id获取挂载的事件
        /// </summary>
        /// <param name="fault_class_id"></param>
        /// <returns>List[base_fault]</returns>
        public List<base_fault> GetFaultByFaultClassId(string fault_class_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            List<base_fault> data = conn.GetByOneFeildsSql<base_fault>("fault_class_id", fault_class_id);

            return data.OrderBy(c => c.create_time).ToList();
        }

        /// <summary>
        /// 将list [base_fault]转换成list [TreeModel]
        /// 在转化为Nodes之后和base_fault_class的nodes衔接时用
        /// 将base_fault转化为Nodes后parent_id为class_id
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNodeLinkWithClass(List<base_fault> list)
        {
            List<TreeModel> treeModels = new();
            foreach (base_fault temp in list)
            {
                TreeModel treeModel = new();

                treeModel.id = temp.fault_id;
                treeModel.label = temp.fault_name;
                treeModel.parentId = temp.fault_class_id;//事件无父子关系,父节点id应为类型id

                treeModels.Add(treeModel);
            }
            return treeModels;
        }

        /// <summary>
        /// 删除事件定义，可批量删除
        /// 如果有事件记录，则不允许删掉
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
                List<string> chartNames = _pikachuRepository.GetAllChartNameHavingSameField("fault_id", tran: transaction, dbConnection: conn);
                List<string> chartExistsData = new();
                foreach (string id in ids)
                {
                    foreach (string chart in chartNames)
                    {
                        CheckChartIfExistsData(id, chart);
                    }
                }
                _pikachuRepository.Delete_Mask<base_fault>(ids, tran: transaction, dbConnection: conn);

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
                case "base_fault":
                    {
                        List<base_fault> base_Faults = _pikachuRepository.GetByOneFeildsSql<base_fault>("fault_id", id).ToList();

                        if (base_Faults.Count() > 1)
                        {
                            throw new Exception("出现两个相同id的事件定义");
                        }
                    }

                    break;

                //事件通知配置表
                case "base_fault_noticecfg":
                    {
                        List<base_fault_noticecfg> base_Fault_Noticecfgs = _pikachuRepository.GetByOneFeildsSql<base_fault_noticecfg>("fault_id", id).ToList();
                        if (base_Fault_Noticecfgs.Count() > 0)
                        {
                            throw new Exception("此事件正在被事件通知配置使用");
                        }
                    }
                    break;
                case "fault_record":
                    {
                        List<fault_record> fault_Records = _pikachuRepository.GetByOneFeildsSql<fault_record>("fault_id", id).ToList();
                        if (fault_Records.Count() > 0)
                        {
                            throw new Exception("此事件定义正在被事件记录使用");
                        }
                    }
                    break;
                case "fault_record_flow":
                    {
                        List<fault_record_flow> fault_Record_Flows = _pikachuRepository.GetByOneFeildsSql<fault_record_flow>("fault_id", id).ToList();
                        if (fault_Record_Flows.Count() > 0)
                        {
                            throw new Exception("此事件还有事件记录包含未处理的流程");
                        }
                    }
                    break;

                    throw new Exception("CheckChartIfExistsData出现未预设的表单");
            }
        }

    }
}
