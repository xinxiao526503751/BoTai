/*
 * @Author: your name
 * @Date: 2021-07-19 08:57:45
 * @LastEditTime: 2021-09-08 22:17:20
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseEquipmentClass.js
 */
import request from '@/utils/request'

// 得到物料类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseEquipmentClass/GetList',
    method: 'post',
    data
  })
}

// 更新物料类型信息
export function Update(data) {
  return request({
    url: '/BaseEquipmentClass/Update',
    method: 'post',
    data
  })
}

// 根据物料类型ID 删除物料类型 可批量删除
export function Delete(data) {
  return request({
    url: '/BaseEquipmentClass/Delete',
    method: 'post',
    data
  })
}

// 新建物料类型信息
export function Create(data) {
  return request({
    url: '/BaseEquipmentClass/Create',
    method: 'post',
    data
  })
}

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/BaseEquipmentClass/GetDetail',
    method: 'post',
    data
  })
}

// 获取所有数据不分页
export function GetAll(data) {
  return request({
    url: '/BaseEquipmentClass/GetAll',
    method: 'post',
    data
  })
}

// 根据传入的id和分页数据找叶
export function GetLeaf(data, params) {
  return request({
    url: '/BaseEquipmentClass/GetLeafs',
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
    url: '/BaseEquipmentClass/SearchBar',
    method: 'post',
    data
  })
}
