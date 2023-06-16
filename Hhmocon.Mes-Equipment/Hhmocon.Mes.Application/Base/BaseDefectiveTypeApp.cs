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

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 缺陷类型APP
    /// </summary>
    public class BaseDefectiveTypeApp
    {

        private readonly IBaseDefectiveTypeRepository _baseDefectiveTypeRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseDefectiveTypeApp(IBaseDefectiveTypeRepository IBaseDefectiveTypeRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseDefectiveTypeRepository = IBaseDefectiveTypeRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_defective_type Insert(base_defective_type data)
        {
            //取ID
            data.defective_type_id = CommonHelper.GetNextGUID();
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
