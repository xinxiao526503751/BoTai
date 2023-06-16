using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseBomDetailApp
    {
        private readonly IBaseDetailBomRespository _baseBomDetailRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseBomDetailApp(IBaseDetailBomRespository IbaseBomDetailRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseBomDetailRepository = IbaseBomDetailRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        public base_bom_detail Insert(base_bom_detail data)
        {
            //取ID
            data.bom_id = CommonHelper.GetNextGUID();
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
        /// 根据bom获取bom明细
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<base_bom_detail_response> GetBomDetailLiistByBomId(string bomId, PageReq req, ref long icount)
        {

            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;

            List<base_bom_detail_response> data = _baseBomDetailRepository.QuertBomDetailListResponse(bomId, whereStr);
            icount = data != null ? data.Count : 0;
            //分页
            data = data != null ? data.Skip((iPage - 1) * iRows).Take(iRows).ToList() : null;

            return data;
        }
    }
}
