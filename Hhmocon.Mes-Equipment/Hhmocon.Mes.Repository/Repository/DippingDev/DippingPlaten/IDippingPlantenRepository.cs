using hmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Repository.DippingDev.DippingPlaten
{
   public interface IDippingPlantenRepository
    {
        List<dipping_platen_data> getDippingPlatenLatest();
    }
}
