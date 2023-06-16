<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form :inline="true" :model="listQuery" class="demo-form-inline">
        <el-form-item label="设备名称：">
          <el-input
            v-model="listQuery.Q1"
            size="small"
            placeholder="请输入设备名称"
            :clearable="true"
            @clear="handleClear"
          ></el-input>
        </el-form-item>
        <el-form-item label="维修项名称：">
          <el-input
            v-model="listQuery.Q2"
            size="small"
            placeholder="请输入维修项名称"
            :clearable="true"
            @clear="handleClear"
          ></el-input>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            icon="el-icon-search"
            size="medium"
            round
            @click="onQuery"
            >查询</el-button
          >
          <el-button
            type="info"
            icon="el-icon-refresh"
            size="medium"
            round
            @click="goBack"
            >重置</el-button
          >
        </el-form-item>
      </el-form>
    </div>
    <div class="app-container flex-item">
      <el-row :gutter="4" class="fh">
        <!-- <el-col :span="4" class="fh ls-border">
          <el-card shadow="never" class="body-small fh" style="overflow: auto">
            <div slot="header" class="clearfix">
              <el-button
                type="text"
                style="padding: 0 11px"
                @click="getAllEquipment"
                >全部设备>></el-button
              >
            </div>
            <el-tree
              :data="locationsTree"
              :expand-on-click-node="false"
              :props="defaultProps"
              @node-click="handleNodeClick"
            ></el-tree>
          </el-card>
        </el-col> -->
        <el-col :span="24" class="fh">
          <div class="bg-white fh">
            <el-row class="flex-item fh1">
              <!--地点下挂载的设备表区域  data指定数据源Equipmentlist为数组-->
              <el-table
                ref="singleTable"
                :data="list"
                :key="tableKey"
                style="width: 100%"
                height="calc(100% - 55px)"
                :header-cell-style="headClass"
                @row-click="rowClick"
                @selection-change="handleSingleSelectionChange"
                highlight-current-row
                border
                stripe
                fit
              >
                <el-table-column
                  align="center"
                  :label="'设备ID'"
                  prop="EQU_ID"
                  v-if="showDescription"
                  min-width="160px"
                >
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'设备编号'"
                  prop="EQU_ID"
                  min-width="120px"
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'设备名称'"
                  prop="EQU_NAME"
                  min-width="120px"
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'添加员工'"
                  prop="CREATED_BY"
                  min-width="120px"
                ></el-table-column>
              </el-table>
              <!-- 分页区域 -->
              <pagination
                v-show="total > 0"
                :total="total"
                :page.sync="listQuery.pageIndex"
                :rows.sync="listQuery.pageSize"
                @pagination="handleCurrentChange"
              />
            </el-row>
            <div
              class="app-container flex-item btns"
              slot="header"
              style="position: relative"
            >
              <el-button
                size="mini"
                type="success"
                style="margin-left: 20px"
                round
                icon="el-icon-plus"
                @click="btnAdd"
                >添加维修项</el-button
              >
              <el-button
                size="mini"
                type="danger"
                style="margin-left: 20px"
                round
                icon="el-icon-delete"
                @click="btnDel"
                >删除维修项</el-button
              >
              <el-button
                size="mini"
                type="success"
                style="position: absolute; right: 15%"
                icon="el-icon-check"
                @click="qualify"
                >完成</el-button
              >
              <el-button
                size="mini"
                type="warning"
                style="position: absolute; right: 5%"
                icon="el-icon-close"
                @click="noQualify"
                >未完成</el-button
              >
            </div>
            <!-- 设备挂载点检项目区域 -->
            <div class="flex-item fh2">
              <el-table
                ref="eqtExamItemTable"
                :data="eqtExamItemList"
                :key="eqtExamItemTableKey"
                highlight-current-row
                border
                stripe
                fit
                style="width: 100%"
                height="285"
                :header-cell-style="headClass"
                @row-click="rowClickEqtExamItemTable"
                @selection-change="eqtItemSelectionChange"
              >
                <el-table-column
                  align="center"
                  type="selection"
                  width="55"
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'维修项编码'"
                  prop="MAINTAIN_ITEM_ID"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MAINTAIN_ITEM_ID }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'维修项名称'"
                  prop="MAINTAIN_ITEM_NAME"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MAINTAIN_ITEM_NAME }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'维修员工'"
                  prop="MAINTAIN_PERSON"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MAINTAIN_PERSON }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'维修结果'"
                  prop="MAINTAIN_RESULT"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MAINTAIN_RESULT }}</span>
                  </template>
                </el-table-column>
                <!-- <el-table-column
                  align="center"
                  :label="'创建时间'"
                  min-width="100px"
                  show-overflow-tooltip
                  prop="create_time"
                ></el-table-column> -->
                <!-- <el-table-column
                  align="center"
                  :label="'操作'"
                  width="100px"
                  class-name="small-padding fixed-width"
                >
                  <template slot-scope="scope">
                    <el-button
                      size="mini"
                      icon="el-icon-edit"
                      @click="handleUpdate(scope.row)"
                    ></el-button>
                    <el-button
                      size="mini"
                      icon="el-icon-delete"
                      @click="handleDelete(scope.row)"
                    ></el-button>
                  </template>
                </el-table-column> -->
              </el-table>
              <pagination
                slot="footer"
                v-show="eqtExamItemTotal > 0"
                :total="eqtExamItemTotal"
                :page.sync="listQuery2.pageIndex"
                :rows.sync="listQuery2.pageSize"
                @pagination="handleCurrentChange2"
              />
            </div>
          </div>
        </el-col>
      </el-row>
    </div>
    <!-- 添加，复制，设备维修项的弹出框 -->
    <el-dialog
      width="1000px"
      v-el-drag-dialog
      :modal="false"
      :close-on-click-modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogTableVisible"
    >
      <div style="height: 500px; overflow: auto">
        <div class="flex-column">
          <div class="app-container flex-item" style="padding-top: 10px">
            <div class="app-container flex-item" style="margin-top: 10px">
              <el-row :gutter="4" class="fh">
                <el-col :span="5" class="fh ls-border">
                  <el-card
                    shadow="never"
                    class="body-small fh"
                    style="overflow: auto"
                  >
                    <div slot="header" class="clearfix">
                      <el-button
                        type="text"
                        style="padding: 0 11px"
                        @click="getAllMaintainItem"
                        >全部维修项>></el-button
                      >
                    </div>
                    <el-tree
                      :data="maintainItemsTree"
                      :expand-on-click-node="false"
                      :props="defaultProps"
                      @node-click="handleNodeClickMaintainItem"
                      style="
                        width: 100%;
                        overflow-y: scroll;
                        overflow-x: hidden;
                        height: 370px;
                      "
                    ></el-tree>
                  </el-card>
                </el-col>
                <el-col :span="19" class="fh">
                  <div class="bg-white fh">
                    <!--点检表区域  data指定数据源examitemlist为数组-->
                    <el-table
                      ref="examItemTable"
                      :data="maintainItemList"
                      :key="examItemTableKey"
                      highlight-current-row
                      border
                      stripe
                      fit
                      style="width: 100%; overflow-x: hidden"
                      :header-cell-style="headClass"
                      height="430px"
                      @row-click="rowClickMaintainItemTable"
                      @selection-change="ItemhandleSelectionChange"
                    >
                      <el-table-column
                        align="center"
                        type="index"
                        width="50"
                      ></el-table-column>
                      <el-table-column
                        align="center"
                        :label="'维修项编码'"
                        prop="MAINTAIN_ITEM_ID"
                        min-width="80px"
                        show-overflow-tooltip
                      ></el-table-column>
                      <el-table-column
                        align="center"
                        :label="'维修项名称'"
                        prop="MAINTAIN_ITEM_NAME"
                        min-width="100px"
                        show-overflow-tooltip
                      ></el-table-column>
                      <el-table-column
                        align="center"
                        :label="'创建人'"
                        prop="CREATED_BY"
                        width="90px"
                        show-overflow-tooltip
                      ></el-table-column>
                    </el-table>
                    <pagination
                      v-show="total3 > 0"
                      :total="total3"
                      :page.sync="listQuery3.pageIndex"
                      :rows.sync="listQuery3.pageSize"
                      @pagination="handleCurrentChange3"
                    />
                  </div>
                </el-col>
              </el-row>
            </div>
          </div>
        </div>
      </div>

      <div slot="footer">
        <el-button size="mini" @click="dialogTableVisible = false"
          >关闭</el-button
        >
        <el-button
          size="mini"
          v-if="dialogStatus == 'create'"
          type="primary"
          @click="saveData"
          >保存</el-button
        >
      </div>
    </el-dialog>
    <!-- 新增项的弹出框 -->
    <el-dialog
      width="800px"
      v-el-drag-dialog
      :modal="false"
      :close-on-click-modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogFormVisible"
    >
      <el-form
        :rules="rules"
        ref="dataForm"
        :model="temp"
        label-position="right"
        label-width="100px"
      >
        <div>
          <el-row>
            <el-button
              size="mini"
              type="success"
              style="margin: 0 0 10px 0"
              icon="el-icon-circle-plus-outline"
              @click="Binding"
            >
              点击绑定设备和维修项
            </el-button>
          </el-row>
          <el-row>
            <el-col :span="12">
              <el-form-item :label="'设备编号：'" size="small" prop="EQU_ID">
                <el-input v-model="temp.EQU_ID" disabled></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item :label="'设备名称：'" size="small" prop="EQU_NAME">
                <el-input v-model="temp.EQU_NAME" disabled></el-input>
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="12">
              <el-form-item
                :label="'维修项编号：'"
                size="small"
                prop="MAINTAIN_ITEM_ID"
              >
                <el-input v-model="temp.MAINTAIN_ITEM_ID" disabled></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item
                :label="'维修项名称：'"
                size="small"
                prop="MAINTAIN_ITEM_NAME"
              >
                <el-input v-model="temp.MAINTAIN_ITEM_NAME" disabled></el-input>
              </el-form-item>
            </el-col>
          </el-row>
        </div>
        <el-tabs type="border-card">
          <el-tab-pane label="其它信息">
            <el-row>
              <el-col :span="12">
                <el-form-item
                  :label="'维修人员：'"
                  size="small"
                  prop="MAINTAIN_PERSON"
                >
                  <el-input v-model="temp.MAINTAIN_PERSON"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item :label="'是否停机：'" size="small" prop="IS_STOP">
                  <el-input v-model="temp.IS_STOP"></el-input>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="12">
                <el-form-item
                  :label="'维修结果：'"
                  size="small"
                  prop="MAINTAIN_RESULT"
                >
                  <el-input v-model="temp.MAINTAIN_RESULT"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item
                  :label="'创建人员：'"
                  size="small"
                  prop="CREATED_BY"
                >
                  <el-input v-model="temp.CREATED_BY"></el-input>
                </el-form-item>
              </el-col>
            </el-row>
          </el-tab-pane>
        </el-tabs>
      </el-form>
      <span slot="footer">
        <el-button
          size="mini"
          v-if="dialogStatus == 'create'"
          type="primary"
          @click="createData"
          >保存</el-button
        >
        <el-button size="mini" @click="closeForm">关闭</el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
