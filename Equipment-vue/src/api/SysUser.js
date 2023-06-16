/*
 * @Author: your name
 * @Date: 2021-08-13 15:12:06
 * @LastEditTime: 2021-11-24 09:53:17
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\SysUser.js
 */
import request from '@/utils/request'

// 新建用户信息
export function Create(data) {
  return request({
    url: '/SysUser/Create',
    method: 'post',
    data
  })
}

// 删除用户信息
export function Delete(data) {
  return request({
    url: '/SysUser/Delete',
    method: 'post',
    data
  })
}

// 更新用户信息
export function Update(data) {
  return request({
    url: '/SysUser/Update',
    method: 'post',
    data
  })
}

// 得到用户列表数据
export function GetList(data) {
  return request({
    url: '/SysUser/GetList',
    method: 'post',
    data
  })
}

// 根据ID得到用户明细信息
export function GetDetail(data) {
  return request({
    url: '/SysUser/GetDetail',
    method: 'post',
    data
  })
}

// 部门下的用户界面的 左上角的 搜索框 功能:用户输入name,根据name或ch_name查询 选中部门时，会传入dept_id，不选中时传入的dept_id=null
export function SearchBar(data) {
  return request({
    url: '/SysUser/SearchBar',
    method: 'post',
    data
  })
}
// 修改密码
export function ChangePassword(data) {
  return request({
    url: '/SysUser/ChangePassword',
    method: 'post',
    data
  })
}

// 重置密码
export function ResetPassword(data) {
  return request({
    url: '/SysUser/ResetPassword',
    method: 'post',
    data
  })
}
