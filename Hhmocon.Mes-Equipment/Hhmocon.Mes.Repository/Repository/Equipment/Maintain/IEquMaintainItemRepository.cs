using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.Repository.Equipment.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Repository.Equipment.Maintain
{
    public interface IEquMaintainItemRepository:IBaseRepository<EquMaintainItem>
    {
        Task<bool> DeleteByTwoId(string id1,string id2);
        Task<EquMaintainItem> FindByTwoId(string id1, string id2);
        Task<bool> UpdateWithTwoId(string id1, string id2, EquMaintainItem equMaintainItem);
    }
}
