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
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository.Repository.Sys.SysParmType
{
    /// <summary>
    /// 单位类型仓储
    /// </summary>
    public class SysParmTypeRepository : ISysParmTypeRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        public SysParmTypeRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dbConnection"></param>
        public void DeleteParmType(string[] ids, IDbConnection dbConnection = null)
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
                    sys_parm_type sys_Parm_Type = _pikachuRepository.GetById<sys_parm_type>(id, tran: transaction, dbConnection: conn);
                    if (sys_Parm_Type == null)
                    {
                        throw new Exception($"无效的参数类型id={id}");
                    }

                    List<sys_parm_type> sys_Parm_Types = _pikachuRepository.GetByOneFeildsSql<sys_parm_type>("parm_type_id", id, tran: transaction, dbConnection: conn);
                    foreach (sys_parm_type temp in sys_Parm_Types)
                    {
                        string[] s = new string[] { temp.parm_type_id };
                        _pikachuRepository.Delete_Mask<sale_order>(s, tran: transaction, dbConnection: conn);
                    }

                    List<sys_parm> sys_Parms = _pikachuRepository.GetByOneFeildsSql<sys_parm>("parm_type_id", id, tran: transaction, dbConnection: conn);
                    foreach (sys_parm temp in sys_Parms)
                    {
                        string[] s = new string[] { temp.parm_type_id };
                        _pikachuRepository.Delete_Mask<sys_parm>(s, tran: transaction, dbConnection: conn);
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

        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            int flag = 0;
            switch (chartName)
            {
                case "sys_parm":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sys_parm>("parm_type_id", id).Count() > 0)
                        {
                            flag++;
                        }
                    }

                    break;
                case "sys_parm_type":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<sys_parm_type>("parm_type_id", id).Count() > 0)
                        {
                            flag++;
                        }
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
    }
}
