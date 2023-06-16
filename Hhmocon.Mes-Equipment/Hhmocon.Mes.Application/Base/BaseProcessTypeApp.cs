using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 工序类型App
    /// </summary>
    public class BaseProcessTypeApp
    {
        private readonly IBaseProcessTypeRepository _baseProcessTypeRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public BaseProcessTypeApp(IBaseProcessTypeRepository repository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseProcessTypeRepository = repository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加工序类型数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_process_type InsertBaseProcessType(base_process_type data)
        {
            //取ID
            data.process_type_id = CommonHelper.GetNextGUID();
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


    }
}

