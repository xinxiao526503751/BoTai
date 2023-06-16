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

using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository.Repository
{
    /// <summary>
    /// 点检项目仓储
    /// </summary>
    public class BaseExamitemRepository : IBaseExamitemRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        /// <summary>
        /// ctor
        /// </summary>
        public BaseExamitemRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 更新点检项目/保养项目信息
        /// 更新点检项目的同时要更新
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public bool Update(base_examitem obj, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            try
            {
                base_examitem _Examitem = _pikachuRepository.GetById<base_examitem>(obj.examitem_id, tran: transaction, dbConnection: conn);
                if (_Examitem != null)//如果能够根据id找到内容
                {
                    obj.examitem_code = _Examitem.examitem_code;//锁死code
                    obj.modified_time = DateTime.Now;        //更新修改时间
                    obj.create_time = _Examitem.create_time;//锁死创建时间
                }
                //更新设备点检项
                _pikachuRepository.Update(obj, tran: transaction, dbConnection: conn);

                List<exam_equipment_item> exam_Equipment_Items = _pikachuRepository.GetByOneFeildsSql<exam_equipment_item>("examitem_id", obj.examitem_id, tran: transaction, dbConnection: conn);
                foreach (exam_equipment_item temp in exam_Equipment_Items)
                {
                    temp.examitem_name = obj.examitem_name;
                    temp.examitem_std = obj.examitem_std;
                    temp.value_type = obj.value_type;
                    temp.description = obj.description;

                    //更新设备——项目关联表中的examitem_name等数据
                    _pikachuRepository.Update(temp, tran: transaction, dbConnection: conn);
                }

                List<exam_plan_method_item> exam_Plan_Method_Items = _pikachuRepository.GetByOneFeildsSql<exam_plan_method_item>("examitem_id", obj.examitem_id, tran: transaction, dbConnection: conn);
                foreach (exam_plan_method_item temp in exam_Plan_Method_Items)
                {
                    temp.examitem_name = obj.examitem_name;
                    temp.examitem_std = obj.examitem_std;
                    temp.value_type = obj.value_type;
                    temp.examitem_code = obj.examitem_code;
                    //关联表中的examitem_name等数据
                    _pikachuRepository.Update(temp, tran: transaction, dbConnection: conn);
                }

                List<exam_plan_rec_item> exam_Plan_Rec_Items = _pikachuRepository.GetByOneFeildsSql<exam_plan_rec_item>("examitem_id", obj.examitem_id, tran: transaction, dbConnection: conn);
                foreach (exam_plan_rec_item temp in exam_Plan_Rec_Items)
                {
                    temp.examitem_name = obj.examitem_name;
                    temp.examitem_std = obj.examitem_std;
                    temp.value_type = obj.value_type;
                    temp.examitem_code = obj.examitem_code;
                    //关联表中的examitem_name等数据
                    _pikachuRepository.Update(temp, tran: transaction, dbConnection: conn);
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }


    }
}
