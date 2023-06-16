/*
 * @Author: gcy
 * @Date: 2021-09-15 14:00:50
 * @LastEditTime: 2021-09-16 09:07:15
 * @LastEditors: Please set LastEditors
 * @Description: 仓库定义接口
 * @FilePath: \Mes_Vue\src\api\WareHouse.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/WareHouse/GetList',
    method: 'post',
    data
  })
}
// 新建地点类型信息
export function Insert(data) {
  return request({
    url: '/WareHouse/Insert',
    method: 'post',
    data
  })
}
// 更新地点类型信息
export function Update(data) {
  return request({
    url: '/WareHouse/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/WareHouse/Delete',
    method: 'post',
    data
  })
}

// 仓库定义页面的搜索框，地点-仓库定义
export function SearchBar(data, param) {
  return request({
    url: '​/WareHouse​/SearchBar',
    method: 'post',
    data,
    params: {
      location_id: param
    }
  })
}
