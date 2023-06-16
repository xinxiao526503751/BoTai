using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;

namespace Hhmocon.Mes.Application
{
    public class BaseLocationTypeApp
    {
        private readonly IBaseLocationTypeRepository _Repository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;


        public BaseLocationTypeApp(IBaseLocationTypeRepository repository, PikachuRepository pikachuRepository, PikachuApp pikachuApp, IAuth auth)
        {
            _Repository = repository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _auth = auth;
        }


        /// <summary>
        /// 添加地点类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_location_type InsertBaseLoctionType(base_location_type data)
        {
            //取ID
            data.location_type_id = CommonHelper.GetNextGUID();
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
