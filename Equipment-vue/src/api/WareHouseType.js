/*
 * @Author: gcy
 * @Date: 2021-09-13 21:51:46
 * @LastEditTime: 2021-09-13 21:54:22
 * @LastEditors: Please set LastEditors
 * @Description: 仓库类型页面接口
 * @FilePath: \Mes_Vue\src\api\WareHouseType.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/WareHouseType/GetList',
    method: 'post',
    data
  })
}
// 新建地点类型信息
export function Insert(data) {
  return request({
    url: '/WareHouseType/Insert',
    method: 'post',
    data
  })
}
// 更新地点类型信息
export function Update(data) {
  return request({
    url: '/WareHouseType/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/WareHouseType/Delete',
    method: 'post',
    data
  })
}
