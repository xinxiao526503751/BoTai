/*
 * @Author: your name
 * @Date: 2021-10-20 09:27:40
 * @LastEditTime: 2022-03-21 14:21:49
 * @LastEditors: Please set LastEditors
 * @Description: 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 * @FilePath: \Mes_Vue\src\store\getters.js
 */
const getters = {
  sidebar: state => state.app.sidebar,
  device: state => state.app.device,
  token: state => state.user.token,
  name: state => state.user.name,
  // defaultorgid: state => state.user.defaultorg.id,
  // isIdentityAuth: state => state.serverConf.isIdentityAuth,
  modules: state => state.user.modules,
  visitedViews: state => state.tagsView.visitedViews,
  cachedViews: state => state.tagsView.cachedViews,
  addRouters: state => state.permission.addRouters,
  permission_routers: state => state.permission.routers
}
export default getters
