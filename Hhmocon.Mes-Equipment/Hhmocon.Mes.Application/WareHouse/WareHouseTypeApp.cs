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
    /// 仓库类型App
    /// </summary>
    public class WareHouseTypeApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public WareHouseTypeApp(PikachuRepository pikachuRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增入库类型
        /// </summary>
        /// <returns></returns>
        public bool Insert(base_warehouse_type obj)
        {
            //对name和code进行查重
            List<base_warehouse_type> exists =
            _pikachuRepository.GetAll<base_warehouse_type>().Where(c =>
                c.warehouse_type_name == obj.warehouse_type_name
                 || c.warehouse_type_code == obj.warehouse_type_code).ToList();
            if (exists.Count > 0)
            {
                throw new Exception("name或code重复");
            }
            //取ID
            obj.warehouse_type_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            return (_pikachuRepository.Insert(obj));
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">被删除的设备id，支持多选</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(string[] ids)
        {
            return _pikachuRepository.Delete_Mask<base_warehouse_type>(ids);
        }

        /// <summary>
        /// 更新设备信息，需要同时更新子设备关联表
        /// </summary>
        /// <param name="obj">修改后的设备对象</param>
        /// <returns>bool值</returns>
        public bool Update(base_equipment obj)
        {
            return _pikachuRepository.Update(obj);
        }
    }
}
