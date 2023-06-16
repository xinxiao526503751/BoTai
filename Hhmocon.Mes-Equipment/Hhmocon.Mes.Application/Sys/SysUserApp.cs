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

using AutoMapper;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.LoginRelated;
using Hhmocon.Mes.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 用户App
    /// </summary>
    public class SysUserApp
    {
        private readonly ISysUserRepository _sysUserRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IMapper _mapper;
        private readonly ILogger<exam_plan_method> _logger;
        private readonly IAuth _auth;

        public SysUserApp(ISysUserRepository sysUserRepository, PikachuRepository pikachuRepository, PikachuApp pikachuApp,
            ILogger<exam_plan_method> logger, IMapper mapper, IAuth auth)
        {
            _sysUserRepository = sysUserRepository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _logger = logger;
            _mapper = mapper;
            _auth = auth;
        }

        /// <summary>
        /// 添加用户数据,对name查重
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Insert(CreateUserRequest obj)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //新建事务
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    sys_user sys_User = new();
                    _mapper.Map(obj, sys_User);

                    //查重
                    List<sys_user> getByName = _sysUserRepository.GetByName(sys_User.user_name, transaction, dbConnection: conn);
                    if (getByName != null)
                    {
                        throw new Exception("登录名重复");
                    }

                    //取ID
                    sys_User.user_id = CommonHelper.GetNextGUID();
                    sys_User.modified_time = Time.Now;
                    sys_User.create_time = DateTime.Now;
                    sys_User.last_login_time = Time.Now;//Time.ToDate("2000-1-1 00:00:00");
                    sys_User.create_by = _auth.GetUserAccount(null);
                    sys_User.create_by_name = _auth.GetUserName(null);
                    sys_User.modified_by = _auth.GetUserAccount(null);
                    sys_User.modified_by_name = _auth.GetUserName(null);
                    //添加用户时不指定密码，密码默认为工号
                    if (sys_User.user_passwd == null)
                    {
                        sys_User.user_passwd = sys_User.empno;
                    }

                    //md5密码加密
                    sys_User.user_passwd = sys_User.user_passwd.ToMD5();

                    //写入用户
                    _pikachuRepository.Insert(sys_User, transaction, dbConnection: conn);
                    sys_user_dept sys_User_Dept = new();
                    _mapper.Map(sys_User, sys_User_Dept);
                    sys_User_Dept.id = CommonHelper.GetNextGUID();
                    sys_User_Dept.modified_time = DateTime.Now;
                    sys_User_Dept.create_time = DateTime.Now;
                    sys_User_Dept.create_by = _auth.GetUserAccount(null);
                    sys_User_Dept.create_by_name = _auth.GetUserName(null);
                    sys_User_Dept.modified_by = _auth.GetUserAccount(null);
                    sys_User_Dept.modified_by_name = _auth.GetUserName(null);
                    //将user_dept关联写入关联表
                    _pikachuRepository.Insert(sys_User_Dept, transaction, dbConnection: conn);

                    if (obj.roles != null)
                    {
                        //遍历角色，写入user_role关联表
                        foreach (string temp in obj.roles)
                        {
                            sys_user_role sys_User_Role = new();
                            sys_User_Role.id = CommonHelper.GetNextGUID();
                            sys_User_Role.role_id = temp;
                            sys_User_Role.user_id = sys_User.user_id;
                            sys_User_Role.modified_time = Time.Now;
                            sys_User_Role.create_time = DateTime.Now;
                            sys_User_Role.create_by = _auth.GetUserAccount(null);
                            sys_User_Role.create_by_name = _auth.GetUserName(null);
                            sys_User_Role.modified_by = _auth.GetUserAccount(null);
                            sys_User_Role.modified_by_name = _auth.GetUserName(null);
                            //写入用户——角色关联
                            _pikachuRepository.Insert(sys_User_Role, transaction, dbConnection: conn);
                        }
                    }

                    //提交事务
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    //事务回滚
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public bool Delete(List<string> user_id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //新建事务
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    List<string> delete_strings_user_dept = new();
                    List<string> delet_strings_user_role = new();
                    foreach (string temp in user_id)
                    {
                        List<sys_user_dept> sys_User_Depts = _pikachuRepository.GetByOneFeildsSql<sys_user_dept>("user_id", temp, tran: transaction, dbConnection: conn);
                        if (sys_User_Depts != null && sys_User_Depts.Count != 0)
                        {
                            foreach (sys_user_dept _User_Dept in sys_User_Depts)
                            {
                                delete_strings_user_dept.Add(_User_Dept.id);
                            }
                        }
                        List<sys_user_role> sys_User_Roles = _pikachuRepository.GetByOneFeildsSql<sys_user_role>("user_id", temp, tran: transaction, dbConnection: conn);
                        if (sys_User_Roles != null && sys_User_Roles.Count != 0)
                        {
                            foreach (sys_user_role _User_Role in sys_User_Roles)
                            {
                                delet_strings_user_role.Add(_User_Role.id);
                            }
                        }
                    }

                    string[] ids1 = delete_strings_user_dept.ToArray();
                    if (ids1.Length > 0)
                    {
                        _pikachuRepository.Delete_Mask<sys_user_dept>(ids1, tran: transaction, dbConnection: conn);
                    }

                    string[] ids2 = delet_strings_user_role.ToArray();
                    _pikachuRepository.Delete_Mask<sys_user_role>(ids2, tran: transaction, dbConnection: conn);

                    string[] ids3 = user_id.ToArray();
                    _pikachuRepository.Delete_Mask<sys_user>(ids3);

                    //提交事务
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    //事务回滚
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    throw new Exception(exception.Message);
                }
            }
        }



        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(UpdateUserRequest updateUserRequest)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //新建事务
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    sys_user _User = _pikachuRepository.GetById<sys_user>(updateUserRequest.user_id, tran: transaction, dbConnection: conn);

                    if (_User != null)
                    {
                        _mapper.Map(updateUserRequest, _User);
                        _User.modified_time = Time.Now;//更新修改时间

                        if (updateUserRequest.user_passwd == null)
                        {
                            throw new Exception("密码不能为null");
                        }
                        //前端在修改资料时传来的密码为不为空 且不是空字符串，后端要更新密码
                        if (updateUserRequest.user_passwd != null && updateUserRequest.user_passwd != "")
                        {
                            _User.user_passwd = updateUserRequest.user_passwd;
                        }
                    }
                    else
                    {
                        throw new Exception("无法根据id找到用户");
                    }

                    _pikachuApp.Update(_User);

                    //删除user_role表中关联user_id的数据
                    List<sys_user_role> sys_User_Roles = _pikachuApp.GetByOneFeildsSql<sys_user_role>("user_id", _User.user_id);//_pikachuApp.GetAll<sys_user_role>(tran: transaction, dbConnection: conn).Where(c => c.user_id == temp).ToList();
                    foreach (sys_user_role s_temp in sys_User_Roles)
                    {
                        s_temp.delete_mark = 1;
                        _pikachuApp.Update(s_temp, tran: transaction, dbConnection: conn);
                    }

                    //删除user_dept表中关联user_id的数据
                    List<sys_user_dept> sys_User_Depts = _pikachuApp.GetByOneFeildsSql<sys_user_dept>("user_id", _User.user_id); ;
                    foreach (sys_user_dept s_temp in sys_User_Depts)
                    {
                        s_temp.delete_mark = 1;
                        _pikachuApp.Update(s_temp, tran: transaction, dbConnection: conn);
                    }

                    //新增user_dept关联
                    sys_user_dept sys_User_Dept = new();
                    sys_User_Dept.id = CommonHelper.GetNextGUID();
                    sys_User_Dept.user_id = updateUserRequest.user_id;
                    sys_User_Dept.dept_id = updateUserRequest.dept_id;
                    sys_User_Dept.create_time = Time.Now;
                    sys_User_Dept.modified_time = Time.Now;
                    _pikachuRepository.Insert(sys_User_Dept, tran: transaction, dbConnection: conn);
                    //遍历角色，新增user_role关联
                    foreach (string temp in updateUserRequest.roles)
                    {
                        sys_user_role sys_User_Role = new();
                        sys_User_Role.id = CommonHelper.GetNextGUID();
                        sys_User_Role.user_id = updateUserRequest.user_id;
                        sys_User_Role.role_id = temp;
                        sys_User_Role.create_time = Time.Now;
                        sys_User_Role.modified_time = Time.Now;
                        _pikachuRepository.Insert(sys_User_Role, tran: transaction, dbConnection: conn);
                    }



                    //提交事务
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    //事务回滚
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_user> GetByName(string name)
        {
            List<sys_user> sys_Users = _sysUserRepository.GetByName(name);
            return sys_Users;
        }

        /// <summary>
        /// 将sys_user列表转化成Node
        /// parent_id=Dept_id;
        /// 用来在生成树时和部门Nodes衔接
        /// </summary>
        /// <returns></returns>
        public List<TreeModel> ListElementToNodeLinkWithDept(List<sys_user> list)
        {
            List<TreeModel> data = _sysUserRepository.ListElementToNodeLinkWithDept(list);
            return data;
        }

        /// <summary>
        /// 部门-用户页面搜索框
        /// 功能：搜索框可输入名称，根据选中的部门id和子部门查找name或者ch_name符合条件的用户(不区分大小写)
        /// 不选中时部门Id为null，在所有部门中找
        /// </summary>
        /// <returns></returns>
        public List<sys_user> SearchBar(SysUserSearchBarRequest sysUserSearchBarRequest)
        {
            List<sys_user> sys_Users = new();
            //如果部门id为Null，在所有用户中查询然后分页
            if (sysUserSearchBarRequest.Dept_id == null)
            {
                return _pikachuRepository.GetAll<sys_user>()
                    .Where(c => c.user_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower()) || c.user_cn_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower()))
                    .Skip((sysUserSearchBarRequest.Page - 1) * sysUserSearchBarRequest.Rows)
                    .Take(sysUserSearchBarRequest.Rows).ToList();
            }
            //如果部门id不为null，以传入的部门id为根找到连锁的部门
            List<sys_dept> sys_Depts = _pikachuApp.GetRootAndBranch<sys_dept>(sysUserSearchBarRequest.Dept_id);
            //遍历部门，根据部门id和传入的name在sys_user表查找用户
            foreach (sys_dept temp in sys_Depts)
            {
                List<sys_user> temp_users = _pikachuRepository.GetAll<sys_user>()
                    .Where(c =>
                    c.dept_id == temp.dept_id
                    &&
                    (c.user_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower()) || c.user_cn_name.ToLower().Contains(sysUserSearchBarRequest.Name.ToLower()))).ToList();
                if (temp_users.Count == 0)
                {
                    continue;
                }

                sys_Users.AddRange(temp_users);
            }
            return sys_Users.Skip((sysUserSearchBarRequest.Page - 1) * sysUserSearchBarRequest.Rows)
                    .Take(sysUserSearchBarRequest.Rows).ToList();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        public ChangePasswordReq ChangePassword(ChangePasswordReq request)
        {

            //request.Account=request.Account?? _auth.GetUserAccount(null);
            request.Account = _auth.GetUserAccount(null);
            if (string.IsNullOrEmpty(request.Account))
            {
                throw new Exception("用户名不能为空");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                throw new Exception("密码不能为空");
            }

            sys_user model = _pikachuApp.GetAll<sys_user>().Where(u => u.user_name == request.Account).FirstOrDefault();
            if (model == null)
            {
                throw new Exception("该用户不存在");
            }

            if (model.user_passwd != request.OldPassword.ToMD5())
            {
                throw new Exception("原密码不正确");
            }

            model.modified_by_name = _auth.GetUserName(null);
            model.create_by = _auth.GetUserAccount(null);
            model.user_passwd = request.Password.ToMD5();
            model.modified_time = Time.Now;
            _pikachuApp.Update(model);
            return request;
        }

        public ChangePasswordReq ResetPassword(ChangePasswordReq request)
        {

            request.Account = request.Account ?? _auth.GetUserAccount(null);
            if (string.IsNullOrEmpty(request.Account))
            {
                throw new Exception("用户名不能为空");
            }

            sys_user model = _pikachuApp.GetAll<sys_user>().Where(u => u.user_name == request.Account).FirstOrDefault();
            if (model == null)
            {
                throw new Exception("该用户不存在");
            }

            model.modified_by_name = _auth.GetUserName(null);
            model.create_by = _auth.GetUserAccount(null);
            model.user_passwd = "123456".ToMD5();
            model.modified_time = Time.Now;
            _pikachuApp.Update(model);
            return request;
        }
    }
}
