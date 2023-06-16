<!--
 * @Author: your name
 * @Date: 2021-08-26 15:38:26
 * @LastEditTime: 2021-09-26 17:10:11
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\components\TreeSelect\index.vue
-->
<!--
变量：
 expandNode：是否展开所有节点
 isShowFilter: 是否需要节点过滤
 treeProps: 树形结构配置项
事件：
 @handleNodeClick: 节点被点击时的回调
 @handleCheck：当复选框被点击的时候触发
 @handleCheckChange：节点选中状态发生变化时的回调
方法：同element-ui
getCurrentKey
getCurrentNode
setCurrentKey
getCheckedKeys
getCheckedNodes

-->
<template>
  <div class="baseTree">
    <div class="header">
      <el-input
        v-if="isShowFilter"
        size="small"
        style="width:80%;"
        :placeholder="placeholder"
        v-model="filterText"
      >
      </el-input>
      <el-button
        v-if="isShowExpand"
        size="mini"
        style="width:5%;border: none;padding:0px"
        @click="onExpand"
      >
        <svg
          :class="{ 'is-active': isActive }"
          class="hamburger"
          viewBox="0 0 1024 1024"
          xmlns="http://www.w3.org/2000/svg"
          width="64"
          height="64"
        >
          <path
            d="M408 442h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zm-8 204c0 4.4 3.6 8 8 8h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56zm504-486H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zm0 632H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zM142.4 642.1L298.7 519a8.84 8.84 0 0 0 0-13.9L142.4 381.9c-5.8-4.6-14.4-.5-14.4 6.9v246.3a8.9 8.9 0 0 0 14.4 7z"
          />
        </svg>
      </el-button>
    </div>
    <el-checkbox
      v-model="checkAll"
      v-show="isCheck"
      @change="handleCheckAllChange"
      >全选</el-checkbox
    >
    <el-tree
      ref="baseTree"
      :data="treeList"
      :node-key="treeProps.id"
      :props="treeProps"
      :highlight-current="highlight"
      :accordion="accordion"
      :default-expand-all="expand"
      :default-expanded-keys="expandedKeys"
      :auto-expand-parent="expandParent"
      :expand-on-click-node="expandNode"
      :show-checkbox="multiple"
      :check-strictly="checkStrictly"
      :filter-node-method="filterNode"
      @node-click="handleNodeClick"
      @node-expand="handleNodeExpand"
      @node-collapse="handleNodeCollapse"
      @check="handleCheck"
      @check-change="handleCheckChange"
      @node-contextmenu="handleNodeContextMenu"
    >
    </el-tree>
  </div>
</template>

