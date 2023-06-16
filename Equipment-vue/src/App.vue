<!-- 刷新当前页面，并且防止出现白屏现象 -->
<template>
  <div id="app">
    <!-- 路由展示 -->
    <router-view v-if="isRouterAlive" />
  </div>
</template>

<script>
export default {
  name: 'App',
  // 解决props进行逐级传递数据的问题，可进行跨级传递数据
  provide() {
    return {
      reload: this.reload
    }
  },

  data() {
    return {
      isRouterAlive: true
    }
  },

  methods: {
    reload() {
      this.isRouterAlive = false
      this.$nextTick(function() {
        this.isRouterAlive = true
      })
    }
  }
}
</script>

<style></style>
