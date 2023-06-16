<!--
 * @Author: your name
 * @Date: 2021-09-06 11:15:20
 * @LastEditTime: 2021-09-06 16:35:44
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\components\Description\DesDemo.vue
-->
<template>
  <div class="des">
    <e-desc
      v-for="item in info"
      ref="des"
      :data="info"
      margin="0 12px"
      label-width="100px"
      @click.native="handleSelect"
    >
      <e-desc-item label="姓名">{{ item.name }}</e-desc-item>
      <e-desc-item label="年龄">{{ item.age }}岁</e-desc-item>
      <e-desc-item label="性别">{{ item.sex }}</e-desc-item>
      <e-desc-item label="学校">{{ item.school }}</e-desc-item>
      <e-desc-item label="专业">{{ item.major }}</e-desc-item>
      <e-desc-item label="爱好">{{ item.hobby }}</e-desc-item>
      <e-desc-item label="手机号">{{ item.phone }}</e-desc-item>
      <e-desc-item label="微信">{{ item.wx }}</e-desc-item>
      <e-desc-item label="QQ">{{ item.qq }}</e-desc-item>
      <e-desc-item label="住址">{{ item.address }}</e-desc-item>
      <e-desc-item label="自我描述" :span="2">{{ item.intro }}</e-desc-item>
      <!-- <e-desc-item label="操作" :span="3">
        <template slot="content">
          <el-button size="small" type="primary">修改</el-button>
          <el-button size="small" type="danger">删除</el-button>
        </template>
      </e-desc-item> -->
    </e-desc>
  </div>
</template>

<script>
import EDesc from './EDesc.vue'
import EDescItem from './EDescItem.vue'
export default {
  name: 'desdemo',
  components: {
    EDesc,
    EDescItem
  },
  data() {
    return {
      info: [
        {
          name: 'Jerry',
          age: 26,
          sex: '男',
          school: '四川大学',
          major: '码农专业',
          address: '四川省成都市',
          hobby: '搬砖、前端、赚钱',
          phone: 18888888888,
          wx: 'Nice2cu_Hu',
          qq: 332983810,
          intro:
            '我是一个粉刷匠，粉刷本领强。我要把那新房子，刷得更漂亮。刷了房顶又刷墙，刷子飞舞忙。哎呀我的小鼻子，变呀变了样。我是一个粉刷匠，粉刷本领强。我要把那新房子，刷得更漂亮。刷了房顶又刷墙，刷子飞舞忙。哎呀我的小鼻子，变呀变了样。'
        },
        {
          name: 'Jerry',
          age: 26,
          sex: '男',
          school: '四川大学',
          major: '码农专业',
          address: '四川省成都市',
          hobby: '搬砖、前端、赚钱',
          phone: 18888888888,
          wx: 'Nice2cu_Hu',
          qq: 332983810,
          intro:
            '我是一个粉刷匠，粉刷本领强。我要把那新房子，刷得更漂亮。刷了房顶又刷墙，刷子飞舞忙。哎呀我的小鼻子，变呀变了样。我是一个粉刷匠，粉刷本领强。我要把那新房子，刷得更漂亮。刷了房顶又刷墙，刷子飞舞忙。哎呀我的小鼻子，变呀变了样。'
        }
      ],
      temp: []
    }
  },
  methods: {
    handleSelect() {
      this.temp = this.$refs.des.data
      console.log(this.temp)
    }
  }
}
</script>
