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
    /// 出入库类型App
    /// </summary>
    public class IoWareHouseTypeApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly IoWareHouseTypeRepository _ioWareHouseTypeRepository;
        private readonly IAuth _auth;
        public IoWareHouseTypeApp(PikachuRepository pikachuRepository, IoWareHouseTypeRepository ioWareHouseTypeRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _ioWareHouseTypeRepository = ioWareHouseTypeRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增入库类型
        /// </summary>
        /// <param name="base_Iowarehouse_Type"></param>
        /// <returns></returns>
        public bool CreateWareHouseTypeIn(base_iowarehouse_type obj)
        {
            //对name和code进行查重
            List<base_iowarehouse_type> exists =
            _pikachuRepository.GetAll<base_iowarehouse_type>().Where(c =>
                c.iowarehouse_type_name == obj.iowarehouse_type_name
                 || c.iowarehouse_type_code == obj.iowarehouse_type_code)
            .Where(c => c.io_type == 0).ToList();
            if (exists.Count > 0)
            {
                throw new Exception("name或code重复");
            }

            //取ID
            obj.iowarehouse_type_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            //新增入库类型
            obj.io_type = 0;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            return _pikachuRepository.Insert(obj);
        }


        /// <summary>
        /// 新增出库类型,对出库的name和code进行查重
        /// </summary>
        /// <param name="base_Iowarehouse_Type"></param>
        /// <returns></returns>
        public bool CreateWareHouseType(base_iowarehouse_type obj)
        {
            //对name和code进行查重
            List<base_iowarehouse_type> All_warehouseTypes = _pikachuRepository.GetAll<base_iowarehouse_type>().Where(c =>
              c.iowarehouse_type_name == obj.iowarehouse_type_name
               || c.iowarehouse_type_code == obj.iowarehouse_type_code).ToList();
            List<base_iowarehouse_type> exists = new();
            if (obj.io_type == 1)
            {
                exists = All_warehouseTypes.Where(c => c.io_type == 1).ToList();
            }

            if (obj.io_type == 0)
            {
                exists = All_warehouseTypes.Where(c => c.io_type == 0).ToList();
            }

            if (exists.Count > 0)
            {
                throw new Exception("name或code重复");
            }

            //取ID
            obj.iowarehouse_type_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            return _pikachuRepository.Insert(obj);
        }




    }
}
