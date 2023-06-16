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
using System.Data;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseExamitemApp
    {
        private readonly IBaseExamitemRepository _baseExamitemRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public BaseExamitemApp(IBaseExamitemRepository baseExamitemRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseExamitemRepository = baseExamitemRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加点检信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_examitem InsertExamitem(base_examitem data)
        {
            //取ID
            data.examitem_id = CommonHelper.GetNextGUID();
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

        /// <summary>
        /// 更新点检项目/保养项目信息
        /// 更新点检项目的同时要更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(base_examitem obj, IDbConnection dbConnection = null)
        {
            return _baseExamitemRepository.Update(obj, dbConnection);
        }


    }
}
