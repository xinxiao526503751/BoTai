using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using System;

namespace Hhmocon.Mes.Application
{
    public class BaseMaterialTypeApp
    {
        private readonly IBaseMaterialTypeRepository _Repository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseMaterialTypeApp(IBaseMaterialTypeRepository repository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _Repository = repository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 插入基础材料类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_material_type InsertBaseMaterialType(base_material_type data)
        {
            //取ID
            data.material_type_id = CommonHelper.GetNextGUID().ToLower();
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
