/*
 * @Author: your name
 * @Date: 2021-07-15 09:26:51
 * @LastEditTime: 2021-09-08 21:10:25
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseEquipmentType.js
 */
import request from '@/utils/request'

// 得到物料类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseEquipmentType/GetList',
    method: 'post',
    data
  })
}

// 更新物料类型信息
export function Update(data) {
  return request({
    url: '/BaseEquipmentType/Update',
    method: 'post',
    data
  })
}

// 根据物料类型ID 删除物料类型 可批量删除
export function Delete(data) {
  return request({
    url: '/BaseEquipmentType/Delete',
    method: 'post',
    data
  })
}

// 新建物料类型信息
export function Create(data) {
  return request({
    url: '/BaseEquipmentType/Create',
    method: 'post',
    data
  })
}

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/BaseEquipmentType/GetDetail',
    method: 'post',
    data
  })
}

// 获取所有数据不分页
export function GetAll(data) {
  return request({
    url: '/BaseEquipmentType/GetAll',
    method: 'post',
    data
  })
}

// 根据传入的id和分页数据找叶
export function GetLeaf(data, params) {
  return request({
    url: '/BaseEquipmentType/GetLeafs',
    method: 'post',
    data,
    params: {
      root_id: params
    }
  })
}

// 物料类型下的物料界面的 左上角的 搜索框 功能:输入code或者name,根据code或者name查询 选中物料类型时，会传入type_id，不选中时传入的dept_id=null
export function SearchBar(data) {
  return request({
    url: '/BaseEquipmentType/SearchBar',
    method: 'post',
    data
  })
}
