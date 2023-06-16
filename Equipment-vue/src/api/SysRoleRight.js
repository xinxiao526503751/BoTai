/*
 * @Author: your name
 * @Date: 2021-09-3 10:01:44
 * @LastEditTime: 2021-09-4 10:20:57
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\SysSystem.js
 */
import request from '@/utils/request'

// 根据角色返回树结构用于给角色权限页面菜单栏 默认不选定任何按钮
export function ObtainNodesByRole(params) {
  return request({
    url: '/SysRoleRight/ObtainNodesByRole',
    method: 'post',
    params: {
      roleId: params
    }
  })
}
//给角色赋权限
export function GiveRolePermissions(data, params) {
  return request({
    url: '/SysRoleRight/GiveRolePermissions',
    method: 'post',
    data,
    params: {
      roleId: params
    }
  })
}
// 根据角色ID获取左边菜单栏
export function ObtainSysRoleNodesByRoleId(params) {
  return request({
    url: '/SysRoleRight/ObtainSysRoleNodesByRoleId',
    method: 'post',
    params: {
      roleId: params
    }
  })
}
// 通过RoleId返回rightId数组
export function ObtainSysRoleByRoleId(params) {
  return request({
    url: '/SysRoleRight/ObtainSysRoleByRoleId',
    method: 'post',
    params: {
      roleId: params
    }
  })
}
