/*
 * @Author: your name
 * @Date: 2021-08-17 19:04:28
 * @LastEditTime: 2021-08-20 09:50:00
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseEquipmentSub.js
 */
import request from '@/utils/request'

// 向关联表中添加设备子设备
export function AddEquipmentSubEquipment(data, parent_id) {
  return request({
    url: '/BaseEquipmentSub/AddEquipmentSubEquipment',
    method: 'post',
    data,
    params: {
      parent_id: parent_id
    }
  })
}

// 传入设备id，通过子设备关联表获取二级设备
export function GetSubEquipment(equipment_id) {
  return request({
    url: '/BaseEquipmentSub/GetSecondEquipmentByEquipmentId',
    method: 'post',
    params: {
      equipment_id: equipment_id
    }
  })
}

// 通过设备id获取工作单元层级树
export function WorkUnitHierarchyTree(equipment_id) {
  return request({
    url: '/BaseEquipmentSub/WorkUnitHierarchyTree',
    method: 'post',
    params: {
      equipment_id: equipment_id
    }
  })
}

// 添加设备子设备出现的地点设备树
export function AddSubEquipment(equipment_id) {
  return request({
    url: '/BaseEquipmentSub/AddSubEquipment',
    method: 'post',
    params: {
      equipment_id: equipment_id
    }
  })
}

// 根据下级设备ID 删除子设备 可批量删除
export function Delete(id, sub_id) {
  return request({
    url: '/BaseEquipmentSub/Delete',
    method: 'post',
    params: {
      id: id,
      sub_id: sub_id
    }
  })
}
