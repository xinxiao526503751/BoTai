/*
 * @Author: your name
 * @Date: 2021-08-17 10:01:44
 * @LastEditTime: 2021-07-17 10:20:57
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\SysRole.js
 */
import request from '@/utils/request'

// 获取角色列表的数据
export function GetList(data) {
  return request({
    url: '/SysRole/GetList',
    method: 'post',
    data
  })
}
// 新增角色数据
export function Create(data) {
  return request({
    url: '/SysRole/Create',
    method: 'post',
    data
  })
}
//删除角色数据
export function Delete(data) {
  return request({
    url: '/SysRole/Delete',
    method: 'post',
    data
  })
}
