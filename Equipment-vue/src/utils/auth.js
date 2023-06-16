/*
 * @Author: your name
 * @Date: 2021-09-14 14:41:14
 * @LastEditTime: 2021-09-16 14:16:05
 * @LastEditors: your name
 * @Description: In User Settings Edit
 * @FilePath: \Mes_Vue\src\utils\auth.js
 */
import Cookies from 'js-cookie'

const TokenKey = 'Authorization'
// 将token存贮到cookie中，保证刷新页面后能记住用户登录状态
export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token) {
  return Cookies.set(TokenKey, token)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}
