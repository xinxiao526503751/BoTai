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
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 订单App
    /// </summary>
    public class SaleOrderApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly SaleOrderRepository _saleOrderRepository;
        private readonly IAuth _auth;
        public SaleOrderApp(PikachuRepository pikachuRepository, SaleOrderRepository saleOrderRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _saleOrderRepository = saleOrderRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <returns></returns>
        public bool Insert(sale_order obj)
        {
            //对订单编号进行查重
            List<sale_order> exists =
            _pikachuRepository.GetAll<sale_order>().Where(c =>
                c.sale_order_code == obj.sale_order_code
            ).ToList();
            if (exists.Count > 0)
            {
                throw new Exception("订单编号重复");
            }

            //取ID
            obj.sale_order_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.sale_order_date = obj.sale_order_date.ToDate();
            obj.check_time = obj.check_time.ToDate();
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            return (_pikachuRepository.Insert(obj));
        }

        public List<SaleOrderResponse> GetSaleOrderList(PageReq req, ref long icount)
        {

            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;

            List<SaleOrderResponse> data = _saleOrderRepository.GetSaleOrderList(whereStr);
            icount = data != null ? data.Count : 0;
            //分页
            data = data?.Skip((iPage - 1) * iRows).Take(iRows).ToList();

            return data;
        }

    }
}
