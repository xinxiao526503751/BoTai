<!--
 * @Author: cy
 * @Date: 2021-09-26 19:50:02
 * @LastEditTime: 2022-08-05 16:01:13
 * @LastEditors: chenyun625 chenyun625@outlook.com
 * @Description: 刀具类型树
 * @FilePath: \Mes_Vue\src\components\CutterTypeTree\index.vue
-->
<template>
  <div class="tree">
    <!-- <div class="app-container flex-item"> -->
    <TreeSelect
      ref="cutterTypeTree"
      :isCheck="isCheck"
      :isShowExpand="isShowExpand"
      :isShowFilter="isShowFilter"
      :multiple="multiple"
      :treeProps="props"
      :treeList="cutterTypeTree"
      :accordion="accordion"
      :expandNode="expandNode"
      @handleNodeClick="handleNodeClickCutterType"
    ></TreeSelect>
    <!-- </div> -->
  </div>
</template>

<script>
import { listToTreeSelect } from '@/utils'
import TreeSelect from '@/components/TreeSelect'
import * as CutterManager from '@/api/CutterManager'

export default {
  name: 'CutterTypeTree',
  components: {
    TreeSelect
  },
  props: {
    // 是否显示全选按钮
    isCheck: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否显示展开收缩按钮
    isShowExpand: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否需要关键字过滤
    isShowFilter: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否可多选，默认单选
    multiple: {
      type: Boolean,
      default() {
        return false
      }
    },
    // 自动收起
    accordion: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否在点击节点的时候展开或者收缩节点， 默认值为 true，如果为 false，则只有点箭头图标的时候才会展开或者收缩节点。
    expandNode: {
      type: Boolean,
      default() {
        return true
      }
    }
  },
  data() {
    return {
      props: {
        // 配置项（必选）
        id: 'id',
        label: 'label',
        parentId: 'parentId',
        children: 'children'
        // disabled: true
      },
      cutters: [], // 获取刀具列表
      cutterTypeTree: [] // 获取刀具类型树
    }
  },
  mounted() {
    this.getCutterTypeTree()
  },
  methods: {
    handleNodeClickCutterType(data) {
      // 绑定和触发自定义事件(子组件的任务)
      this.$emit('handleNodeClickCutterType', data)
    },
    getCutterTypeTree() {
      // 获取刀具列表树
      var _this = this // 记录vuecomponent
      // CutterManager.GetTypeTree().then(response => {
      //   _this.cutters = response.cutterTypeTree.map(function(
      //     item,
      //     index,
      //     input
      //   ) {
      //     return {
      //       id: item.id,
      //       label: item.label,
      //       parentId: item.parentId || null,
      //       children: item.children
      //     }
      //   })
      //   var cuttertypestmp = JSON.parse(JSON.stringify(_this.cutters))
      //   _this.cutterTypeTree = listToTreeSelect(cuttertypestmp)
      // })
    }
  }
}
</script>
