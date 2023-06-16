<template>
  <div class="layout">
    <div class="header">
      <Header></Header>
    </div>
    <div class="sideNav">
      <SideNav></SideNav>
    </div>
    <div class="content">
        <!-- <tag-view class="custom-tags-view"></tag-view> -->
        <Content></Content>
    </div>
  </div>
</template>


<script>
import TagView from '@/views/Layout/TagsView'
import SideNav from '@/views/Layout/SideNav'
import Content from '@/views/Layout/Content'
import Header from '@/views/Layout/Header'

export default {
  name: 'Layout',
  components: {
    SideNav,
    Content,
    Header,
    TagView
  }
}
</script>

<style lang="scss" scoped>
.layout {
  width: 100%;
  height: 100%;
  .header {
    width: 100%;
    height: 54px;
  }
  .sideNav {
    position: fixed;
    top: 54px;
    left: 0;
    width: 200px;
    height: calc(100% - 54px);
    background-color: #112364;
  }
  .content{
    position: fixed;
    padding: 10px 10px 0 10px;
    top: 54px;
    left: 200px;
    width: calc(100% - 200px); //calc()计算属性在写得时候，计算符号两边必须存在空格
    height: calc(100% - 54px);
    background-color: #eee;
  }
}
</style>