<script>
export default {
  name: 'TreeSelect',
  props: {
    // treeList: {
    //   type: Array,
    //   default () {
    //     return [];
    //   }
    // },
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
    // 默认树形结构配置项
    treeProps: {
      type: Object,
      default() {
        return {
          id: 'id', // Number类型：树组件ID(node-key)
          label: 'label', // String类型 : 树组件显示名称
          pid: 'parentId', // Number类型：父级ID
          children: 'children' // String类型：子节点
        }
      }
    },
    // 自动收起
    accordion: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否展开所有节点，默认展开
    expand: {
      type: Boolean,
      default() {
        return true
      }
    },
    // 展开子节点的时候是否自动展开父节点 默认值为 true
    expandParent: {
      type: Boolean,
      default() {
        return true
      }
    },
    // 是否在点击节点的时候展开或者收缩节点， 默认值为 true，如果为 false，则只有点箭头图标的时候才会展开或者收缩节点。
    expandNode: {
      type: Boolean,
      default() {
        return true
      }
    },
    // 默认展开的节点的 key 的数组
    expandedKeys: {
      type: Array,
      default() {
        return []
      }
    },
    // 是否高亮当前选中节点，默认值是 true
    highlight: {
      type: Boolean,
      default() {
        return true
      }
    },
    // 是否可多选，默认单选
    multiple: {
      type: Boolean,
      default() {
        return false
      }
    },
    // 显示复选框情况下，是否严格遵循父子不互相关联
    checkStrictly: {
      type: Boolean,
      default() {
        return false
      }
    },
    // 图标url
    iconUrl: {
      type: String,
      default() {
        return ''
      }
    },
    placeholder: {
      type: String,
      default: () => {
        return '输入查询关键字'
      }
    },
    // 是否需要关键字过滤
    isShowFilter: {
      type: Boolean,
      default: () => {
        return false
      }
    }
  },
  data() {
    return {
      filterText: '',
      visible: false,
      checkAll: false, // 默认不全选
      isActive: false // 树折叠按钮默认
    }
  },
  methods: {
    // 展开收缩事件树
    onExpand() {
      let task = new Promise((resolve, reject) => {
        this.isActive = !this.isActive
        resolve()
      }).then(() => {
        console.log(this.isActive)
        this.treeNodeExpand(this.isActive)
      })
    },
    // 这是个耗时操作,所以需要异步操作
    treeNodeExpand(status) {
      for (
        let i = 0;
        i < this.$refs.baseTree.store._getAllNodes().length;
        i++
      ) {
        this.$refs.baseTree.store._getAllNodes()[i].expanded = status
      }
    },
    /**
     * @description: 对树节点进行筛选时执行的方法，返回 true 表示这个节点可以显示，返回 false 则表示这个节点会被隐藏
     * @param {*} value
     * @param {*} data
     * @return {*}
     */
    handleCheckAllChange(val) {
      if (this.checkAll) {
        this.$refs.baseTree.setCheckedNodes(this.treeList)
      } else {
        this.$refs.baseTree.setCheckedKeys([])
      }
    },
    /**
     * @description: 对树节点进行筛选时执行的方法，返回 true 表示这个节点可以显示，返回 false 则表示这个节点会被隐藏
     * @param {*} value
     * @param {*} data
     * @return {*}
     */
    filterNode(value, data) {
      if (!value) return true
      return data[this.treeProps.label].indexOf(value) !== -1
    },
    /**
     * @description: 节点被展开时触发的事件
     * @param {*} data         该节点所对应的数据对象
     * @param {*} node         节点对应的Node对象
     * @param {*} vueComponent 节点组件本身
     * @return {*}
     */
    handleNodeExpand(data, node, vueComponent) {
      this.$emit('handleNodeExpand', data)
    },
    /**
     * @description: 节点被关闭时触发的事件
     * @param {*} data         该节点所对应的数据对象
     * @param {*} node         节点对应的Node对象
     * @param {*} vueComponent 节点组件本身
     * @return {*}
     */
    handleNodeCollapse(data, node, vueComponent) {
      this.$emit('handleNodeCollapse', data)
    },
    /**
     * @description: [事件] - 节点被点击时的回调
     * @param {*} data         该节点所对应的数据对象
     * @param {*} node         节点对应的Node对象
     * @param {*} vueComponent 节点组件本身
     * @return {*}
     */
    handleNodeClick(data, node, vueComponent) {
      this.$emit('handleNodeClick', data)
    },
    /**
     * @description: 事件 - 当某一节点被鼠标右键点击时会触发该事件
     * @param {*} event
     * @param {*} data         传递给 data 属性的数组中该节点所对应的对象
     * @param {*} node         节点对应的Node对象
     * @param {*} vueComponent 节点组件本身
     * @return {*}
     */
    handleNodeContextMenu(event, data, node, vueComponent) {
      this.$emit('handleNodeContextMenu', event, data)
    },
    /**
     * @description: [事件] - 当复选框被点击的时候触发
     * @param {*} checkedNodes      该节点所对应的对象
     * @param {*} checkedKeys       树目前的选中状态对象
     * @param {*} halfCheckedNodes
     * @param {*} halfCheckedKeys
     * @return {*}
     */
    handleCheck(node, checkedData) {
      // console.log('handleCheck: ', node, checkedData)
      this.$emit('handleCheck', node, checkedData)
    },
    /**
     * @description: 事件 - 节点选中状态发生变化时的回调
     * @param {*}
     * @return {*}
     */
    handleCheckChange(data, checked) {
      let currentNode = this.$refs.baseTree.getNode(data)
      // console.log('handleCheckChange: ', data, checked)
      if (this.checkStrictly) {
        // 用于：父子节点严格互不关联时，父节点勾选变化时通知子节点同步变化，实现单向关联
        if (checked) {
          // 选中 子节点只要被选中父节点就被选中
          this.parentNodeChange(currentNode)
        } else {
          // 未选中 处理子节点全部未选中
          this.childNodeChange(currentNode)
        }
      }
      this.$emit('handleCheckChange', data, checked)
    },
    // ------------------------------------------------------------------------
    /**
     * @description: 获取当前被选中节点的 key，使用此方法必须设置 node-key 属性，若没有节点被选中则返回 null
     * @param {*}
     * @return {*}
     */
    getCurrentKey: function() {
      return this.$refs.baseTree.getCurrentKey()
    },
    /**
     * @description: 获取当前被选中节点的 data，若没有节点被选中则返回 null
     * @param {*}
     * @return {*}
     */
    getCurrentNode: function() {
      return this.$refs.baseTree.getCurrentNode()
    },
    /**
     * @description:  通过 key 设置某个节点的当前选中状态，使用此方法必须设置 node-key 属性
     * @param {*} key 待被选节点的 key，若为 null 则取消当前高亮的节点
     * @return {*}
     */
    setCurrentKey: function(key) {
      // $nextTick 是确保DOM渲染结束之后执行的
      this.$nextTick(() => {
        this.$refs.baseTree.setCurrentKey(key)
      })
    },
    setCurrentNode: function(node) {
      // $nextTick 是确保DOM渲染结束之后执行的
      this.$nextTick(() => {
        this.$refs.baseTree.setCurrentNode(node)
      })
    },
    /**
     * @description: 若节点可被选择（即 show-checkbox 为 true），则返回目前被选中的节点的 key 所组成的数组
     * @param {*}
     * @return {*}
     */
    getCheckedKeys: function(bool) {
      return this.$refs.baseTree.getCheckedKeys(bool)
    },
    /**
     * @description: 若节点可被选择（即 show-checkbox 为 true），则返回目前被选中的节点所组成的数组
     * @param {*}
     * @return {*}
     */
    getCheckedNodes: function() {
      return this.$refs.baseTree.getCheckedNodes()
    },
    /**
     * @description: 通过 keys 设置目前勾选的节点，使用此方法必须设置 node-key 属性
     * @param {*} keys     勾选节点的 key 的数组
     * @param {*} leafOnly boolean 类型的参数，若为 true 则仅设置叶子节点的选中状态，默认值为 false
     * @return {*}
     */
    setCheckedKeys: function(keys) {
      // if(!keys)keys = [];
      const leafOnly = false
      this.$nextTick(() => {
        this.$refs.baseTree.setCheckedKeys(keys, leafOnly)
      })
    },
    /**
     * @description: 通过 key / data 设置某个节点的勾选状态，使用此方法必须设置 node-key 属性
     * @param {*} val       勾选节点的 key 或者 data
     * @param {*} checked   boolean 类型，节点是否选中
     * @param {*} deep      boolean 类型，是否设置子节点 ，默认为 false
     * @return {*}
     */
    setChecked: function(val, checked, deep) {
      this.$refs.baseTree.setChecked(val, checked)
    },
    /**
     * @description: 获取选中的节点，包括半选状态下父级节点的id，并赋值一个新的数组作为参数，用于授权处理
     * @param {*}
     * @return {*}
     */
    getCheckedAndHalfKeys: function() {
      return this.$refs.baseTree
        .getCheckedKeys()
        .concat(this.$refs.baseTree.getHalfCheckedKeys())
    },
    // --------------------------------------------------------------------------
    // 统一处理子节点为不选中
    childNodeChange(node) {
      for (let i = 0; i < node.childNodes.length; i++) {
        node.childNodes[i].checked = false
        this.childNodeChange(node.childNodes[i])
      }
    },
    // 统一处理父节点为选中
    parentNodeChange(node) {
      if (node.parent.key !== undefined) {
        node.parent.checked = true
        this.parentNodeChange(node.parent)
      }
    }
  },
  computed: {
    /**
     * @description: 树形结构数据(非标准的转换为标准结构)
     * @param {*}
     * @return {*}
     */
    treeList() {
      return this.$attrs.treeList || []
    }
  },
  watch: {
    filterText(val) {
      this.$refs.baseTree.filter(val)
    }
  }
}
</script>

<style lang="scss" scoped>
.header {
  width: 100%;
  margin-bottom: 10px;
}
// 字体和大小
.custom-tree-node {
  font-family: 'Microsoft YaHei';
  font-size: 14px;
  position: relative;
}

// 选中状态背景色
.baseTree
  .el-tree--highlight-current
  .el-tree-node.is-current
  > .el-tree-node__content {
  background-color: #f5f7fa !important;
}

// 原生el-tree-node的div是块级元素，需要改为inline-block，才能显示滚动条
.baseTree .el-tree > .el-tree-node {
  display: inline-block;
  min-width: 100%;
}
.hamburger {
  display: inline-block;
  vertical-align: middle;
  width: 20px;
  height: 20px;
}

.hamburger.is-active {
  transform: rotate(180deg);
}
</style>
