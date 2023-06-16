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
    /// 产品缺陷App
    /// </summary>
    public class BaseDefectiveReasonApp
    {
        private readonly IBaseDefectiveReasonRepository _baseDefectiveReasonRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseDefectiveReasonApp(IBaseDefectiveReasonRepository IBaseDefectiveReasonRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseDefectiveReasonRepository = IBaseDefectiveReasonRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_defective_reason Insert(base_defective_reason data)
        {
            //取ID
            data.defective_reason_id = CommonHelper.GetNextGUID();
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
