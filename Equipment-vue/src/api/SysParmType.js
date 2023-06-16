/*
 * @Author: gcy
 * @Date: 2021-09-21 16:21:13
 * @LastEditTime: 2021-09-21 16:22:42
 * @LastEditors: Please set LastEditors
 * @Description: 系统参数类型接口
 * @FilePath: \Mes_Vue\src\api\SysParmType.js
 */
import request from '@/utils/request'

// 得到系统参数分页列表数据
export function GetList(data) {
  return request({
    url: '/SysParmType/GetList',
    method: 'post',
    data
  })
}
// 新建系统参数信息
export function Insert(data) {
  return request({
    url: '/SysParmType/Insert',
    method: 'post',
    data
  })
}
// 更新系统参数信息
export function Update(data) {
  return request({
    url: '/SysParmType/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/SysParmType/Delete',
    method: 'post',
    data
  })
}
