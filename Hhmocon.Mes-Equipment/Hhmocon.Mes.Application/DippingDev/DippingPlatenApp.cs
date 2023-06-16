using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Repository.DippingDev.DippingPlaten;
using hmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.DippingDev
{
    public class DippingPlatenApp
    {
        private PikachuRepository _pikachuRepository;
        private readonly IDippingPlantenRepository _IDippingPlantenRepository;

        public DippingPlatenApp(PikachuRepository pikachuRepository, IDippingPlantenRepository dippingPlantenRepository)
        {
            _pikachuRepository = pikachuRepository;
            _IDippingPlantenRepository = dippingPlantenRepository;
        }

        public dipping_platen_data Insert(dipping_platen_data data)
        {
            //取ID
            //data.dipping_platen_data_id = CommonHelper.GetNextGUID();
            //
            data.datetime = DateTime.Now;
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
        /// 返回最近加入的数据
        /// </summary>
        /// <returns></returns>
        public dipping_platen_data GetDataLast()
        {
            List<dipping_platen_data> platenDatas = _IDippingPlantenRepository.getDippingPlatenLatest();

            dipping_platen_data dataLast = platenDatas.LastOrDefault();
            return dataLast;
        }

        /// <summary>
        /// 需要list格式返回
        /// </summary>
        /// <returns></returns>
        public List<dipping_platen_data> GetDataLast2()
        {
            List<dipping_platen_data> platenDatas = _IDippingPlantenRepository.getDippingPlatenLatest();

            return platenDatas;
        }
    }
}
