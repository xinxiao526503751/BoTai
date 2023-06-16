/*
 * @Author: your name
 * @Date: 2021-07-20 16:53:52
 * @LastEditTime: 2021-09-08 23:01:09
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseEquipment.js
 */
import request from '@/utils/request'

// 得到物料类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseEquipment/GetList',
    method: 'post',
    data
  })
}

// 更新物料类型信息
export function Update(data) {
  return request({
    url: '/BaseEquipment/Update',
    method: 'post',
    data
  })
}

// 根据物料类型ID 删除物料类型 可批量删除
export function Delete(data) {
  return request({
    url: '/BaseEquipment/Delete',
    method: 'post',
    data
  })
}

// 新建物料类型信息
export function Create(data) {
  return request({
    url: '/BaseEquipment/Create',
    method: 'post',
    data
  })
}

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/BaseEquipment/GetDetail',
    method: 'post',
    data
  })
}

// 根据地点id获取地点下挂载的设备
export function GetEquipment(data, location_id, OnlyRootEquip) {
  return request({
    url: '/BaseEquipment/GetEquipmentByLocationId',
    method: 'post',
    data,
    params: {
      location_id: location_id,
      OnlyRootEquip: OnlyRootEquip
    }
  })
}

// 获取设备不分页
export function GetAll(data) {
  return request({
    url: '/BaseEquipment/GetAll',
    method: 'post',
    data
  })
}

export function SearchLeafs(data, location_id) {
  return request({
    url: '/BaseEquipment/SearchLeafs',
    method: 'post',
    data,
    params: {
      location_id: location_id // OnlyRootEquip: OnlyRootEquip
    }
  })
}