import { listToTreeSelect } from '@/utils'
import * as EquMaintainItem from '@/api/EquMaintainItem'
import * as MaintainItem from '@/api/MaintainItem'
import * as EquInfo from '@/api/EquInfo'
import Treeselect from '@riophae/vue-treeselect'
import '@riophae/vue-treeselect/dist/vue-treeselect.css'
import elDragDialog from '@/directive/el-dragDialog'
import Pagination from '@/components/Pagination'
import { TwoListToTree } from '@/utils'
import { forEach } from 'shelljs/commands'

export default {
  name: 'EquMaintainItems',
  components: {
    Pagination,
    Treeselect
  },
  directives: {
    elDragDialog
  },
  data() {
    return {
      defaultProps: {
        // tree配置项
        children: 'children',
        label: 'label'
      },
      singleleSelection: [], // 设备单选值
      multipleSelection: [], // 列表checkbox多选中的值
      eqtItemMultiSelection: [],
      tableKey: 0, // 设备table
      eqtExamItemTableKey: 0, //设备下点检项目table
      examItemTableKey: 0, // 点检项目table
      list: null, // 设备表格绑定的数据
      maintainItemList: null, // 点检项目表格绑定的数据
      eqtExamItemList: null, // 设备对应的点检项目绑定的数据
      total: 0, // 总数据条数
      total3:0,
      eqtExamItemTotal: 0,
      examItemTotal: 0, // 点检总条数
      listLoading: true,
      dialogFormVisible: false, //新增弹窗显示标志
      listQuery: {
        Q1: '',
        Q2: '',
        QId: '',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      listQuery2: {
        Q1: '',
        Q2: '',
        QId: '',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      listQuery3: {
        Q1: '',
        Q2: '',
        QId: '',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      codeKey: '',
      nameKey: '',
      nameMaintainItemKey: '', //查询条件
      equipmentId: '', //设备Id
      type_id: '', // 获取点检树下的列表
      location_id: null, // 传递给后端获取地点树下设备
      locations: [], // 用户可访问到的地点列表
      locationsTree: [], // 用户可访问到的地点所有组成的树
      examItems: [], // 用户可访问到的点检列表
      maintainItemsTree: [], // 用户可访问到的所有点检类型组成的树
      // OnlyRootEquip: false,
      showDescription: false,
      showExamItemDescription: false,
      // 添加表单数据传给后台
      temp: {
        EQU_ID: '',
        EQU_NAME: '',
        MAINTAIN_ITEM_ID: '',
        MAINTAIN_ITEM_NAME: '',
        MAINTAIN_PERSON: '',
        MAINTAIN_RESULT: '',
        IS_STOP: '',
        CREATED_BY: ''
      },
      tempUpdate: {
        exam_equipment_item_id: undefined,
        equipment_id: undefined,
        equipment_code: '',
        equipment_name: '',
        default_value: '', // 默认值
        max_value: '', // 最大值
        min_value: '', // 最小值
        method_type: '1'
      },
      tableTotalData: [],
      searchStatus: false, //搜索按钮标志
      dialogFormVisible: false, // 控制Table对话框的显示与隐藏
      dialogTableVisible: false,
      chkRoot: false, // 根节点是否选中
      treeDisabled: true, // 树选择框时候可用
      dialogStatus: '',
      textMap: {
        create: '添加维修项-新增',
        update: '编辑维修项-更新'
      },
      // 添加表单验证规则对象
      rules: {
        EQU_ID: [
          {
            required: true,
            message: '设备ID不能为空',
            trigger: 'blur'
          }
        ],
        EQU_NAME: [
          {
            required: true,
            message: '设备名称不能为空',
            trigger: 'blur'
          }
        ],
        MAINTAIN_ITEM_ID: [
          {
            required: true,
            message: '维修项ID不能为空',
            trigger: 'blur'
          }
        ],
        MAINTAIN_ITEM_NAME: [
          {
            required: true,
            message: '维修项名称不能为空',
            trigger: 'blur'
          }
        ],
        MAINTAIN_PERSON: [
          {
            required: true,
            message: '维修人员不能为空',
            trigger: 'blur'
          }
        ],
        IS_STOP: [
          {
            required: true,
            message: '是否停机项不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: true,
            message: '信息添加人员不能为空',
            trigger: 'blur'
          }
        ]
      },
      downloadLoading: false,
      nodeClickStatus:false,
      receivedIds: null,
      receovedIds: [],
      checkItemRowData: null, //点检项行数据
      ids: {
        id1: '',
        id2: ''
      },
      updateIds: {
        id1: '',
        id2: ''
      }
    }
  },
  // 生命周期函数
  created() {
    this.getEquMaintainItemsFilterPageList()
  },
  mounted() {
    this.getMaintainItemList()
    this.getTree()
  },
  computed: {},
  methods: {
    // 过滤分页
    getEquMaintainItemsFilterPageList(){
      this.listLoading=true
      this.listQuery.flag=1
      EquMaintainItem.QueryEquMaintainItem(this.listQuery).then(response=>{
        //console.log('------------------',response)
        this.list=response.Data
        this.total=response.Total
        this.listLoading=false
      }).catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 根据设备编号获取维修项目
    getMaintainItemByEquId(data) {
      this.listLoading = true
      EquMaintainItem.GetByEquId(data)
        .then((response) => {
          if (response.Code === 200) {
            this.listLoading = false
            this.eqtExamItemList = response.Data
            this.eqtExamItemTotal = response.Total
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 设备单行处理,当某一行被点击时会触发该事件
    rowClick(row) {
      this.$refs.singleTable.setCurrentRow(row)
      this.listQuery2.QId = row.EQU_ID
      this.getMaintainItemByEquId(this.listQuery2)
    },
    // 设备下维修项目行处理
    rowClickEqtExamItemTable(row) {
      this.checkItemRowData = row
      this.$refs.eqtExamItemTable.clearSelection()
      this.$refs.eqtExamItemTable.toggleRowSelection(row)
    },
    // 点检项目行处理（弹窗里的table）
    rowClickMaintainItemTable(row) {
      this.$refs.examItemTable.clearSelection()
      this.$refs.examItemTable.toggleRowSelection(row)
      this.temp.MAINTAIN_ITEM_ID = row.MAINTAIN_ITEM_ID
      this.temp.MAINTAIN_ITEM_NAME = row.MAINTAIN_ITEM_NAME
    },
    // 点击地点树的事件
    handleNodeClick(data) {},
    // 节点点击事件函数
    getMaintainItemByTypeId(data) {
      MaintainItem.GetItemsWithTypeId(data)
        .then((response) => {
          if (response.Code === 200) {
            this.nodeClickStatus=true
            this.maintainItemList = response.Data
            this.total3=response.Total
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 点击树的事件,节点被点击时的回调
    handleNodeClickMaintainItem(data) {
      if (!data.children) {
        this.temp.EQU_ID = data.id
        this.temp.EQU_NAME = data.label
      } else {
        var nodeId = data.id
        this.listQuery3.QId=data.id
        this.getMaintainItemByTypeId(this.listQuery3)
      }
    },
    //搜索框清除按钮
    handleClear() {
      if (this.listQuery.Q1 === '' && this.listQuery.Q2 === '') {
        this.getEquMaintainItemsFilterPageList()
      }
    },
    saveData() {
      this.dialogTableVisible = false
    },
    // 绑定弹窗的主数据
    getMaintainItemList() {
      MaintainItem.QueryItems(this.listQuery3).then((response) => {
        if(response.Code===200){
          this.maintainItemList=response.Data
          this.total3=response.Total
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 获取全部设备
    getAllEquipment() {
      //this.getEquMaintainItemsFilterPageList()
    },
    // 设备和维修项绑定事件
    Binding() {
      this.dialogStatus = 'create'
      this.dialogTableVisible = true
    },
    // 获取全部点检项目
    getAllMaintainItem() {
      //this.getMaintainItemList()
    },
    // 处理设备单选事件
    handleSingleSelectionChange(val) {
      this.singleleSelection = val
    },
    eqtItemSelectionChange(val) {
      this.eqtItemMultiSelection = val
    },
    // 处理多选事件,先共用一个,当选择项发生变化时会触发该事件
    ItemhandleSelectionChange(val) {
      this.multipleSelection = val
    },
    // 完成按钮
    qualify() {
      if (this.eqtItemMultiSelection.length === 0) {
        this.$notify({
          title: ' 提示信息',
          message: '至少选择一条数据',
          position: 'bottom-right',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.eqtItemMultiSelection.forEach((element) => {
        ;(this.updateIds.id1 = element.EQU_ID),
          (this.updateIds.id2 = element.MAINTAIN_ITEM_ID)

        element.MAINTAIN_RESULT = '完成'

        this.updateResult(this.updateIds, element)
      })
    },
    // 更新函数
    updateResult(params, data) {
      EquMaintainItem.UpdateWithTwoId(params, data).then((response) => {
        if (response.Code === 200) {
          this.$notify({
            title: ' 提示信息',
            message: '修改成功',
            position: 'bottom-right',
            type: 'success',
            duration: 2000
          })
        }
      })
    },
    // 未完成按钮
    noQualify() {
      if (this.eqtItemMultiSelection.length === 0) {
        this.$notify({
          title: ' 提示信息',
          message: '至少选择一条数据',
          position: 'bottom-right',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.eqtItemMultiSelection.forEach((element) => {
        ;(this.updateIds.id1 = element.EQU_ID),
          (this.updateIds.id2 = element.MAINTAIN_ITEM_ID)

        element.MAINTAIN_RESULT = '未完成'

        this.updateResult(this.updateIds, element)
      })
    },
    // 新增弹窗关闭按钮
    closeForm() {
      this.dialogFormVisible = false
    },
    // 获取设备树
    async getTree() {
      this.listLoading = true
      var tempEquType
      var tempEquInfo
      await EquInfo.GetAllEquType().then((response) => {
        tempEquType = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_TYPE_ID,
            label: item.EQU_TYPE_NAME,
            parentId: null
          }
        })
      })
      await EquInfo.GetAllEquInfo().then((response) => {
        tempEquInfo = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_ID,
            label: item.EQU_NAME,
            parentId: item.TYPE_ID
          }
        })
      })

      var treeData = TwoListToTree(tempEquType, tempEquInfo)
      this.maintainItemsTree = treeData
      this.listLoading = false
      //console.log('------树型数据',treeData)
    },
    // 获取车间地点树
    getWorkPlaceTree() {
      EquCheckItem.GetWorkPlaceTree().then((response) => {
        if (response.Code === 200) {
          var tempTree = response.Data.map(function (item, index, input) {
            return {
              id: item.PlaceId,
              label: item.PlaceName,
              parentId: item.ParentPlaceId || null
            }
          })
          var treeTemp = JSON.parse(JSON.stringify(tempTree))
          this.locationsTree = listToTreeSelect(treeTemp)
        }
      })
    },
    // 搜索框进行处理筛选
    onQuery() {
      if (this.listQuery.Q1 === '' && this.listQuery.Q2 === '') {
        this.$message.warning('请输入查询条件')
        return
      } else {
        this.listQuery.flag=1
        this.queryData(this.listQuery)
      }
    },
    // 重置按钮
    goBack() {
      this.getEquMaintainItemsFilterPageList()
    },
    // 模糊查询函数
    queryData(data) {
      this.listLoading = true
      EquMaintainItem.QueryEquMaintainItem(data)
        .then((response) => {
          if (response.Code === 200) {
            this.list = response.Data
            this.total = response.Total
            this.listLoading = false
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 监听页码值改变的事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if (this.searchStatus) {
        this.queryData(this.listQuery)
        return
      }
      this.getEquMaintainItemsFilterPageList()
    },
    //挂载的维修项显示页码改变事件
    handleCurrentChange2(val) {
      this.listQuery2.pageIndex = val.page
      this.listQuery2.pageSize = val.rows
      this.getMaintainItemByEquId(this.listQuery2)
    },
    // 弹窗分页器页码改变事件
    handleCurrentChange3(val) {
      this.listQuery3.pageIndex = val.page
      this.listQuery3.pageSize = val.rows
      if(this.nodeClickStatus){
        this.getMaintainItemByTypeId(this.listQuery3)
        return
      }
      this.getMaintainItemList()
    },
    // 添加事件
    btnAdd() {
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
        this.resetTemp()
      })
    },
    // 删除
    btnDel() {
      if (this.eqtItemMultiSelection.length !== 1) {
        this.$message({
          message: '请逐条删除数据',
          type: 'warning'
        })
        return
      }
      var array = this.eqtItemMultiSelection[0]
      ;(this.ids.id1 = array.EQU_ID), (this.ids.id2 = array.MAINTAIN_ITEM_ID)
      this.$confirm(`是否确定删除选中的多条数据?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          this.deleteEquMaintainItem(this.ids)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '删除失败'
          })
        })
    },
    // 删除点检项函数
    deleteEquMaintainItem(ids) {
      EquMaintainItem.DeleteByTwoId(ids).then((response) => {
        if (response.Code === 200) {
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        } 
        this.getEquMaintainItemsFilterPageList()
        this.getMaintainItemByEquId(this.listQuery2)
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 弹出添加框
    handleCreate() {
      this.dialogStatus = 'create'
      this.dialogTableVisible = true
    },
    // 维修项新增函数
    addMaintainItem(data) {
      EquMaintainItem.Add(data).then((response) => {
        //console.log(response)
        if (response.Code === 200) {
          this.dialogFormVisible = false
          this.$notify({
            title: '成功',
            message: '创建成功',
            type: 'success',
            duration: 2000
          })
        }
        this.getEquMaintainItemsFilterPageList()
      }).catch(()=>{
        this.$message({
          message:'该维修项已存在，请勿重复添加',
          type:'warning'
        })
      })
    },
    // 新增弹窗保存按钮
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          if (
            this.temp.EQU_ID === '' ||
            this.temp.EQU_NAME === '' ||
            this.temp.MAINTAIN_ITEM_ID === '' ||
            this.temp.MAINTAIN_ITEM_NAME === ''
          ) {
            this.$message({
              message: '设备项或维修项未绑定，请前往绑定再进行数据添加',
              type: 'warning'
            })
            return
          }
          this.addMaintainItem(this.temp)
        }
      })
    },
    // 弹出修改框
    handleUpdate(row) {
      this.tempUpdate = Object.assign({}, row)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 修改提交
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
        }
      })
    },
    // 设置表头颜色
    headClass() {
      return 'background:#337ab7'
    },
    // 重置新增数据
    resetTemp(){
      this.temp={
        EQU_ID: '',
        EQU_NAME: '',
        MAINTAIN_ITEM_ID: '',
        MAINTAIN_ITEM_NAME: '',
        MAINTAIN_PERSON: '',
        MAINTAIN_RESULT: '',
        IS_STOP: '',
        CREATED_BY: ''
      }
    }
  }
}
</script>

<style scoped>
.filter-container {
  height: 50px;
  padding: 0 10px;
  font-size: 14px;
  /* background-color: #fff; */
}
.filter-container .el-form {
  height: 40px;
}
.filter-container .el-form .el-form-item .el-button[data-v-501bf07c] {
  padding-bottom: 7px;
}
.filter-container .el-row {
  padding: 10px 0;
}
.el-button--medium.is-round {
  padding: 7px 20px !important;
}

.app-container {
  width: 100%;
  height: calc(100% - 50px);
  padding: 0;
}
.app-container .btns {
  height: 50px;
  padding: 10px 0;
}
.filter-container1 {
  margin-top: 10px;
  font-size: 14px;
}
.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: '';
}

.clearfix:after {
  clear: both;
}
/* 上下同，左右同 */
.el-card__header {
  padding: 12px 20px;
}
.body-small.dialog-mini .el-dialog__body .el-form {
  padding-right: 0px;
  padding-top: 0px;
}
.el-select.filter-item.el-select--small {
  width: 100%;
}

.fh1 {
  height: 50%;
}
.fh1 .table{
  height: 100%;
}
.fh2 {
  height: 40%;
}
</style>
