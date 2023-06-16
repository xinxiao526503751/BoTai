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
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 订单细节仓储
    /// </summary>
    public class SaleOrderDetailRepository : ISaleOrderDetailRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<SaleOrderDetailRepository> _logger;
        public SaleOrderDetailRepository(PikachuRepository pikachuRepository, ILogger<SaleOrderDetailRepository> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }


        public List<sale_order_detail_rep> GetListByOrderId(string id, string whereStr)
        {
            //using (var conn = SqlServerDbHelper.GetConn())
            //{
            //    List<sale_order_detail> data = conn.GetByOneFeildsSql<sale_order_detail>("sale_order_id", orderId);
            //    return data;
            //}

            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr = " AND " + whereStr;
                }

                string query = null;
                query = "SELECT sod.*,bm.material_name " +
                         "FROM sale_order_detail sod LEFT JOIN  base_material bm  ON sod.material_id = bm.material_id" +
                         " WHERE sod.sale_order_id = @id AND sod.delete_mark = 0" + whereStr;

                List<sale_order_detail_rep> _Reps = new();
                List<sale_order_detail_rep> b = conn.Query<sale_order_detail, base_material, List<sale_order_detail_rep>>(query,
                    (saleOrderDetail, baseMterial) =>
                    {
                        sale_order_detail_rep _Order_Detail_Rep = new();

                        if (saleOrderDetail != null)
                        {
                            // _Order_Detail_Rep = saleOrderDetail;
                            _Order_Detail_Rep.barcode = saleOrderDetail.barcode;
                            _Order_Detail_Rep.bpack_qty = saleOrderDetail.bpack_qty;
                            _Order_Detail_Rep.check_by = saleOrderDetail.check_by;
                            _Order_Detail_Rep.check_time = saleOrderDetail.check_time;
                            _Order_Detail_Rep.create_by = saleOrderDetail.create_by;
                            _Order_Detail_Rep.create_by_name = saleOrderDetail.create_by_name;
                            _Order_Detail_Rep.create_time = saleOrderDetail.create_time;
                            _Order_Detail_Rep.create_time = saleOrderDetail.create_time;
                            _Order_Detail_Rep.delete_mark = saleOrderDetail.delete_mark;
                            _Order_Detail_Rep.is_checked = saleOrderDetail.is_checked;
                            _Order_Detail_Rep.is_finish = saleOrderDetail.is_finish;
                            _Order_Detail_Rep.is_planed = saleOrderDetail.is_planed;
                            _Order_Detail_Rep.material_id = saleOrderDetail.material_id;
                            _Order_Detail_Rep.modified_by_name = saleOrderDetail.modified_by_name;
                            _Order_Detail_Rep.modified_by = saleOrderDetail.modified_by;
                            _Order_Detail_Rep.modified_time = saleOrderDetail.modified_time;
                            _Order_Detail_Rep.out_qty = saleOrderDetail.out_qty;
                            _Order_Detail_Rep.price = saleOrderDetail.price;
                            _Order_Detail_Rep.qty = saleOrderDetail.qty;
                            _Order_Detail_Rep.remark = saleOrderDetail.remark;
                            _Order_Detail_Rep.sale_order_detail_code = saleOrderDetail.sale_order_detail_code;
                            _Order_Detail_Rep.sale_order_detail_id = saleOrderDetail.sale_order_detail_id;
                            _Order_Detail_Rep.sale_order_id = saleOrderDetail.sale_order_id;
                            _Order_Detail_Rep.spack_qty = saleOrderDetail.spack_qty;
                            _Order_Detail_Rep.totalmny = saleOrderDetail.totalmny;
                            _Order_Detail_Rep.delivery_date = saleOrderDetail.delivery_date;
                            _Order_Detail_Rep.unit = saleOrderDetail.unit;

                        }
                        if (baseMterial != null)
                        {
                            _Order_Detail_Rep.material_name = baseMterial.material_name;
                        }
                        _Reps.Add(_Order_Detail_Rep);
                        return _Reps;
                    }, new { id }, splitOn: "material_name").Distinct().SingleOrDefault();
                return b;//.Where(a=>a.sale_order_id==orderId).ToList();
            }
        }


    }
}
