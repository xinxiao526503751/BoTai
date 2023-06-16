using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using hmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.DippingDev
{

     public class DippingDevApp
    {
        private PikachuRepository _pikachuRepository;
        private IDippingDevRepository _dippingDevRepository;
        public DippingDevApp(IDippingDevRepository dippingDevRepository,PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
            _dippingDevRepository = dippingDevRepository;
        }

        public dipping_dev_data Insert(dipping_dev_data data)
        {
            //取ID
            //data.exam_plan_rec_id = CommonHelper.GetNextGUID();
            data.datatime = DateTime.Now;
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
        /// 根据条件获取分页数据，带总数统计
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<T> GetList<T>(PageReq req, ref long icount)
        {
            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;

            if (!string.IsNullOrEmpty(whereStr))
                whereStr = "WHERE " + whereStr;

            if (!string.IsNullOrEmpty(strOrder))
            {
                ordStr = strOrder;
                if (!string.IsNullOrEmpty(strSort))
                {
                    ordStr = "ORDER BY " + ordStr + " " + strSort;
                }
                else
                {
                    ordStr = "ORDER BY " + ordStr + " ";
                }
            }
 
            icount = _pikachuRepository.GetCount<T>(whereStr);
            return _pikachuRepository.GetList<T>(iPage, iRows, whereStr, ordStr);
        }

        /// <summary>
        /// 根据条件获取DippingDevs
        /// </summary>
        /// <param name="dippingQueryRequest"></param>
        /// <returns></returns>
        public List<dipping_dev_data> GetDippingDevs(DippingQueryRequest dippingQueryRequest)
        {
            return _dippingDevRepository.GetDippingDevs(dippingQueryRequest.EquipmentName, dippingQueryRequest.VariableCode, dippingQueryRequest.StartTime, dippingQueryRequest.EndTime);
        }

        public List<dipping_platen_data> GetPlatenDevs(DippingQueryRequest dippingQueryRequest)
        {
            return _dippingDevRepository.GetPlatenDevs(dippingQueryRequest.EquipmentName, dippingQueryRequest.VariableCode, dippingQueryRequest.StartTime, dippingQueryRequest.EndTime);
        }

        /// <summary>
        /// 获取最底纸面纸最新的一条数据
        /// </summary>
        /// <returns></returns>
        public List<dipping_dev_data> GetDippinggDataLast(string dippingDevCode)
        {
            List<dipping_dev_data> platenDatas = _dippingDevRepository.GetDippingDataLast(dippingDevCode).OrderBy(a => a.datatime).ToList();
            List<dipping_dev_data> dataLast = new();
            dataLast.Add(platenDatas.LastOrDefault());
            return dataLast;
        }
    }


}
