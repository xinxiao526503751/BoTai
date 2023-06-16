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

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 生产任务子表仓储接口
    /// </summary>
    public class LituoProductionTaskDetailRepository
    {
        private readonly PikachuRepository _PikachuRepository;
        private readonly IAuth _auth;
        private readonly LituoProductionTaskDetailRepository _lituoProductionTaskDetailRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuRepository"></param>
        /// <param name="auth"></param>
        /// <param name="lituoProductionTaskDetailRepository"></param>
        public LituoProductionTaskDetailRepository(PikachuRepository pikachuRepository, IAuth auth, LituoProductionTaskDetailRepository lituoProductionTaskDetailRepository)
        {
            _PikachuRepository = pikachuRepository;
            _lituoProductionTaskDetailRepository = lituoProductionTaskDetailRepository;
            _auth = auth;
        }



    }
}
