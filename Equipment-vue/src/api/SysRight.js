/*
 * @Author: your name
 * @Date: 2021-08-13 10:01:44
 * @LastEditTime: 2021-07-14 10:20:57
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\SysRight.js
 */
import request from '@/utils/request'

// 新增系统数据
export function Create(data) {
  return request({
    url: '/SysRight/Create',
    method: 'post',
    data
  })
}
// 更新系统数据
export function Update(data) {
  return request({
    url: '/SysRight/Update',
    method: 'post',
    data
  })
}
//删除系统数据
export function Delete(data) {
  return request({
    url: '/SysRight/Delete',
    method: 'post',
    data
  })
}
//获取系统数据，返回树格式
export function GetSysNodes() {
  return request({
    url: '/SysRight/GetSysNodes',
    method: 'post'
  })
}
//根据系统ID获取权限数据
export function GetRightBySysId(data, params) {
  return request({
    url: '/SysRight/GetRightBySysId',
    method: 'post',
    data,
    params: {
      SysId: params
    }
  })
}
export function SearchRightBySysId(data, params) {
  return request({
    url: '/SysRight/SearchRightBySysId',
    method: 'post',
    data,
    params: {
      SysId: params
    }
  })
}

//根据权限级ID获取按钮级数据
export function GetButtonByRightId(data, params) {
  return request({
    url: '/SysRight/GetButtonByRightId',
    method: 'post',
    data,
    params: {
      rightId: params
    }
  })
}
//返回树结构用于给角色权限页面菜单栏
export function ObtainAllNodes() {
  return request({
    url: '/SysRight/ObtainAllNodes',
    method: 'post'
  })
}
