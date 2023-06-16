/*
 * @Author: gcy
 * @Date: 2021-09-21 16:18:22
 * @LastEditTime: 2021-09-23 09:22:27
 * @LastEditors: Please set LastEditors
 * @Description: 系统参数接口
 * @FilePath: \Mes_Vue\src\api\SysParm.js
 */
import request from '@/utils/request'

// 得到系统参数分页列表数据
export function GetList(data) {
  return request({
    url: '/SysParm/GetList',
    method: 'post',
    data
  })
}
// 新建系统参数信息
export function Insert(data) {
  return request({
    url: '/SysParm/Insert',
    method: 'post',
    data
  })
}
// 更新系统参数信息
export function Update(data) {
  return request({
    url: '/SysParm/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/SysParm/Delete',
    method: 'post',
    data
  })
}

// 通过参数类型名称获取参数类型下的参数
export function GetParamByParamType(data, ParamTypeName) {
  return request({
    url: '/SysParm/GetParamByParamType',
    method: 'post',
    data,
    params: {
      ParamTypeName: ParamTypeName
    }
  })
}
