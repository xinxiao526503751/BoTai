/*
 * @Author: your name
 * @Date: 2021-09-18 14:33:29
 * @LastEditTime: 2021-09-18 15:19:16
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \Mes_Vue\src\api\IOWareHouseType.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/IOWareHouseType/GetList',
    method: 'post',
    data
  })
}
// 新建地点类型信息
export function CreateWareHouseType(data) {
  return request({
    url: '/IOWareHouseType/CreateWareHouseType',
    method: 'post',
    data
  })
}
// 更新地点类型信息
export function Update(data) {
  return request({
    url: '/IOWareHouseType/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/IOWareHouseType/Delete',
    method: 'post',
    data
  })
}
