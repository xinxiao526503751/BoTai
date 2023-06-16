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
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 订单仓储
    /// </summary>
    public class SaleOrderRepository : ISaleOrderRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<SaleOrderRepository> _logger;
        public SaleOrderRepository(PikachuRepository pikachuRepository, ILogger<SaleOrderRepository> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }

        public List<SaleOrderResponse> GetSaleOrderList(string whereStr)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                //if (!string.IsNullOrEmpty(id)) //特定地点的搜索
                //    query = "SELECT pwr.*,be.equipment_name,be.equipment_code,bm.material_name,bp.process_name " +
                //         "FROM plan_work_rpt pwr LEFT JOIN  base_equipment be  ON pwr.equipment_id = be.equipment_id" +
                //           "LEFT JOIN  base_material bm  ON pwr.material_id = bm.material_id" +
                //          "LEFT JOIN  base_process bp  ON pwr.process_id = bp.process_id WHERE pwr.location_id = @id " + whereStr;
                ////全部地点的搜索
                if (!string.IsNullOrEmpty(whereStr))
                {
                    query = "SELECT so.*,sd.dept_name,bs.supplier_name " +
                         "FROM sale_order so LEFT JOIN  sys_dept sd  ON so.dept_id = sd.dept_id " +
                           "LEFT JOIN   base_supplier bs  ON so.supplier_id = bs.supplier_id " +
                           "WHERE so.DELETE_MARK = '0' AND " + whereStr;
                }
                else
                {
                    query = "SELECT so.*,sd.dept_name,bs.supplier_name " +
                         "FROM sale_order so LEFT JOIN  sys_dept sd  ON so.dept_id = sd.dept_id " +
                           "LEFT JOIN   base_supplier bs  ON so.supplier_id = bs.supplier_id " +
                           "WHERE so.DELETE_MARK = '0'";
                }

                List<SaleOrderResponse> _saleOrderResponseList = new();
                List<SaleOrderResponse> b = conn.Query<sale_order, sys_dept, base_supplier, List<SaleOrderResponse>>(query,
                    (saleOrder, sysDept, baseSupplier) =>
                    {
                        SaleOrderResponse _saleOrderResponse = new();

                        if (saleOrder != null)
                        {

                            _saleOrderResponse.dept_id = saleOrder.dept_id;
                            _saleOrderResponse.check_by = saleOrder.check_by;
                            _saleOrderResponse.check_time = saleOrder.check_time;
                            _saleOrderResponse.create_by_name = saleOrder.create_by_name;
                            _saleOrderResponse.create_time = saleOrder.create_time;
                            _saleOrderResponse.delete_mark = saleOrder.delete_mark;
                            _saleOrderResponse.is_checked = saleOrder.is_checked;
                            _saleOrderResponse.is_finish = saleOrder.is_finish;
                            _saleOrderResponse.is_planed = saleOrder.is_planed;
                            _saleOrderResponse.modified_by = saleOrder.modified_by;
                            _saleOrderResponse.modified_by_name = saleOrder.modified_by_name;
                            _saleOrderResponse.modified_time = saleOrder.modified_time;
                            _saleOrderResponse.remark = saleOrder.remark;
                            _saleOrderResponse.sale_man = saleOrder.sale_man;
                            _saleOrderResponse.sale_order_code = saleOrder.sale_order_code;
                            _saleOrderResponse.sale_order_date = saleOrder.sale_order_date.ToDate();
                            _saleOrderResponse.sale_order_id = saleOrder.sale_order_id;
                            _saleOrderResponse.start_time = saleOrder.start_time;
                            _saleOrderResponse.supplier_id = saleOrder.supplier_id;

                        }
                        if (sysDept != null)
                        {
                            _saleOrderResponse.dept_name = sysDept.dept_name;
                        }
                        if (baseSupplier != null)
                        {
                            _saleOrderResponse.supplier_name = baseSupplier.supplier_name;
                        }
                        _saleOrderResponseList.Add(_saleOrderResponse);
                        return _saleOrderResponseList;
                    }, splitOn: "dept_name,supplier_name").Distinct().SingleOrDefault();
                return b;
            }
        }
    }
}
