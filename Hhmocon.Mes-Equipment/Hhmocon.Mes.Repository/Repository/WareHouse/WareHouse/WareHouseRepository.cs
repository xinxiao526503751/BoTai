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
    /// 仓储定义
    /// </summary>
    public class WareHouseRepository : IWareHouseRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;

        public WareHouseRepository(PikachuRepository pikachuRepository)
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
                case "warehouse_io_rec":
                    {
                        Pass_flag++;
                    }

                    break;
                //检查库存表有没有数据
                case "base_stock":
                    {
                        //if (_pikachuRepository.GetByOneFeildsSql<base_stock>("warehouse_id", id).Count() > 0)
                        //{
                        //    flag++;
                        //}
                        Pass_flag++;
                    }
                    break;
                case "base_warehouse":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_warehouse_loc>("warehouse_id", id).Count() > 1)
                        {
                            throw new Exception("存在两个相同Id的库存");
                        }
                        Pass_flag++;
                    }
                    break;
                case "base_warehouse_loc":
                    {
                        base_warehouse base_Warehouse = _pikachuRepository.GetById<base_warehouse>(id);
                        if (_pikachuRepository.GetByOneFeildsSql<base_warehouse_loc>("warehouse_id", id).Count() > 0)
                        {
                            throw new Exception($"仓库{base_Warehouse.warehouse_name}下存在库位，操作失败");
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
        /// 删除仓库
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteWareHouse(string[] ids, IDbConnection dbConnection = null)
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
                    base_warehouse dept = _pikachuRepository.GetById<base_warehouse>(id, tran: transaction, dbConnection: conn);
                    if (dept == null)
                    {
                        throw new Exception($"无效的仓库id={id}");
                    }

                    //删库存
                    List<base_stock> base_Stocks = _pikachuRepository.GetByOneFeildsSql<base_stock>("warehouse_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_stock temp in base_Stocks)
                    {
                        string[] s = new string[] { temp.warehouse_id };
                        _pikachuRepository.Delete_Mask<base_stock>(s, tran: transaction, dbConnection: conn);
                    }


                    //删除库位
                    List<base_warehouse_loc> base_warehouse_loc = _pikachuRepository.GetByOneFeildsSql<base_warehouse_loc>("warehouse_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_warehouse_loc temp in base_warehouse_loc)
                    {
                        string[] s = new string[] { temp.warehouse_loc_id };
                        _pikachuRepository.Delete_Mask<base_warehouse_loc>(s, tran: transaction, dbConnection: conn);
                    }

                    //删除仓库
                    List<base_warehouse> base_Warehouses = _pikachuRepository.GetByOneFeildsSql<base_warehouse>("warehouse_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_warehouse temp in base_Warehouses)
                    {
                        string[] s = new string[] { temp.warehouse_id };
                        _pikachuRepository.Delete_Mask<base_warehouse>(s, tran: transaction, dbConnection: conn);
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
