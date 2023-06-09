import router from './router'
import store from './store'
import NProgress from 'nprogress' // Progress 进度条
import 'nprogress/nprogress.css' // Progress 进度条样式
import { getToken } from '@/utils/auth' // 验权

// 路由守卫的配置
const whiteList = ['/login'] // 不重定向白名单
router.beforeEach((to, from, next) => {
  NProgress.start()
  if (getToken()) {
    if (to.path === '/login') {
      // 登录后login自动跳转
      next({ path: '/' })
      return
    }
    if (store.getters.modules != null) {
      next()
      return
    }

    store
      .dispatch('GetInfo')
      .then(res => {
        // 拉取用户信息
        store.dispatch('GetModules').then(modules => {
          // 获取用户可访问的模块
          store.dispatch('GenerateRoutes', { modules }).then(() => {
            // 根据权限生成可访问的路由表

            router.addRoutes(store.getters.addRouters) // 动态添加可访问路由表
            next({ ...to, replace: true }) // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
          })
        })
      })
      .catch(err => {
        store.dispatch('FedLogOut').then(() => {
          Message.error(err || '获取用户信息失败')
          next({ path: '/' })
        })
      })

    return
  }
  if (whiteList.indexOf(to.path) !== -1) {
    // 没登录情况下过滤白名单
    next()
  } else {
    next('/login')
  }
  // if (store.getters.token) {
  //   // 判断是否有token
  //   if (to.path === '/login') {
  //     //如果登录成功，重定向到主页面
  //     next({ path: '/' })
  //     NProgress.done()
  //   } else {
  //     //router.beforeEach
  //     if (store.getters.modules == null) {
  //       // 判断当前用户是否已拉取完user_info信息
  //       store
  //         .dispatch('GetInfo')
  //         .then(res => {
  //           // 拉取info
  //           const roles = res.data.role
  //           store.dispatch('GenerateRoutes', { roles }).then(() => {
  //             // 生成可访问的路由表
  //             router.addRoutes(store.getters.addRouters) // 动态添加可访问路由表
  //             next({ ...to, replace: true }) // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
  //           })
  //         })
  //         .catch(err => {
  //           store.dispatch('FedLogOut').then(() => {
  //             Message.error(err || '获取用户信息失败')
  //             next({ path: '/' })
  //           })
  //         })
  //     } else {
  //       next() //当有用户权限的时候，说明所有可访问路由已生成 如访问没权限的全面会自动进入404页面
  //     }
  //   }
  // } else {
  //   if (whiteList.indexOf(to.path) !== -1) {
  //     // 在免登录白名单，直接进入
  //     next()
  //   } else {
  //     next('/login') // 否则全部重定向到登录页
  //   }
  // }
})
router.afterEach(() => {
  NProgress.done() // 结束Progress
})
