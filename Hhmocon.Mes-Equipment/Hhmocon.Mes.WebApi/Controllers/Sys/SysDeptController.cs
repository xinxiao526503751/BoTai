using AutoMapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hhmocon.Mes.WebApi.Controllers
{
    /// <summary>
    /// 部门接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysDeptController
    {
        private readonly SysDeptApp _app;
        private readonly PikachuApp _pikachuApp;
        private readonly IMapper _mapper;
        /// <summary>
        /// 部门控制器构造函数
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pikachuApp"></param>
        public SysDeptController(SysDeptApp app, PikachuApp pikachuApp, IMapper mapper)
        {
            _app = app;
            _pikachuApp = pikachuApp;
            _mapper = mapper;
        }

        /// <summary>
        /// 新建部门信息
        /// </summary>
        /// <param name="obj">部门对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(sys_dept obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_app.Insert(obj))
                {
                    result.Result = obj.dept_id;
                }
                else
                {
                    result.Code = 100;
                    result.Message = "数据写入失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 删除部门信息，如果该部门底下有东西，需要给提示
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="check_flag">
        /// true代表删前检查引用，
        /// false代表直接删掉。</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids, bool check_flag)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;
                if (ids.Length == 0 || ids == null)
                {
                    throw new Exception("未选中部门");
                }

                if (check_flag)
                {
                    //查找用到dept_id字段的表下是不是有数据
                    List<string> chartNames = _pikachuApp.GetAllChartNameHavingSameField("dept_id");
                    List<string> chartExistsData = new();
                    foreach (string id in ids)
                    {
                        foreach (string chart in chartNames)
                        {
                            _app.CheckChartIfExistsData(ref chartExistsData, id, chart);
                            if (chartExistsData.Count > 0)
                            {
                                string chars = string.Join(",", chartExistsData.ToArray());
                                string name = _pikachuApp.GetById<sys_dept>(id).dept_name;
                                switch (chars)
                                {
                                    case "sale_order":
                                        throw new Exception($"销售订单正在引用{name}");
                                    case "sys_dept":
                                        throw new Exception($"系统部门正在引用{name}，该部门拥有子部门");
                                    case "sys_user":
                                        throw new Exception($"{name}下存在用户");
                                    case "sys_user_dept":
                                        throw new Exception($"用户-部门正在引用{name}，这是内部错误，请联系售后");
                                }

                            }
                        }
                    }

                    _app.deleteDepartment(ids);
                }
                else
                {
                    _app.deleteDepartment(ids);
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="obj">部门对象</param>
        /// <returns></returns>

        [HttpPost]
        public Response<string> Update(sys_dept obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sys_dept _Dept = _pikachuApp.GetById<sys_dept>(obj.dept_id);
                //如果能够根据id找到部门
                if (_Dept != null)
                {
                    obj.dept_code = _Dept.dept_code;//锁死code
                    obj.create_time = _Dept.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到部门要返回错误信息
                    result.Result = obj.dept_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.dept_id;

                if (!_pikachuApp.Update(obj))
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "更新失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }



        /// <summary>
        /// 得到部门列表数据
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<sys_dept>(req, ref lcount);
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 给部门专门做一个查询的接口，限制于该部门和子部门
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <param name="dept_id">部门id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetListForSpecialDept(PageReq req, string dept_id)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                //找到该部门和子部门
                List<sys_dept> sys_Depts = new();
                List<sys_dept> data = new();
                sys_Depts = _pikachuApp.GetRootAndBranch<sys_dept>(dept_id, 1);

                //遍历
                foreach (sys_dept temp in sys_Depts)
                {
                    IEnumerable<sys_dept> depts = _pikachuApp.GetList<sys_dept>(req, ref lcount).Where(c => c.dept_id.Equals(temp.dept_id));

                    if (depts != null)
                    {
                        data.AddRange(depts.ToList());
                    }
                }
                pd.Data = data;
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        ///  根据ID得到部门明细信息
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<sys_dept> GetDetail(string id)
        {
            Response<sys_dept> result = new Response<sys_dept>();
            try
            {
                result.Result = _pikachuApp.GetById<sys_dept>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 返回表中所有数据不分页，用来给前端使用插件生成用户管理界面下左侧的部门树状栏
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<TreeEasy>> GetAll()
        {
            Response<List<TreeEasy>> result = new Response<List<TreeEasy>>();
            try
            {
                List<sys_dept> sys_Depts = _pikachuApp.GetAll<sys_dept>();
                //转化为Node
                List<TreeModel> sys_Depts_Nodes = _pikachuApp.ListElementToNode(sys_Depts, false);
                //将节点列表转化为树
                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(sys_Depts_Nodes);
                //将树转化为TreeEasy树
                List<TreeEasy> easies = _pikachuApp.TreeModelToTreeEasy(treeModels);
                result.Result = easies;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// "再给我做个不分页获取全部部门列表的接口吧"
        /// "我要列表的"
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<sys_dept>> GetYourAll()
        {
            Response<List<sys_dept>> result = new Response<List<sys_dept>>();
            try
            {
                List<sys_dept> sys_Depts = _pikachuApp.GetAll<sys_dept>();
                result.Result = sys_Depts;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 根据部门id获取所有子部门
        /// </summary>
        /// <param name="sys_dept_id">部门id，为Null时获取所有部门</param>
        /// <param name="rootneed">得到的值中是否添加根节点.1加0不加</param>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetSubDeptByDeptId(string sys_dept_id, PageReq req, int rootneed = 1)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();

            List<sys_dept> subdepts = new();
            try
            {

                subdepts = _pikachuApp.GetRootAndBranch<sys_dept>(sys_dept_id, rootneed);


                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    subdepts = subdepts.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Data = subdepts;
                pd.Total = subdepts.Count;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }


            result.Result = pd;
            return result;
        }


        /// <summary>
        /// 传入部门id获取挂载在部门下的用户
        /// </summary>
        /// <param name="dept_id">部门id</param>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetUserByDeptId(string dept_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            List<GetUserByDeptIdResponse> getUserByDeptIdResponses = new();
            try
            {
                //根据部门id找到所有部门
                List<sys_dept> depts = _pikachuApp.GetRootAndBranch<sys_dept>(dept_id);

                //遍历部门
                foreach (sys_dept temp in depts)
                {
                    //根据User的dept_id查找部门下的user
                    List<sys_user> user_temp = _app.GetUserByDeptIdInUserChart(temp.dept_id);
                    if (user_temp != null)
                    {
                        //遍历部门下的用户
                        //用户密码不能给前端
                        foreach (sys_user u_temp in user_temp)
                        {
                            u_temp.user_passwd = "";


                            GetUserByDeptIdResponse getUserByDeptIdResponse = new();
                            getUserByDeptIdResponse.role_names = new();
                            _mapper.Map(u_temp, getUserByDeptIdResponse);

                            sys_dept sys_Dept = _pikachuApp.GetById<sys_dept>(u_temp.dept_id);
                            if (sys_Dept == null)
                            {
                                throw new Exception($"无效的部门{u_temp.dept_id}");
                            }
                            getUserByDeptIdResponse.dept_name = sys_Dept.dept_name;
                            List<sys_user_role> _User_Roles = _pikachuApp.GetByOneFeildsSql<sys_user_role>("user_id", u_temp.user_id);
                            List<string> role_names = new();
                            foreach (sys_user_role sys_User_Role in _User_Roles)
                            {
                                sys_role sys_Role = _pikachuApp.GetById<sys_role>(sys_User_Role.role_id);
                                if (sys_Role == null)
                                {
                                    throw new Exception($"无效的角色id{sys_User_Role.role_id}");
                                }
                                if (sys_Role.role_name != null && sys_Role.role_name != "")
                                {
                                    role_names.Add(sys_Role.role_name);
                                }
                            }
                            getUserByDeptIdResponse.role_names.AddRange(role_names);
                            StringBuilder temp_string_builder = new();
                            foreach (string temp_b in getUserByDeptIdResponse.role_names)
                            {
                                temp_string_builder.Append(temp_b).Append(',');
                            }

                            getUserByDeptIdResponse.role_name_string = temp_string_builder.ToString().TrimEnd(',');

                            getUserByDeptIdResponses.Add(getUserByDeptIdResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            if (req != null)
            {
                int iPage = req.page;
                int iRows = req.rows;
                //分页
                getUserByDeptIdResponses = getUserByDeptIdResponses.Skip((iPage - 1) * iRows).Take(iRows).ToList();

            }
            pd.Data = getUserByDeptIdResponses;
            pd.Total = getUserByDeptIdResponses.Count;
            result.Result = pd;
            return result;
        }


    }
}
