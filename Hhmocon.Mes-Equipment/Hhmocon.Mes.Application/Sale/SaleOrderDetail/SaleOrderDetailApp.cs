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
    /// 订单明细App
    /// </summary>
    public class SaleOrderDetailApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly SaleOrderDetailRepository _saleOrderDetailRepository;
        private readonly IAuth _auth;
        public SaleOrderDetailApp(PikachuRepository pikachuRepository, SaleOrderDetailRepository SaleOrderDetailRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _saleOrderDetailRepository = SaleOrderDetailRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增物料订单
        /// </summary>
        /// <returns></returns>
        public bool Insert(sale_order_detail obj)
        {
            //对物料订单编号进行查重
            List<sale_order_detail> exists =
            _pikachuRepository.GetAll<sale_order_detail>().Where(c =>
                c.sale_order_detail_code == obj.sale_order_detail_code
            ).ToList();
            if (exists.Count > 0)
            {
                throw new Exception("物料订单编号重复");
            }

            //取ID
            obj.sale_order_detail_id = CommonHelper.GetNextGUID();
            obj.check_time = _pikachuRepository.GetById<sale_order>(obj.sale_order_id).check_time;
            obj.start_time = _pikachuRepository.GetById<sale_order>(obj.sale_order_id).start_time;
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);

            return (_pikachuRepository.Insert(obj));
        }

        /// <summary>
        /// 根据订单id获取详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="lcount"></param>
        /// <param name="ordeId"></param>
        /// <returns></returns>
        public List<sale_order_detail_rep> GetListByOrderId(PageReq req, ref long lcount, string ordeId)
        {

            string whereStr = CommonHelper.GetSqlConditonalStr(req.key);
            List<sale_order_detail_rep> data = _saleOrderDetailRepository.GetListByOrderId(ordeId, whereStr);
            lcount = data != null ? data.Count : 0;
            if (req != null)
            {
                int iPage = req.page;
                int iRows = req.rows;
                data = data?.Skip((iPage - 1) * iRows).Take(iRows).ToList();
            }
            return data;
        }
    }
}
