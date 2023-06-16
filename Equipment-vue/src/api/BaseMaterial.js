/*
 * @Author: your name
 * @Date: 2021-06-16 15:44:25
 * @LastEditTime: 2021-09-08 19:19:50
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseMaterialType.js
 */
/* eslint-disable camelcase */
import request from '@/utils/request'

// 得到物料类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseMaterial/GetList',
    method: 'post',
    data
  })
}

// 根据条件得到全部列表数据
export function GetAll(data) {
  return request({
    url: '/BaseMaterial/GetAll',
    method: 'post',
    data
  })
}

// 更新物料类型信息
export function Update(data) {
  return request({
    url: '/BaseMaterial/Update',
    method: 'post',
    data
  })
}

// 根据物料类型ID 删除物料类型 可批量删除
export function Delete(data) {
  return request({
    url: '/BaseMaterial/Delete',
    method: 'post',
    data
  })
}

// 新建物料类型信息
export function Create(data) {
  return request({
    url: '/BaseMaterial/Create',
    method: 'post',
    data
  })
}

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/BaseMaterial/GetDetail',
    method: 'post',
    data
  })
}

// 根据传入的material_type_id查找所有material的属性有该materialTypeId的元素
export function GetAllByMaterialTypeName(data) {
  return request({
    url: '/BaseMaterial/GetAllBy_materialTypeName',
    method: 'post',
    data
  })
}

// 根据传入的id和分页数据找叶
export function GetLeaf(data, params) {
  return request({
    url: '/BaseMaterial/GetLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}

// 物料类型下的物料界面的 左上角的 搜索框 功能:输入code或者name,根据code或者name查询 选中物料类型时，会传入type_id，不选中时传入的dept_id=null
export function SearchBar(data) {
  return request({
    url: '/BaseMaterial/SearchBar',
    method: 'post',
    data
  })
}
