using Hhmocon.Mes.Application.Equipment.Base;
using Hhmocon.Mes.Repository.Domain.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.Maintain
{
    public interface IEquMaintainItemApp:IBaseApp<EquMaintainItem>
    {
        Task<bool> DeleteByTwoId(string id1, string id2);
        Task<EquMaintainItem> FindByTwoId(string id1, string id2);
        Task<bool> UpdateWithTwoId(string id1, string id2, EquMaintainItem equMaintainItem);
    }
}
