/*
 * @Author: your name
 * @Date: 2021-06-16 15:44:25
 * @LastEditTime: 2021-07-14 15:24:31
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseMaterialType.js
 */
import request from '@/utils/request'

// 得到物料类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseMaterialType/GetList',
    method: 'post',
    data
  })
}

// 更新物料类型信息
export function Update(data) {
  return request({
    url: '/BaseMaterialType/Update',
    method: 'post',
    data
  })
}

// 根据物料类型ID 删除物料类型 可批量删除
export function Delete(data) {
  return request({
    url: '/BaseMaterialType/DeleteMask',
    method: 'post',
    data
  })
}

// 新建物料类型信息
export function Create(data) {
  return request({
    url: '/BaseMaterialType/Create',
    method: 'post',
    data
  })
}

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/BaseMaterialType/GetDetail',
    method: 'post',
    data
  })
}

// 获取所有数据不分页
export function GetAll(data) {
  return request({
    url: '/BaseMaterialType/GetAll',
    method: 'post',
    data
  })
}
