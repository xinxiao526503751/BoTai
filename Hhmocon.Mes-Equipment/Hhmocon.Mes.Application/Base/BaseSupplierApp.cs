using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;

namespace Hhmocon.Mes.Application
{
    public class BaseSupplierApp
    {
        private readonly IBaseSupplierRepository _baseSupplierRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public BaseSupplierApp(IBaseSupplierRepository baseSupplierRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseSupplierRepository = baseSupplierRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加供应商数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_supplier InserSuppliers(base_supplier data)
        {
            //取ID
            data.supplier_id = CommonHelper.GetNextGUID();
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
