<!--
 * @Author: gcy
 * @Date: 2021-09-26 16:21:43
 * @LastEditTime: 2021-09-26 19:55:18
 * @LastEditors: Please set LastEditors
 * @Description: 地点树
 * @FilePath: \Mes_Vue\src\components\LocationTree\index.vue
-->
<template>
  <div class="tree">
    <div class="app-container flex-item">
      <TreeSelect
        ref="locationTree"
        :isCheck="isCheck"
        :isShowExpand="isShowExpand"
        :isShowFilter="isShowFilter"
        :multiple="multiple"
        :treeProps="props"
        :treeList="locationsTree"
        :accordion="accordion"
        :expandNode="expandNode"
        @handleNodeClick="handleNodeClickLocation"
      ></TreeSelect>
    </div>
  </div>
</template>

<script>
import { listToTreeSelect } from '@/utils'
import TreeSelect from '@/components/TreeSelect'
import * as BaseLocation from '@/api/BaseLocation'

export default {
  name: 'LocationTree',
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
        // pid: 'parentId',
        children: 'children'
        // disabled:true
      },
      locations: [], // 用户可访问到的地点列表
      locationsTree: [] // 用户可访问到的地点所有组成的树
    }
  },
  mounted() {
    this.getLocationTree()
  },
  methods: {
    handleNodeClickLocation(data) {
      this.$emit('handleNodeClickLocation', data)
    },
    // 获取地点的树
    getLocationTree() {
      var _this = this // 记录vuecomponent
      BaseLocation.GetAll().then(response => {
        _this.locations = response.Result.map(function(item, index, input) {
          return {
            id: item.location_id,
            label: item.location_name,
            parentId: item.location_parentid || null
          }
        })
        var locationtmp = JSON.parse(JSON.stringify(_this.locations))
        _this.locationsTree = listToTreeSelect(locationtmp)
      })
    }
  }
}
</script>
