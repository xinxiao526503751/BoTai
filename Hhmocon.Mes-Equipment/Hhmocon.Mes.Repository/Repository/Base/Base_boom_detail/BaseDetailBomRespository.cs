using Dapper;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    public class BaseDetailBomRespository : IBaseDetailBomRespository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;


        public BaseDetailBomRespository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 根据bom获取bom明细数据
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public List<base_bom_detail> GetListByBomId(string bomId)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<base_bom_detail> data = conn.GetByOneFeildsSql<base_bom_detail>("bom_id", bomId);
                return data;
            }
        }

        public List<base_bom_detail_response> QuertBomDetailListResponse(string id, string whereStr)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                if (!string.IsNullOrEmpty(whereStr))
                {
                    query = "SELECT bbd.*,bm.material_name,bm.material_code,bm.material_spec " +
                         "FROM base_bom_detail bbd LEFT JOIN  base_material bm  ON bbd.material_id = bm.material_id" +
                          " WHERE bbd.bom_id = @id  AND bbd.DELETE_MARK = '0' ";
                }
                else
                {
                    query = "SELECT bbd.*,bm.material_name,bm.material_code,bm.material_spec " +
                           "FROM base_bom_detail bbd LEFT JOIN  base_material bm  ON bbd.material_id = bm.material_id" +
                            " WHERE bbd.bom_id = @id  AND bbd.DELETE_MARK = '0' ";
                }

                List<base_bom_detail_response> responseList = new();
                List<base_bom_detail_response> b = conn.Query<base_bom_detail, base_material, List<base_bom_detail_response>>(query,
                    (baseBomDetail, baseMterial) =>
                    {
                        base_bom_detail_response _bomResponse = new();

                        if (baseBomDetail != null)
                        {

                            _bomResponse.bom_id = baseBomDetail.bom_id;
                            _bomResponse.bom_detail_id = baseBomDetail.bom_detail_id;
                            _bomResponse.start_date = baseBomDetail.start_date;
                            _bomResponse.end_date = baseBomDetail.end_date;
                            _bomResponse.loss_rate = baseBomDetail.loss_rate;
                            _bomResponse.quantity_deno = baseBomDetail.quantity_deno;
                            _bomResponse.quantity_nume = baseBomDetail.quantity_nume;
                            _bomResponse.material_id = baseBomDetail.material_id;

                        }

                        if (baseMterial != null)
                        {
                            _bomResponse.material_name = baseMterial.material_name;
                            _bomResponse.material_code = baseMterial.material_code;
                            _bomResponse.material_spec = baseMterial.material_spec;
                        }

                        responseList.Add(_bomResponse);
                        return responseList;
                    }, new { id }, splitOn: "material_name").Distinct().SingleOrDefault();
                return b;
            }
        }
    }
}
