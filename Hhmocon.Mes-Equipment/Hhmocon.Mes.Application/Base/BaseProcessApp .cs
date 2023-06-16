using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;

namespace Hhmocon.Mes.Application
{
    public class BaseProcessApp
    {
        private readonly IBaseProcessRepository _baseProcessRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;


        public BaseProcessApp(IBaseProcessRepository repository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseProcessRepository = repository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加工序数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_process InsertBaseProcess(base_process data)
        {
            //取ID
            data.process_id = CommonHelper.GetNextGUID();
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

