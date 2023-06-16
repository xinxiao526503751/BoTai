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
    /// 事件通知配置仓储
    /// </summary>
    public class BaseFaultNoticecfgRepository : IBaseFaultNoticecfgRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<BaseFaultNoticecfgRepository> _logger;
        private readonly IAuth auth;
        public BaseFaultNoticecfgRepository(PikachuRepository pikachuRepository, ILogger<BaseFaultNoticecfgRepository> logger, IAuth _auth)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
            auth = _auth;
        }

        /// <summary>
        /// 根据事件ID和通知等级获取已经保存的通知配置
        /// </summary>
        /// <param name="fault_id">事件id</param>
        /// <param name="notice_level">通知等级</param>
        /// <returns></returns>
        public List<base_fault_noticecfg> GetByFaultIdAndNoticeLevel(string fault_id, string notice_level)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            List<string> fault_ids = new();
            fault_ids.Add(fault_id);
            List<base_fault_noticecfg> data = conn.GetByTwoFeildsSql<base_fault_noticecfg>("notice_level", notice_level, "fault_id", fault_ids).ToList();

            return data;
        }

        /// <summary>
        /// 插入事件配置
        /// </summary>
        /// <param name="t">事件</param>
        /// <param name="user_ids">用户Id</param>
        /// <returns></returns>
        public bool InsertBaseFaultNoticeCfg(base_fault_noticecfg t, List<string> user_ids)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();

            t.modified_time = Time.Now;
            t.create_time = DateTime.Now;
            t.create_by = auth.GetUserAccount(null);
            t.create_by_name = auth.GetUserName(null);
            t.modified_by = auth.GetUserAccount(null);
            t.modified_by_name = auth.GetUserName(null);

            try
            {
                foreach (string temp in user_ids)
                {
                    //取ID
                    t.fault_noticecfg_id = CommonHelper.GetNextGUID();
                    t.user_id = temp;
                    conn.Insert(t, transaction);
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


    }
}
