/*
 * @Author: your name
 * @Date: 2021-06-16 15:44:25
 * @LastEditTime: 2022-06-28 13:28:25
 * @LastEditors: chenyun625 chenyun625@outlook.com
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\cuttertypeType.js
 */
/* eslint-disable camelcase */
import request from '@/utils/request'

// 得到刀具类型分页列表数据，传的时候，参数放在了url后面
export function GetList(data) {
  return request({
    url: '/cuttermodel/list',
    method: 'get',
    params: data
  })
}
export function GetTypeWithModelTree(data) {
  return request({
    url: '/cuttermodel/GetTypeWithModelTree',
    method: 'post',
    data
  })
}

// 根据类型Id获取父子类型下的所有Model
export function OneCutterList(data) {
  var id = data
  return request({
    url: `/cuttermodel/getModelByTypeId?parentId=${id}`,
    method: 'get'
    // params: data
  })
}

// 更新刀具类型信息
export function Update(data) {
  return request({
    url: '/cuttermodel/update',
    method: 'post',
    data
  })
}

// 根据刀具类型ID 删除刀具类型 可批量删除
export function Delete(data) {
  return request({
    url: '/cuttermodel/delete',
    method: 'post',
    data
  })
}

// 新建刀具类型信息
export function Save(data) {
  return request({
    url: '/cuttermodel/save',
    method: 'post',
    data
  })
}

// 根据ID得到材料类型明细信息
export function GetDetail(data) {
  return request({
    url: '/cuttermodel/GetDetail',
    method: 'post',
    data
  })
}

// 根据传入的material_type_id查找所有material的属性有该materialTypeId的元素
export function GetAllByMaterialTypeName(data) {
  return request({
    url: '/cuttermodel/GetAllBy_materialTypeName',
    method: 'post',
    data
  })
}

// 根据传入的id和分页数据找叶
export function GetLeaf(data, params) {
  return request({
    url: '/cuttermodel/GetLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}

// 刀具类型下的刀具界面的 左上角的 搜索框 功能:输入code或者name,根据code或者name查询 选中刀具类型时，会传入type_id，不选中时传入的dept_id=null
export function SearchBar(data) {
  return request({
    url: '/cuttermodel/SearchBar',
    method: 'post',
    data
  })
}
