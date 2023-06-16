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

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 仓库位置仓储
    /// </summary>
    public class WareHouseLocRepository : IWareHouseLocRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pikachuRepository"></param>
        public WareHouseLocRepository(PikachuRepository pikachuRepository)
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
                //检查库存表有没有数据
                case "base_stock":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_warehouse_loc>("warehouse_loc_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;

                //检查库位表是不是只有一个该id
                case "base_warehouse_loc":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_warehouse_loc>("warehouse_loc_id", id).Count() > 1)
                        {
                            throw new Exception($"存在两个id={id}相同的库位");
                        }
                        Pass_flag++;
                    }
                    break;

                //仓库是库位的上一级，不需做检查
                case "base_warehouse":
                    {
                        Pass_flag++;
                    }
                    break;

                case "warehouse_io_rec":
                    {
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
        /// 删除库位
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void deleteWarehouseLoc(string[] ids, IDbConnection dbConnection = null)
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
                    base_warehouse_loc base_Warehouse_Loc = _pikachuRepository.GetById<base_warehouse_loc>(id, tran: transaction, dbConnection: conn);
                    if (base_Warehouse_Loc == null)
                    {
                        throw new Exception($"无效的库位id={id}");
                    }

                    //删除库位
                    List<base_warehouse_loc> base_Warehouse_Locs = _pikachuRepository.GetByOneFeildsSql<base_warehouse_loc>("warehouse_loc_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_warehouse_loc temp in base_Warehouse_Locs)
                    {
                        string[] s = new string[] { temp.warehouse_loc_id };
                        _pikachuRepository.Delete_Mask<base_warehouse_loc>(s, tran: transaction, dbConnection: conn);
                    }

                    //删库存
                    List<base_stock> base_Stocks = _pikachuRepository.GetByOneFeildsSql<base_stock>("warehouse_loc_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_stock temp in base_Stocks)
                    {
                        string[] s = new string[] { temp.warehouse_id };
                        _pikachuRepository.Delete_Mask<base_stock>(s, tran: transaction, dbConnection: conn);
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
