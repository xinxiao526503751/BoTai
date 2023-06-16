/*
 * @Author: your name
 * @Date: 2021-10-20 09:27:40
 * @LastEditTime: 2021-11-18 17:21:59
 * @LastEditors: your name
 * @Description: 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 * @FilePath: \Mes_Vue\src\store\modules\permission.js
 */
import { constantRouterMap } from '@/router'
import Layout from '@/views/Layout'
const groupRoutes = data => {
  const parentPath = data.url
  var newPath = {
    path: parentPath || '/',
    // component: data.children && data.children.length > 0 ? Layout : () => import('@/views' + data.item.url.toLowerCase()),
    component:
      data.children && data.children.length > 0
        ? data && data.parentId === null
          ? Layout
          : Layout
        : () => import('@/views' + data.url.toLowerCase()),
    meta: {
      title: data.name
      // sortNo: data.item.sortNo,
      // elements: (data.item && data.item.elements) || ''
    },
    name: data.name,
    hidden: false,
    children: []
  }
  if (data.children && data.children.length > 0) {
    data.children.forEach(child => {
      newPath.children.push(groupRoutes(child))
    })
  }
  return newPath
}
const permission = {
  state: {
    routers: constantRouterMap,
    addRouters: []
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      state.addRouters = routers
      state.routers = constantRouterMap.concat(routers)
    }
  },
  actions: {
    GenerateRoutes({ dispatch, commit }, modules) {
      return new Promise(resolve => {
        ;(async () => {
          const newPaths = []
          var mod = Object.values(modules) // 将对象转为数组
          // console.log('module:', mod[0].Result)
          await mod[0].Result.forEach((value, index) => {
            newPaths.push(groupRoutes(value))
          })
          commit('SET_ROUTERS', newPaths)
          resolve(newPaths)
        })()
      })
    }
  }
}

export default permission
