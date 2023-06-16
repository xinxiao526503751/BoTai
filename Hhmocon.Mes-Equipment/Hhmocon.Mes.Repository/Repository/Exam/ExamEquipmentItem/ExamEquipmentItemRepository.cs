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

using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 设备-点检项关联表
    /// </summary>
    public class ExamEquipmentItemRepository : IExamEquipmentItemRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;

        public ExamEquipmentItemRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
        }
        public List<exam_equipment_item> GetByEquipmentId(string equipment_id)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByEquipmentId<exam_equipment_item>(equipment_id);
                List<exam_equipment_item> data = _pikachuRepository.GetbySql<exam_equipment_item>(sql);

                return data;
            }
        }
    }
}
