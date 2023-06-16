using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseMaterialApp
    {
        private readonly IBaseMaterialRepository _Repository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;

        public BaseMaterialApp(IBaseMaterialRepository repository, PikachuRepository pikachuRepository, PikachuApp pikachuApp, IAuth auth)
        {
            _Repository = repository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _auth = auth;
        }

        /// <summary>
        /// 通过物料类型名称获取物料
        /// </summary>
        /// <param name="material_type_name"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<base_material> GetBaseMaterial_ByMaterialTypeName(string material_type_name, ref long icount)
        {

            string sql = string.Format(" material_type_name = '{0}'", material_type_name);

            //假查询部分
            sql = SqlAssemble.Delete_Mark(sql);


            //根据物料类型名称查找物料类型
            List<base_material_type> material_Types =
                _pikachuRepository.GetbySql<base_material_type>(sql);
            List<base_material> materials = new();

            foreach (base_material_type temp in material_Types)
            {
                //根据material_type_id得到 material_type_id=这个值 的物料列表
                List<base_material> materials_temp = _Repository.GetbyMaterialTypeId(temp.material_type_id);
                //统计物料类型名下物料的总数
                icount += materials_temp.Count;
                materials.AddRange(materials_temp);
            }
            return materials;
        }

        /// <summary>
        /// 根据物料类型id获取全部物料信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<base_material> GetBaseMaterial_ByMaterialTypeId(string material_type_id, ref long icount)
        {
            List<base_material> result = _pikachuRepository.GetByTypeId<base_material>(material_type_id);
            if (result != null)
            {
                icount = result.Count;
            }

            return result;
        }

        /// <summary>
        /// 插入基础物料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_material InsertBaseMaterial(base_material data)
        {
            //取ID
            data.material_id = CommonHelper.GetNextGUID();

            data.modified_time = Time.ShortDate.ToDate();
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);

            if (_Repository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 物料类型-物料页面搜索框
        /// 功能：搜索框可输入名称，根据选中的部门id和子部门查找name或者ch_name符合条件的用户(不区分大小写)
        /// 不选中时部门Id为null，在所有部门中找
        /// </summary>
        /// <returns></returns>
        public List<base_material> SearchBar(MaterialSearchBarRequest sysUserSearchBarRequest)
        {
            List<sys_user> sys_Users = new();
            //如果物料类型id为Null，在所有物料中查询然后分页
            if (sysUserSearchBarRequest.Type_Id == null)
            {
                return _pikachuRepository.GetAll<base_material>()
                    .Where(c => c.material_code.ToLower().Contains(sysUserSearchBarRequest.Code.ToLower())
                    || c.material_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower())
                    || c.material_en_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower()))
                    .Skip((sysUserSearchBarRequest.Page - 1) * sysUserSearchBarRequest.Rows)
                    .Take(sysUserSearchBarRequest.Rows).ToList();
            }

            return _pikachuRepository.GetAll<base_material>()
                    .Where(c => c.material_type_id == sysUserSearchBarRequest.Type_Id &&
                    (
                    c.material_code.ToLower().Contains(sysUserSearchBarRequest.Code.ToLower())
                    || c.material_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower())
                    || c.material_en_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower()))
                    )
                    .Skip((sysUserSearchBarRequest.Page - 1) * sysUserSearchBarRequest.Rows)
                    .Take(sysUserSearchBarRequest.Rows).ToList();
        }

    }
}
