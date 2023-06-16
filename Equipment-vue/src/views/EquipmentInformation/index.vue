<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form
        :inline="true"
        :model="listQuery"
        class="demo-form-inline"
        style="position: relative"
      >
        <el-form-item label="设备名称：">
          <el-input
            v-model="listQuery.Q2"
            size="small"
            placeholder="请输入设备名称"
            :clearable="true"
            @clear="handleClear"
          ></el-input>
        </el-form-item>
        <el-button
          type="primary"
          icon="el-icon-search"
          size="medium"
          style="margin-top: 5px"
          round
          @click="onQuery"
          >查询</el-button
        >
        <el-button
          type="success"
          icon="el-icon-circle-plus-outline"
          size="medium"
          round
          @click="onAdd"
          >新增</el-button
        >
        <el-button
          type="danger"
          icon="el-icon-delete"
          size="medium"
          round
          @click="onDelete"
          >删除</el-button
        >
        <el-button
          type="info"
          style="position: absolute; right: 20px"
          icon="el-icon-s-grid"
          size="medium"
          @click="onJump"
          >设备信息维护</el-button
        >
      </el-form>
    </div>
    <div class="app-container flex-item flex-column">
      <el-row :gutter="4" class="fh">
        <el-col :span="4" class="fh ls-border">
          <el-card shadow="never" class="body-small fh" style="overflow: auto">
            <div slot="header" class="clearfix">
              <el-button type="text" style="padding: 0 11px" @click="getAll"
                >全部设备类型>></el-button
              >
            </div>
            <el-tree
              :data="equipmentTree"
              :expand-on-click-node="false"
              default-expand-all
              :props="defaultProps"
              @node-click="handleNodeClick"
            >
              <span class="custom-tree-node" slot-scope="{ node }">
                <span>{{ node.label }}</span>
              </span>
            </el-tree>
          </el-card>
        </el-col>
        <el-col :span="20" class="fh">
          <div class="flex-column">
            <!-- 主table区域 地点下的设备 -->
            <div class="demo-card table fh1">
              <el-table
                class="maintable"
                :header-cell-style="headClass"
                ref="mainTable"
                :key="tableKey"
                :data="list"
                style="width: 100%"
                height="calc(100% - 55px)"
                @row-click="rowClick"
                @selection-change="handleSelectionChange"
                v-loading="listLoading"
                border
                fit
                stripe
                highlight-current-row
              >
                <el-table-column align="center" type="selection" width="45">
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="140px"
                  :label="'设备编码'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.EQU_ID }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="100px"
                  :label="'设备名称'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.EQU_NAME }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="80px"
                  :label="'设备规格'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.EQU_SPEC }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="80px"
                  :label="'设备状态'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.EQU_STATUS }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="80px"
                  :label="'所属车间'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.WORK_SHOP }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="80px"
                  :label="'安装地点'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.INSTALL_SITE }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="80px"
                  :label="'负责人'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.HEAD }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="120px"
                  :label="'联系方式'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.PHONE_NO }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="100px"
                  :label="'生产厂商'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MANUFACTURER }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="120px"
                  :label="'供应商'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.VENDOR }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="120px"
                  :label="'购入时间'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.PUR_TIME }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="120px"
                  :label="'启用时间'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.ENABLE_TIME }}</span>
                  </template>
                </el-table-column>
                <el-table-column align="center" width="200px" :label="'操作'">
                  <template slot-scope="scope">
                    <el-button
                      size="mini"
                      round
                      type="primary"
                      icon="el-icon-edit"
                      @click="handleUpdate(scope.row)"
                      >更新</el-button
                    >
                    <el-button
                      size="mini"
                      round
                      type="danger"
                      icon="el-icon-delete"
                      @click="handleDelete(scope.row)"
                      >删除</el-button
                    >
                  </template>
                </el-table-column>
              </el-table>
              <!-- 分页 -->
              <pagination
                v-show="total > 0"
                :total="total"
                :page.sync="listQuery.pageIndex"
                :rows.sync="listQuery.pageSize"
                @pagination="handleCurrentChange"
              />
            </div>
          </div>
        </el-col>
      </el-row>
    </div>
    <!-- 设备新增弹出窗 -->
    <el-dialog
      title="设备类型-新增"
      :modal="false"
      v-el-drag-dialog
      :visible.sync="dialogVisible"
      width="500px"
    >
      <el-form
        :rules="rules1"
        ref="dataForm1"
        :model="temp1"
        label-position="right"
        label-width="100px"
      >
        <div>
          <el-form-item size="small" label="类型名称：" prop="EQU_TYPE_NAME">
            <el-input v-model="temp1.EQU_TYPE_NAME"></el-input>
          </el-form-item>
        </div>
        <div>
          <el-form-item size="small" label="类型编码：" prop="EQU_TYPE_ID">
            <el-input v-model="temp1.EQU_TYPE_ID"></el-input>
          </el-form-item>
        </div>
        <div>
          <el-form-item size="small" label="添加人：" prop="CREATED_BY">
            <el-input v-model="temp1.CREATED_BY"></el-input>
          </el-form-item>
        </div>
      </el-form>
      <span slot="footer">
        <el-button size="mini" type="primary" @click="submit">保存</el-button>
        <el-button size="mini" @click="dialogVisible = false">关闭</el-button>
      </span>
    </el-dialog>
    <!-- 弹出主table对话框 -->
    <el-dialog
      width="800px"
      class="dialog-mini"
      :modal="false"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogFormVisible"
      v-el-drag-dialog
      :close-on-click-modal="false"
    >
      <!-- 主table表单 -->
      <el-form
        :rules="rules"
        ref="dataForm"
        :model="temp"
        label-position="right"
        label-width="100px"
      >
        <div>
          <el-form-item size="small" label="设备名称：" prop="EQU_NAME">
            <el-input v-model="temp.EQU_NAME"></el-input>
          </el-form-item>
        </div>
        <el-tabs type="border-card">
          <el-tab-pane label="基础信息">
            <el-row>
              <el-col :span="8">
                <el-form-item
                  :label="'所属车间：'"
                  size="small"
                  prop="WORK_SHOP"
                >
                  <!-- <el-input v-model="temp.WORK_SHOP"></el-input> -->
                  <el-cascader
                    v-model="temp.WORK_SHOP"
                    :options="options"
                    clearable
                    @change="handleChange"
                    ref="myCascader"
                  >
                  </el-cascader>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  :label="'安装地点：'"
                  size="small"
                  prop="INSTALL_SITE"
                >
                  <!-- <el-input v-model="temp.INSTALL_SITE"></el-input> -->
                  <el-select
                    v-model="temp.INSTALL_SITE"
                    placeholder="请选择"
                    clearable
                  >
                    <el-option
                      v-for="item in installSiteOptions"
                      :key="item.SiteName"
                      :label="item.SiteName"
                      :value="item.SiteName"
                    >
                    </el-option>
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item :label="'设备类型：'" size="small" prop="TYPE_ID">
                  <el-input v-model="temp.TYPE_ID" disabled></el-input>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item
                  size="small"
                  :label="'设备规格：'"
                  prop="EQU_SPEC"
                >
                  <el-input v-model="temp.EQU_SPEC"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  size="small"
                  :label="'设备状态：'"
                  prop="EQU_STATUS"
                >
                  <el-input v-model="temp.EQU_STATUS"></el-input>
                </el-form-item>
              </el-col>

              <el-col :span="8">
                <el-form-item size="small" :label="'负责人：'" prop="HEAD">
                  <el-input v-model="temp.HEAD"></el-input>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item
                  :label="'联系方式：'"
                  size="small"
                  prop="PHONE_NO"
                >
                  <el-input v-model="temp.PHONE_NO"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  :label="'生产厂商：'"
                  size="small"
                  prop="MANUFACTURER"
                >
                  <el-input v-model="temp.MANUFACTURER"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item :label="'供应商：'" size="small" prop="VENDOR">
                  <el-input v-model="temp.VENDOR"></el-input>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item
                  :label="'购入日期：'"
                  size="small"
                  prop="PUR_TIME"
                >
                  <el-date-picker
                    style="width: 142px"
                    v-model="temp.PUR_TIME"
                    type="date"
                    value-format="yyyy-MM-dd"
                    placeholder="购入日期"
                  >
                  </el-date-picker>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  :label="'启用日期：'"
                  size="small"
                  prop="ENABLE_TIME"
                >
                  <el-date-picker
                    style="width: 142px"
                    v-model="temp.ENABLE_TIME"
                    type="date"
                    value-format="yyyy-MM-dd"
                    placeholder="启用日期"
                  >
                  </el-date-picker>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  :label="'创建人：'"
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
        <el-button
          size="mini"
          v-else-if="dialogStatus == 'update'"
          type="primary"
          @click="updateData"
          >保存</el-button
        >
        <el-button size="mini" @click="closeForm">关闭</el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
