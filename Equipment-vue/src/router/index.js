import Vue from 'vue'
import Router from 'vue-router'
// 先使用一次路由
Vue.use(Router)

import Layout from '@/views/Layout'

// 对push原型方法的重写
const originalPush = Router.prototype.push
Router.prototype.push = function push(location, onResolve, onReject) {
  if (onResolve || onReject)
    return originalPush.call(this, location, onResolve, onReject)
  return originalPush.call(this, location).catch(err => err)
}
export const constantRouterMap = [
  {
    path: '/login',
    component: () => import('@/views/Login/index'),
    hidden: true  //隐藏
  },
  {
    path: '/home',
    name: 'home',
    meta:{title:'首页'},
    component: resolve => require(['@/views/Home/index.vue'], resolve)
  },
  // {
  //   path: '///',
  //   name: 'test',
  //   meta:{title:'测试'},
  //   component: resolve => require(['@/views/Test/index.vue'], resolve)
  // },
  {
    path: '/',
    name: 'layout',
    component: Layout,
    meta:{title:'布局'},
    children: [
      {
        path: 'infoMaintain',
        name: 'infoMaintain',
        meta:{title:'设备信息维护'},
        component: resolve => require(['@/views/InfoMaintain/index.vue'], resolve)
      },
      {
        path: 'equipmentInformation',
        name: 'equipmentInformation',
        meta:{title:'设备信息'},
        component: resolve => require(['@/views/EquipmentInformation/index.vue'], resolve)
      },
      {
        path: 'equipmentCheck',
        name: 'equipmentCheck',
        meta:{title:'设备点检'},
        component: resolve => require(['@/views/EquipmentCheck/index.vue'], resolve),
        children:[
          {
            path: 'checkItems',
            name: 'checkItems',
            meta:{title:'点检项目'},
            component: resolve => require(['@/views/EquipmentCheck/CheckItems/index.vue'], resolve),
          },
          {
            path: 'equCheckItems',
            name: 'equCheckItems',
            meta:{title:'设备点检项'},
            component: resolve => require(['@/views/EquipmentCheck/EquCheckItems/index.vue'], resolve),
          },
          {
            path: 'checkPlan',
            name: 'checkPlan',
            meta:{title:'点检项目'},
            component: resolve => require(['@/views/EquipmentCheck/CheckPlan/index.vue'], resolve),
          },
          {
            path: 'checkRecord',
            name: 'checkRecord',
            meta:{title:'点检项目'},
            component: resolve => require(['@/views/EquipmentCheck/CheckRecord/index.vue'], resolve),
          }
        ]
      },
      {
        path: 'equipmentUpkeep',
        name: 'equipmentUpkeep',
        meta:{title:'设备保养'},
        component: resolve => require(['@/views/EquipmentUpkeep/index.vue'], resolve),
        children:[
          {
            path: 'upkeepItems',
            name: 'upkeepItems',
            meta:{title:'保养项目'},
            component: resolve => require(['@/views/EquipmentUpkeep/UpkeepItems/index.vue'], resolve),
          },
          {
            path: 'equUpkeepItems',
            name: 'equUpkeepItems',
            meta:{title:'设备点检项'},
            component: resolve => require(['@/views/EquipmentUpkeep/EquUpkeepItems/index.vue'], resolve),
          },
          {
            path: 'upkeepPlan',
            name: 'upkeepPlan',
            meta:{title:'点检项目'},
            component: resolve => require(['@/views/EquipmentUpkeep/UpkeepPlan/index.vue'], resolve),
          },
          {
            path: 'upkeepRecord',
            name: 'upkeepRecord',
            meta:{title:'点检项目'},
            component: resolve => require(['@/views/EquipmentUpkeep/UpkeepRecord/index.vue'], resolve),
          }
        ]
      },
      {
        path: 'equipmentMaintenance',
        name: 'equipmentMaintenance',
        meta:{title:'设备维修'},
        component: resolve => require(['@/views/EquipmentMaintenance/index.vue'], resolve),
        children:[
          {
            path: 'maintainItems',
            name: 'maintainItems',
            meta:{title:'维修项目'},
            component: resolve => require(['@/views/EquipmentMaintenance/MaintainItems/index.vue'], resolve),
          },
          {
            path: 'equMaintainItems',
            name: 'equMaintainItems',
            meta:{title:'设备维修项'},
            component: resolve => require(['@/views/EquipmentMaintenance/EquMaintainItems/index.vue'], resolve),
          },
          {
            path: 'maintainRecord',
            name: 'maintainRecord',
            meta:{title:'维修记录'},
            component: resolve => require(['@/views/EquipmentMaintenance/MaintainRecord/index.vue'], resolve),
          }
        ]
      }
    ]
  }
]

var router = new Router({
  routes: constantRouterMap
})
export default router
