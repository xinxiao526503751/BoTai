<template>
  <!--background-color="#112f50" 导航栏背景颜色
      text-color="#fff" 导航栏内文字颜色
      active-text-color="#ffd04b" 点击导航栏显示的文字高亮颜色
      :default-active="this.$route.path" 动态获得点击的页面的路由地址
    -->
  <el-menu
    :default-active="this.$route.path"
    router
    class="el-menu-vertical-demo"
    background-color="#112364"
    text-color="#fff"
    active-text-color="#ffd04b"
    unique-opened
  >
    <!-- <el-menu-item index="/home">
      <i class="el-icon-s-home"></i>
      <span slot="title">系统首页</span>
    </el-menu-item> -->
    <el-menu-item @click="Jump">
      <i class="el-icon-s-home"></i>
      <span slot="title">系统首页</span>
    </el-menu-item>
    <el-menu-item index="/equipmentInformation">
      <template slot="title">
        <i class="el-icon-tickets"></i>
        <span slot="title">设备信息</span>
      </template>
    </el-menu-item>
    <el-submenu index="/equipmentCheck">
      <template slot="title">
        <i class="el-icon-search"></i>
        <span slot="title">设备点检</span>
      </template>
      <el-menu-item index="/equipmentCheck/checkItems">
        <template slot="title">
        <span slot="title">点检项目</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentCheck/checkPlan">
        <template slot="title">
        <span slot="title">点检计划</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentCheck/equCheckItems">
        <template slot="title">
        <span slot="title">设备点检项</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentCheck/checkRecord">
        <template slot="title">
        <span slot="title">点检记录</span>
      </template>
      </el-menu-item>
    </el-submenu>
    <el-submenu index="/equipmentUpkeep">
      <template slot="title">
        <i class="el-icon-brush"></i>
        <span slot="title">设备保养</span>
      </template>
      <el-menu-item index="/equipmentUpkeep/upkeepItems">
        <template slot="title">
        <span slot="title">保养项目</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentUpkeep/upkeepPlan">
        <template slot="title">
        <span slot="title">保养计划</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentUpkeep/equUpkeepItems">
        <template slot="title">
        <span slot="title">设备保养项</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentUpkeep/upkeepRecord">
        <template slot="title">
        <span slot="title">保养记录</span>
      </template>
      </el-menu-item>
    </el-submenu>
    <el-submenu index="/equipmentMaintenance">
      <template slot="title">
        <i class="el-icon-s-tools"></i>
        <span slot="title">设备维修</span>
      </template>
      <el-menu-item index="/equipmentMaintenance/maintainItems">
        <template slot="title">
        <span slot="title">维修项目</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentMaintenance/EquMaintainItems">
        <template slot="title">
        <span slot="title">设备维修项</span>
      </template>
      </el-menu-item>
      <el-menu-item index="/equipmentMaintenance/maintainRecord">
        <template slot="title">
        <span slot="title">维修记录</span>
      </template>
      </el-menu-item>
    </el-submenu>
  </el-menu>
</template>
  
  <script>
export default {
  name: 'SideNav',
  methods: {
    Jump(){
      let routeUrl=this.$router.resolve({
        path:'/home'
      })
      window.open(routeUrl.href,'_blank')
    }
  },
}
</script>
  <style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  min-height: 400px;
}
.el-menu {
  border-right: none;
}
.el-menu .is-active {
  color: #fff !important;
}
</style>