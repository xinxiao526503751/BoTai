/*
 * @Author: your name
 * @Date: 2021-06-16 15:44:25
 * @LastEditTime: 2022-08-20 14:47:39
 * @LastEditors: chenyun625 chenyun625@outlook.com
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\cuttertypeType.js
 */
/* eslint-disable camelcase */
import request from '@/utils/request'

// 得到刀具类型分页列表数据
export function GetList(data) {
  return request({
    url: '/CutterManager/GetListCutterType',
    method: 'post',
    data
  })
}
// 此接口不可用
// 刀具类型下的刀具界面的 左上角的 搜索框 功能:输入code或者name,根据code或者name查询 选中刀具类型时，会传入type_id，不选中时传入的dept_id=null
export function SearchBar(data) {
  return request({
    url: '/CutterManager/SearchBar',
    method: 'post',
    data
  })
}
// 新建刀具类型信息
export function CreateCutterType(data) {
  return request({
    url: '/CutterManager/CreateCutterType',
    method: 'post',
    data
  })
}

export function UpdateCutterType(data) {
  return request({
    url: '/CutterManager/UpdateCutterType',
    method: 'post',
    data
  })
}

// 根据刀具类型ID 删除刀具类型 可批量删除
export function DeleteCutterType(data) {
  return request({
    url: '/CutterManager/DeleteCutterType',
    method: 'post',
    data
  })
}

// 得到刀具参数分页列表数据
export function GetListBase(data) {
  return request({
    url: '/CutterManager/GetListCutterBase',
    method: 'post',
    data
  })
}
// 新建刀具类型信息
export function CreateCutterBase(data) {
  return request({
    url: '/CutterManager/CreateCutterBase',
    method: 'post',
    data
  })
}

export function UpdateCutterBase(data) {
  return request({
    url: '/CutterManager/UpdateCutterBase',
    method: 'post',
    data
  })
}

// 根据刀具类型ID 删除刀具类型 可批量删除
export function DeleteCutterBase(data) {
  return request({
    url: '/CutterManager/DeleteCutterBase',
    method: 'post',
    data
  })
}

export function GetCutterFeatureOnZouDao(data, params) {
  return request({
    url: '/CutterManager/GetCutterFeatureOnZouDao',
    method: 'post',
    data,
    params: {
      id: params
    }
  })
}
// 根据id得到单个刀具数据
// export function OneCutterList(data) {
//   var id = data
//   return request({
//     url: `/CutterManager/getTypeByParentId?parentId=${id}`,
//     method: 'get'
//     // params: data
//   })
// }

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/CutterManager/GetDetail',
    method: 'post',
    data
  })
}

// 根据传入的material_type_id查找所有material的属性有该materialTypeId的元素
export function GetAllByMaterialTypeName(data) {
  return request({
    url: '/CutterManager/GetAllBy_materialTypeName',
    method: 'post',
    data
  })
}

// 根据传入的id和分页数据找叶
export function GetLeaf(data, params) {
  return request({
    url: '/CutterManager/GetLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}


