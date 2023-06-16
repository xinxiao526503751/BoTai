using Dapper;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using hmocon.Mes.Repository.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    public class BaseBomRepository : IBaseBomRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        public BaseBomRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }
        public List<base_bom_response> QuertBomListResponse(string id, string whereStr)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                if (!string.IsNullOrEmpty(id))
                {
                    query = "SELECT bb.*,bm.material_name,bm.material_code,bm.material_spec " +
                         "FROM base_bom bb LEFT JOIN  base_material bm  ON bb.material_id = bm.material_id" +
                          " WHERE bb.material_id = @id  AND bb.DELETE_MARK = '0' " + whereStr;
                }
                else
                {
                    query = "SELECT bb.*,bm.material_name,bm.material_code,bm.material_spec " +
                           "FROM base_bom bb LEFT JOIN  base_material bm  ON bb.material_id = bm.material_id" +
                            "WHERE bb.DELETE_MARK = '0' " + whereStr;
                }

                List<base_bom_response> responseList = new();
                List<base_bom_response> b = conn.Query<base_bom, base_material, List<base_bom_response>>(query,
                        (baseBom, baseMterial) =>
                        {
                            base_bom_response _bomResponse = new();

                            if (baseBom != null)
                            {

                                _bomResponse.bom_id = baseBom.bom_id;
                                _bomResponse.bom_name = baseBom.bom_name;
                                _bomResponse.bom_code = baseBom.bom_code;
                                _bomResponse.yield = baseBom.yield;
                                _bomResponse.version = baseBom.version;
                                _bomResponse.weight = baseBom.weight;
                                _bomResponse.qty = baseBom.qty;
                                _bomResponse.unit = baseBom.unit;
                                _bomResponse.material_id = baseBom.material_id;
                                _bomResponse.parent_bom_id = baseBom.parent_bom_id;
                                _bomResponse.parent_bom_name = baseBom.parent_bom_name;
                            }

                            if (baseMterial != null)
                            {
                                _bomResponse.material_name = baseMterial.material_name;
                                _bomResponse.material_spec = baseMterial.material_spec;
                                _bomResponse.material_code = baseMterial.material_code;
                            }

                            responseList.Add(_bomResponse);
                            return responseList;
                        }, new { id }, splitOn: "material_name").Distinct().SingleOrDefault();
                return b;
            }
        }

        public List<base_bom_response> QuertBomListByParentId(string id)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                query = "SELECT bb.*,bm.material_name,bm.material_code " +
                       "FROM base_bom bb LEFT JOIN  base_material bm  ON bb.material_id = bm.material_id" +
                        " WHERE bb.parent_bom_id = @id  AND bb.DELETE_MARK = '0' ";
                List<base_bom_response> responseList = new();
                List<base_bom_response> b = conn.Query<base_bom, base_material, List<base_bom_response>>(query,
                    (baseBom, baseMterial) =>
                    {
                        base_bom_response _bomResponse = new();

                        if (baseBom != null)
                        {

                            _bomResponse.bom_id = baseBom.bom_id;
                            _bomResponse.bom_code = baseBom.bom_code;
                            _bomResponse.yield = baseBom.yield;
                            _bomResponse.version = baseBom.version;
                            _bomResponse.weight = baseBom.weight;
                            _bomResponse.parent_bom_id = baseBom.parent_bom_id;
                        }

                        if (baseMterial != null)
                        {
                            _bomResponse.material_name = baseMterial.material_name;
                            _bomResponse.material_code = baseMterial.material_code;
                        }

                        responseList.Add(_bomResponse);
                        return responseList;
                    }, new { id }, splitOn: "material_name").Distinct().SingleOrDefault();
                return b;
            }
        }
    }
}
