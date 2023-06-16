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
using Hhmocon.Mes.Util.AutofacManager;

namespace Hhmocon.Mes.Repository
{
    public class BaseEquipmentClassRepository : IBaseEquipmentClassRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;

        public BaseEquipmentClassRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
        }

    }
}
