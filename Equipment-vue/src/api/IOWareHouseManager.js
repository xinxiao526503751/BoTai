/*
 * @Author: gcy
 * @Date: 2021-09-22 13:58:47
 * @LastEditTime: 2021-09-26 09:56:30
 * @LastEditors: Please set LastEditors
 * @Description: 出入库管理接口
 * @FilePath: \Mes_Vue\src\api\IOWareHouseManager.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/IOWareHouseManager/GetList',
    method: 'post',
    data
  })
}
// 出入库管页面，左边的地点仓库树 地点-仓库
export function LocationWareHouseTree(data) {
  return request({
    url: '/IOWareHouseManager/LocationWareHouseTree',
    method: 'post',
    data
  })
}
// 根据地点id或仓库id获取出入库记录
export function GetIoWareHouseRec(id, IOopration) {
  return request({
    url: '/IOWareHouseManager/GetIoWareHouseRec',
    method: 'post',
    params: {
      id: id,
      IOopration: IOopration
    }
  })
}

// 入库操作,可以多条记录同时入库
export function IoWareHouseOperation(data) {
  return request({
    url: '/IOWareHouseManager/IoWareHouseOperation',
    method: 'post',
    data
  })
}

// 调拨操作
export function MoveOperation(data) {
  return request({
    url: '/IOWareHouseManager/MoveOperation',
    method: 'post',
    data
  })
}

// 盘点操作
export function CheckOperation(warehouse_loc_id, num) {
  return request({
    url: '/IOWareHouseManager/CheckOperation',
    method: 'post',
    params: {
      warehouse_loc_id: warehouse_loc_id,
      num: num
    }
  })
}
