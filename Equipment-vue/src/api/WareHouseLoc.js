/*
 * @Author: gcy
 * @Date: 2021-09-15 14:02:25
 * @LastEditTime: 2021-09-15 14:04:15
 * @LastEditors: gcy
 * @Description: 仓库库位接口
 * @FilePath: \Mes_Vue\src\api\WareHouseLoc.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/WareHouseLoc/GetList',
    method: 'post',
    data
  })
}
// 新建地点类型信息
export function Insert(data) {
  return request({
    url: '/WareHouseLoc/Insert',
    method: 'post',
    data
  })
}
// 更新地点类型信息
export function Update(data) {
  return request({
    url: '/WareHouseLoc/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/WareHouseLoc/Delete',
    method: 'post',
    data
  })
}
