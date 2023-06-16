/*
 * @Author: your name
 * @Date: 2021-08-13 14:50:07
 * @LastEditTime: 2021-08-16 19:55:14
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\SysDept.js
 */
import request from '@/utils/request'

// 新建部门信息
export function Create(data) {
  return request({
    url: '/SysDept/Create',
    method: 'post',
    data
  })
}

// 删除部门信息
export function Delete(data) {
  return request({
    url: '/SysDept/Delete',
    method: 'post',
    data
  })
}

// 更新部门信息
export function Update(data) {
  return request({
    url: '/SysDept/Update',
    method: 'post',
    data
  })
}

// 得到部门列表数据
export function GetList(data) {
  return request({
    url: '/SysDept/GetList',
    method: 'post',
    data
  })
}

// 根据ID得到部门明细信息
export function GetDetail(data) {
  return request({
    url: '/SysDept/GetDetail',
    method: 'post',
    data
  })
}

// 返回表中所有数据不分页，用来给前端使用插件生成用户管理界面下左侧的部门树状栏
export function GetAll(data) {
  return request({
    url: '/SysDept/GetAll',
    method: 'post',
    data
  })
}

// 传入部门id获取挂载在部门下的用户
export function GetUserByDeptId(data, dept_id) {
  return request({
    url: '/SysDept/GetUserByDeptId',
    method: 'post',
    data,
    params: {
      dept_id: dept_id
    }
  })
}

// 根据部门id获取所有子部门 第二个参数给true表示只要id下挂载的二级部门，不给默认false
export function GetSubDeptByDeptId(
  data,
  sys_dept_id,
  GlitteringCloud,
  rootneed
) {
  return request({
    url: '/SysDept/GetSubDeptByDeptId',
    method: 'post',
    data,
    params: {
      sys_dept_id: sys_dept_id,
      GlitteringCloud: GlitteringCloud,
      rootneed: rootneed
    }
  })
}

// 获取部门列表不分页数据
export function GetYourAll(data) {
  return request({
    url: '/SysDept/GetYourAll',
    method: 'post',
    data
  })
}

// 给部门专门做一个查询的接口，限制于该部门
export function GetListForSpecialDept(data, dept_id) {
  return request({
    url: '/SysDept/GetListForSpecialDept',
    method: 'post',
    data,
    params: {
      dept_id: dept_id
    }
  })
}
