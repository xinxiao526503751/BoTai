/*
 * @Author: your name
 * @Date: 2021-08-13 10:01:44
 * @LastEditTime: 2021-07-14 10:20:57
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\SysSystem.js
 */
import request from '@/utils/request'

// 新增系统数据
export function Create(data) {
  return request({
    url: '/SysSystem/Create',
    method: 'post',
    data
  })
}
// 更新系统数据
export function Update(data) {
  return request({
    url: '/SysSystem/Update',
    method: 'post',
    data
  })
}
//删除系统数据
export function Delete(data) {
  return request({
    url: '/SysSystem/Delete',
    method: 'post',
    data
  })
}
//遍历获取所有根节点及下面的子节点
export function ObtainAllNodes() {
  return request({
    url: '/SysSystem/ObtainAllNodes',
    method: 'post'
  })
}