// 分页组件
import Pagination from '@/components/Pagination'
// 树型数据组件
import Treeselect from '@riophae/vue-treeselect'
import * as EquInfo from '@/api/EquInfo'
import elDragDialog from '@/directive/el-dragDialog'
import { listToTreeSelect, arrayToTree } from '@/utils'
import { forEach } from 'shelljs/commands'
export default {
  name: 'EquipmentInformation',
  // 两个子组件
  components: {
    Pagination,
    Treeselect
  },
  directives: {
    elDragDialog
  },
  data() {
    return {
      // tree配置项
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      options: [], // 车间地点级联选择器
      textMap: {
        create: '设备-新增',
        update: '设备-修改'
      },
      // 弹窗展示flag
      dialogFormVisible: false,
      // 弹窗属性参数
      dialogStatus: '',
      listLoading: true, //列表加载
      // 主展示表格数据
      list: null,
      // 分页参数
      listQuery: {
        Q1: '',
        Q2: '',
        QId: '',
        pageIndex: 1,
        pageSize: 10,
        flag: 0
      },
      // 弹窗参数汇总
      temp: {
        EQU_NAME: '',
        EQU_SPEC: '',
        EQU_STATUS: '',
        WORK_SHOP: '',
        INSTALL_SITE: '',
        HEAD: '',
        PHONE_NO: '',
        MANUFACTURER: '',
        VENDOR: '',
        PUR_TIME: '',
        ENABLE_TIME: '',
        CREATED_BY: '',
        EQU_ID: ''
      },
      // 设备类型新增
      temp1: {
        EQU_TYPE_NAME: '',
        EQU_TYPE_ID: '',
        CREATED_BY: ''
      },
      // 设备类型新增弹窗显示标志
      dialogVisible: false,
      // 表单规则验证
      rules: {
        EQU_NAME: [
          {
            required: true,
            message: '设备名称不能为空',
            trigger: 'blur'
          }
        ],
        WORK_SHOP: [
          {
            required: true,
            message: '所属车间不能为空',
            trigger: 'blur'
          }
        ],
        PUR_TIME: [
          {
            required: true,
            message: '购入日期不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: true,
            message: '创建人不能为空',
            trigger: 'blur'
          }
        ],
        PHONE_NO: [
          { required: true, message: '请输入手机号', trigger: 'blur' },
          {
            pattern: /^1\d{10}$/,
            message: '请输入正确的11位手机号',
            trigger: 'blur'
          },
          { max: 11, message: '手机号不能超过11位', trigger: 'blur' },
          { min: 11, message: '手机号不能少于11位', trigger: 'blur' }
        ]
      },
      // 表单验证
      rules1: {
        EQU_TYPE_NAME: [
          {
            required: true,
            message: '类型名称不能为空',
            trigger: 'blur'
          }
        ],
        EQU_TYPE_ID: [
          {
            required: true,
            message: '类型编码不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: true,
            message: '创建人不能为空',
            trigger: 'blur'
          }
        ]
      },
      locationsTree: [], //车间树
      equipmentTree: [], //设备树
      searchStatus: false, //查询标志
      nodeClickStatus: false, //节点点击标志
      tableTotalData: [], //表格临时数据
      show: false, //设备类型删除标志显示标志
      tableKey: 0, //主设备
      total: 0, //分页数据总数
      locationOptions: '', //车间地点选择
      eqTypeOptions: '', //设备类型选择
      nodeId: null,
      multipleSelection: [], //主设备列表选择的值
      tempNodeData: [], //节点临时数据
      showTree: false,
      typeId: '',
      installSiteOptions: null //安装地点数据
    }
  },
  created() {
    this.getEquType()
    this.getEquInfoPageList()
  },
  mounted() {},
  methods: {
    // 设备信息维护页面跳转
    onJump() {
      this.$router.push({
        path: 'infoMaintain'
      })
    },
    // 获取设备类型数据
    getEquType() {
      EquInfo.GetAllEquType().then((response) => {
        //console.log("------设备类型数据",response);
        var tempData = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_TYPE_ID,
            label: item.EQU_TYPE_NAME,
            parentId: null
          }
        })
        var temp = JSON.parse(JSON.stringify(tempData))
        this.equipmentTree = listToTreeSelect(temp)
      })
    },
    // 获取设备信息分页数据
    getEquInfoPageList() {
      this.listLoading = true
      EquInfo.QueryEquInfo(this.listQuery)
        .then((response) => {
          if (response.Code === 200) {
            this.list = response.Data
            this.total = response.Total
            this.listLoading = false
          }
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 获得车间树
    getWorkPlaceTree() {
      EquInfo.GetWorkPlaceTree().then((response) => {
        var tempTree = response.Data.map(function (item) {
          return {
            id: item.PlaceId,
            value: item.PlaceId,
            label: item.PlaceName,
            parentId: item.ParentPlaceId
          }
        })
        var tempPlaceData = JSON.parse(JSON.stringify(tempTree))
        this.options = listToTreeSelect(tempPlaceData)
      })
    },
    // 主数据展示区的查询按钮
    onQuery() {
      if (this.listQuery.Q2 === '') {
        this.$message.warning('请输入查询条件')
        return
      }
      this.queryAndPage(this.listQuery)
    },
    // 模糊分页函数
    queryAndPage(data) {
      this.listLoading = true
      EquInfo.QueryEquInfo(data)
        .then((response) => {
          if (response.Code === 200) {
            this.searchStatus = true
            this.list = response.Data
            this.total = response.Total
            this.listLoading = false
          }
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 搜索输入框清除
    handleClear() {
      this.getEquInfoPageList()
    },
    // 类型树上方汉字点击事件
    getAll() {
      // this.listQuery={
      //   Q1:'',
      //   Q2:'',
      //   QId:'',
      //   pageIndex:1,
      //   pageSize:10
      // }
      //this.getEquInfoPageList(this.listQuery)
    },
    // 数据新增按钮
    onAdd() {
      if (this.tempNodeData.length === 0) {
        this.$message({
          message: '请选择一个设备类型，再进行新增操作',
          type: 'warning'
        })
        return
      }
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      // 在新增弹窗打开之后初始化车间数据
      this.getWorkPlaceTree()
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
        this.resetTemp()
        this.temp.TYPE_ID = this.typeId
      })
    },
    // 设备信息添加接口函数
    addEquInfo(addData) {
      EquInfo.AddEquInfo(addData)
        .then((response) => {
          // console.log('添加刀具类型数据', response)
          if (response.Code === 200) {
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '创建成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getEquInfoByTypeId(this.listQuery)
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 新增弹窗保存按钮
    createData() {
      //console.log('-------新增数据', this.temp)
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.addEquInfo(this.temp)
        }
      })
    },
    // 可批量删除按钮
    onDelete() {
      var ids = []
      var array = this.multipleSelection
      for (let i = 0; i < array.length; i++) {
        ids.push(array[i].EQU_ID)
      }
      this.$confirm(`是否确定删除选中的多条数据?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          //console.log('----ids',ids)
          this.deleteEquInfoByIds(ids)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 可批量删除函数
    deleteEquInfoByIds(ids) {
      EquInfo.DeleteByIds(ids)
        .then((response) => {
          if (response.Code === 200) {
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getEquInfoPageList()
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 节点点击事件
    handleNodeClick(data) {
      this.tempNodeData.push(data)
      this.listQuery.QId = data.id
      this.typeId = data.id
      //调用节点点击函数
      this.getEquInfoByTypeId(this.listQuery)
    },
    // 节点点击函数
    getEquInfoByTypeId(data) {
      this.listLoading = true
      EquInfo.GetByTypeId(data)
        .then((response) => {
          if (response.Code === 200) {
            this.nodeClickStatus = true
            this.list = response.Data
            this.total = response.Total
            this.listLoading = false
          }
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 分页页码变化事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if (this.searchStatus) {
        this.queryAndPage(this.listQuery)
        return
      }
      if (this.nodeClickStatus) {
        this.getEquInfoByTypeId(this.listQuery)
        return
      }
      this.getEquInfoPageList()
    },
    // 级联选择器选项改变事件
    handleChange(value) {
      // console.log('-----value', value)
      if (value !== undefined) {
        this.handleGetCascaderData()
        for (let i = 0; i < value.length; i++) {
          if (i === value.length - 1) {
            var placeId = value[i]
            // 实时获取该车间下的安装地点
            this.getInstallSiteDataByPlaceId(placeId)
          }
        }
      }else{
        return
      }
    },
    // 级联选择器节点函数
    handleGetCascaderData() {
      let labelArray = []
      let checkedNodes = this.$refs.myCascader.getCheckedNodes()
      checkedNodes.forEach((node) => {
        labelArray.push(node.label)
      })
      this.temp.WORK_SHOP = labelArray[0]
      //console.log('----级联选择器节点数据',labelArray)
    },
    // 根据车间地点Id获取安装地点数据
    getInstallSiteDataByPlaceId(id) {
      EquInfo.GetInstallSiteDataByPlaceId(id)
        .then((response) => {
          if (response.Code === 200) {
            this.installSiteOptions = response.Data
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 更新弹窗关闭按钮
    closeForm() {
      this.dialogFormVisible = false
      this.rowClick()
    },
    // 表格行点击事件
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    // 选框选择改变事件
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    // 设备类型添加按钮
    addEqu() {
      this.dialogVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm1'].clearValidate()
      })
    },
    //设备类型添加提交按钮
    submit() {
      this.$refs['dataForm1'].validate((valid) => {
        if (valid) {
          this.addEquType(this.temp1)
        }
      })
    },
    // 设备类型添加函数
    addEquType(addData1) {
      EquInfo.AddEquType(addData1).then((response) => {
        if (response.Code === 200) {
          this.dialogVisible = false
          this.$notify({
            title: '成功',
            message: '创建成功',
            type: 'success',
            duration: 2000
          })
        } else {
          this.$notify({
            title: '失败',
            message: '创建失败',
            type: 'warning',
            duration: 2000
          })
        }
        this.getEquType()
      })
    },
    // 更新点击按钮
    handleUpdate(row) {
      // 点击更新按钮时，将此行数据绑定在更新表单上
      this.temp = row
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 更新弹窗保存按钮
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const id = this.temp.EQU_ID
          this.updateEquType(id, this.temp)
        }
      })
    },
    // 更新接口函数
    updateEquType(id, data) {
      EquInfo.UpdateEquType(id, data)
        .then((response) => {
          if (response.Code === 200) {
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '更新成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getEquInfoPageList()
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 单个删除点击按钮
    handleDelete(row) {
      this.$confirm('此操作将永久删除该行数据，是否继续？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          let id = row.EQU_ID
          this.deleteEquInfoById(id)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 设备信息删除函数
    deleteEquInfoById(id) {
      EquInfo.DeleteEquInfo(id)
        .then((response) => {
          if (response.Code === 200) {
            if (this.total % this.listQuery.pageSize === 1) {
              this.listQuery.pageIndex = this.listQuery.pageIndex - 1
            }
            if (this.total === 1) {
              this.listQuery.pageIndex = 1
            }
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
            this.getEquInfoPageList()
          }
        })
        .catch((error) => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 设置表头颜色
    headClass() {
      return 'background:#337ab7'
    },
    // 弹窗数据重置
    resetTemp() {
      this.temp = {
        EQU_NAME: '',
        EQU_SPEC: '',
        EQU_STATUS: '',
        WORK_SHOP: '',
        INSTALL_SITE: '',
        HEAD: '',
        PHONE_NO: '',
        MANUFACTURER: '',
        VENDOR: '',
        PUR_TIME: '',
        ENABLE_TIME: '',
        CREATED_BY: ''
      }
    }
  }
}
</script>
<style scoped>
.filter-container {
  padding: 0 10px;
  margin-bottom: 5px;
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
.custom-tree-node {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 14px;
  padding-right: 8px;
}
.app-container {
  padding: 0;
}
.text {
  font-size: 16px;
}
.app-container .clearfix {
  position: relative;
}
.app-container .clearfix i {
  position: absolute;
  top: 8px;
  right: 10px;
  font-size: 20px;
  color: #00a0ff;
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
.el-card__header {
  padding: 12px 20px;
}
.body-small.dialog-mini .el-dialog__body .el-form {
  padding-right: 0px;
  padding-top: 0px;
}
.fh1 {
  background-color: #fff;
  height: 100% !important;
}

.fh3 {
  height: 35px !important;
}
.row {
  display: flex;
  flex-wrap: wrap;
}
.el-row .el-card {
  min-width: 100%;
  height: 100% !important;
  margin-right: 20px;
  transition: all 0.5s;
}
.el-tree .el-tree-node__expand-icon.expanded {
  -webkit-transform: rotate(0deg);
  transform: rotate(0deg);
}
.el-tree .el-icon-caret-right:before {
  content: '\e723';
}
.el-tree .el-tree-node__expand-icon.expanded.el-icon-caret-right:before {
  content: '\e722';
}
</style>
