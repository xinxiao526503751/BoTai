/*
 * @Author: your name
 * @Date: 2021-09-08 15:27:35
 * @LastEditTime: 2021-09-08 15:47:18
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \Mes_Vue\src\api\SysUserRole.js
 */
import request from '@/utils/request'

//给角色赋权限
export function GetRolesByUserId(params) {
  return request({
    url: '/SysUserRole/GetRolesByUserId',
    method: 'post',
    params: {
      user_id: params
    }
  })
}
