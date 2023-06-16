using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Repository
{
    public interface IBaseMaterialRepository
    {
        //通过物料类型Id查找物料
        List<base_material> GetbyMaterialTypeId(string material_type_id);

        //通过物料类型来查询相关物料
        List<base_material> GetbyMaterialTypeCodes(string material_type_code);

        bool Insert(base_material data);
    }
}
