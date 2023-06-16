<!--
 * @Author: gcy
 * @Date: 2021-09-27 08:59:12
 * @LastEditTime: 2021-09-27 09:35:40
 * @LastEditors: Please set LastEditors
 * @Description: 设备分类树
 * @FilePath: \Mes_Vue\src\components\EqptClassTree\index.vue
-->
<template>
  <div class="tree">
    <TreeSelect
      ref="eqptClassTree"
      :isCheck="isCheck"
      :isShowExpand="isShowExpand"
      :isShowFilter="isShowFilter"
      :multiple="multiple"
      :treeProps="props"
      :treeList="eqptClassTree"
      :accordion="accordion"
      :expandNode="expandNode"
      @handleNodeClick="handleNodeClickEqptClass"
    ></TreeSelect>
  </div>
</template>

<script>
import { listToTreeSelect } from '@/utils'
import TreeSelect from '@/components/TreeSelect'
import * as BaseEquipmentClass from '@/api/BaseEquipmentClass'

export default {
  name: 'EqptClassTree',
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
      eqptclass: [], // 用户可访问到的列表
      eqptClassTree: [] // 用户可访问到的所有设备分类组成的树
    }
  },
  mounted() {
    this.getEqptClassTree()
  },
  methods: {
    handleNodeClickEqptClass(data) {
      this.$emit('handleNodeClickEqptClass', data)
    },
    //获取列表树
    getEqptClassTree() {
      var _this = this // 记录vuecomponent
      BaseEquipmentClass.GetAll().then(response => {
        _this.eqptclass = response.Result.map(function(item, index, input) {
          return {
            id: item.equipment_class_id,
            label: item.equipment_class_name,
            parentId: item.equipment_class_parentid || null
          }
        })
        var eqpttmp = JSON.parse(JSON.stringify(_this.eqptclass))
        _this.eqptClassTree = listToTreeSelect(eqpttmp)
      })
    }
  }
}
</script>
