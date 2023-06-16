/*
 * @Author: your name
 * @Date: 2021-09-13 10:46:40
 * @LastEditTime: 2021-11-18 20:31:41
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \Mes_Vue\src\api\Login.js
 */
import request from '@/utils/request'
import { getToken } from '@/utils/auth' // 验权
//登录接口
export function login(username, password) {
  return request({
    url: '/Check/Login',
    method: 'post',
    data: {
      Account: username,
      Password: password,
      AppKey: 'openauth'
    }
  })
}
//根据token获取用户名称
export function getInfo(token) {
  return request({
    url: '/Check/GetUserName',
    method: 'get',
    params: { token }
  })
}
//获取权限菜单
export function getModules() {
  return request({
    url: '/Check/GetRight',
    method: 'get',
    params: { Authorization: getToken() }
  })
}
//注销登录
export function logout() {
  return request({
    url: '/Check/Logout',
    method: 'post'
  })
}
