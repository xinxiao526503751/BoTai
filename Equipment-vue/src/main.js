/*
 * @Author: your name
 * @Date: 2021-06-11 15:09:17
 * @LastEditTime: 2022-08-06 22:06:02
 * @LastEditors: chenyun625 chenyun625@outlook.com
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\main.js
 */
// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import store from './store'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
// 导入全局样式
import '@/styles/index.scss'
import './permission' // permisssion
import './icons' // icons
// import './utils/flexible'
import * as filters from './filters' // global filters
import * as echarts from 'echarts'

// register global utility filters
Object.keys(filters).forEach(key => {
  Vue.filter(key, filters[key])
})
// import { formatTime, parseTime } from '@/utils/index'
// import Moment from 'moment'

// Vue.filter('date', formatTime)
// Vue.filter('comverTime', function(data, format) {
//   return Moment.unix(data).format(format)
// })
Vue.use(ElementUI)
// 将echarts添加到Vue实例上面，可以通过this来调用echarts
Vue.prototype.$echarts = echarts
// 关闭生产提示
Vue.config.productionTip = false
// 调试工具
Vue.config.devtools = true

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  components: { App },
  template: '<App/>',
  beforeCreate() {
    Vue.prototype.$bus = this; //安装全局事件总线，这个事件在Vue的原型上
}
})
