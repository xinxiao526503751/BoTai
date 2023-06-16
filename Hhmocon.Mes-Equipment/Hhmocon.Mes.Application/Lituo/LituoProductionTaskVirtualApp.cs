using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Lituo
{
   public class LituoProductionTaskVirtualApp
    {
        private PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public LituoProductionTaskVirtualApp(PikachuRepository pikachuRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }
        public lituo_production_task_virtual Insert(lituo_production_task_virtual data)
        {
            Random rand = new Random();

            data.process_data_id = CommonHelper.GetNextGUID();
            data.create_time = DateTime.Now;
            data.create_time = DateTime.Now;
            data.unloading_process = rand.Next(255, 450);
            data.engraving_process = rand.Next(255, 450);
            data.platening_process = rand.Next(255, 450);
            data.welting_process = rand.Next(255, 450);
            data.packaging_process = rand.Next(255, 450);
            //data.create_by = _auth.GetUserAccount(null);
            //data.create_by_name = _auth.GetUserName(null);
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
