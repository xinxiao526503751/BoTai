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
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using System;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseExamitemTypeApp
    {
        private readonly IBaseExamitemTypeRepository _baseExamitemTypeRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public BaseExamitemTypeApp(IBaseExamitemTypeRepository baseExamitemTypeRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseExamitemTypeRepository = baseExamitemTypeRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加点检类型信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_examitem_type InsertExamitemType(base_examitem_type data)
        {
            //取ID
            data.examitem_type_id = CommonHelper.GetNextGUID();
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
