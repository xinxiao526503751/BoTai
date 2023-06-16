<template>
  <div class="app">
    <div class="header">
      <el-menu
        class="el-menu-demo"
        mode="horizontal"
        @select="handleSelect"
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b"
      >
        <el-menu-item @click="onClick1">设备类型维护</el-menu-item>
        <el-menu-item @click="onClick2">车间地点维护</el-menu-item>
        <el-menu-item @click="onClick3">安装地点维护</el-menu-item>
      </el-menu>
    </div>
    <div class="infos">
      <div class="equType" v-show="typeShow">
        <div class="head" style="position: relative">
          <el-button
            icon="el-icon-circle-plus-outline"
            style="margin-left: 20px"
            @click="typeAdd"
            >类型新增</el-button
          >
          <el-button
            icon="el-icon-circle-close"
            style="position: absolute; right: 20px"
            @click="typeDelete"
            >类型删除</el-button
          >
        </div>
        <div class="body">
          <el-table
            ref="typeTable"
            :data="typeData"
            highlight-current-row
            stripe
            border
            fit
            style="width: 100%"
            :header-cell-style="headClass"
            @row-click="typeRowClick"
            @selection-change="typeHandleSelectionChange"
          >
            <el-table-column
              prop="EQU_TYPE_ID"
              label="设备类型编号"
              min-width="180"
              align="center"
            >
            </el-table-column>
            <el-table-column
              prop="EQU_TYPE_NAME"
              label="设备类型名称"
              min-width="180"
              align="center"
            >
            </el-table-column>
          </el-table>
        </div>
      </div>
      <div class="workPlace" v-show="placeShow">
        <div class="head" style="position: relative">
          <el-button
            icon="el-icon-circle-plus-outline"
            style="margin-left: 20px"
            @click="workPlaceAdd"
            >车间地点新建</el-button
          >
          <el-button
            icon="el-icon-circle-close"
            style="position: absolute; right: 20px"
            @click="workPlaceDelete"
            >车间地点删除</el-button
          >
        </div>
        <div class="body">
          <el-table
            ref="workPlaceTable"
            :data="workPlaceData"
            highlight-current-row
            stripe
            border
            fit
            style="width: 100%"
            :header-cell-style="headClass"
            @row-click="workPlaceRowClick"
            @selection-change="workPlaceHandleSelectionChange"
          >
            <el-table-column
              prop="PlaceId"
              label="车间地点Id"
              min-width="180"
              align="center"
            >
            </el-table-column>
            <el-table-column
              prop="PlaceName"
              label="车间地点名称"
              min-width="180"
              align="center"
            >
            </el-table-column>
            <el-table-column
              prop="ParentPlaceId"
              label="父级车间Id"
              min-width="180"
              align="center"
            >
            </el-table-column>
          </el-table>
        </div>
      </div>
      <div class="installSite" v-show="siteShow">
        <div class="head" style="position: relative">
          <el-button
            icon="el-icon-circle-plus-outline"
            style="margin-left: 20px"
            @click="installSiteAdd"
            >安装地点新建</el-button
          >
          <el-button
            icon="el-icon-circle-close"
            style="position: absolute; right: 20px"
            @click="installSiteDelete"
            >安装地点删除</el-button
          >
        </div>
        <div class="body">
          <el-table
            ref="installSiteTable"
            :data="installSiteData"
            highlight-current-row
            stripe
            border
            fit
            style="width: 100%"
            :header-cell-style="headClass"
            @row-click="installSiteRowClick"
            @selection-change="installSiteHandleSelectionChange"
          >
            <el-table-column
              prop="SiteId"
              label="安装地点Id"
              min-width="180"
              align="center"
            >
            </el-table-column>
            <el-table-column
              prop="SiteName"
              label="安装地点名称"
              min-width="180"
              align="center"
            >
            </el-table-column>
          </el-table>
        </div>
      </div>
    </div>
    <!-- 设备类型新增抽屉 -->
    <el-drawer
      title="设备类型-新增"
      :visible.sync="typeDrawer"
      :direction="direction"
      :before-close="handleClose"
      :modal="false"
    >
      <el-form
        :rules="rules"
        ref="dataForm"
        :model="typeTempData"
        label-position="right"
        label-width="140px"
      >
        <el-row>
          <el-col>
            <el-form-item
              size="small"
              :label="'设备类型编号：'"
              prop="EQU_TYPE_ID"
            >
              <el-input
                v-model="typeTempData.EQU_TYPE_ID"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item
              size="small"
              :label="'设备类型名称：'"
              prop="EQU_TYPE_NAME"
            >
              <el-input
                v-model="typeTempData.EQU_TYPE_NAME"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'添加人：'" prop="CREATED_BY">
              <el-input
                v-model="typeTempData.CREATED_BY"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-drawer>
    <!-- 车间地点新增抽屉 -->
    <el-drawer
      title="车间地点-新增"
      :visible.sync="workPlaceDrawer"
      :direction="direction"
      :before-close="handleClose2"
      :modal="false"
    >
      <el-form
        :rules="rules2"
        ref="dataForm2"
        :model="workPlaceTempData"
        label-position="right"
        label-width="140px"
      >
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'车间代号：'" prop="PlaceId">
              <el-input
                v-model="workPlaceTempData.PlaceId"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'车间名称：'" prop="PlaceName">
              <el-input
                v-model="workPlaceTempData.PlaceName"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item
              size="small"
              :label="'父级车间代号：'"
              prop="ParentPlaceId"
            >
              <el-input
                v-model="workPlaceTempData.ParentPlaceId"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'添加人：'" prop="CREATED_BY">
              <el-input
                v-model="workPlaceTempData.CREATED_BY"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-drawer>
    <!-- 安装地点新增抽屉 -->
    <el-drawer
      title="安装地点-新增"
      :visible.sync="installSiteDrawer"
      :direction="direction"
      :before-close="handleClose3"
      :modal="false"
    >
      <el-form
        :rules="rules3"
        ref="dataForm3"
        :model="installSiteTempData"
        label-position="right"
        label-width="140px"
      >
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'所属车间名称：'" prop="PlaceId">
              <el-cascader
                v-model="installSiteTempData.PlaceId"
                :options="options"
                style="width: 300px"
                clearable
                @change="handleChange"
              >
              </el-cascader>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'安装地点编号：'" prop="SiteId">
              <el-input
                v-model="installSiteTempData.SiteId"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item
              size="small"
              :label="'安装地点名称：'"
              prop="SiteName"
            >
              <el-input
                v-model="installSiteTempData.SiteName"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col>
            <el-form-item size="small" :label="'添加人：'" prop="CREATED_BY">
              <el-input
                v-model="installSiteTempData.CREATED_BY"
                style="width: 300px"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-drawer>
  </div>
</template>

<script>
import * as EquInfo from '@/api/EquInfo'
import { listToTreeSelect, arrayToTree } from '@/utils'
export default {
  name: 'InfoMaintain',
  data() {
    return {
      typeShow: true,
      placeShow: false,
      siteShow: false,
      typeData: null, //设备类型数据
      workPlaceData: null, //车间地点数据
      installSiteData: null, //安装地点数据
      typeDrawer: false, //设备新增右侧抽屉
      workPlaceDrawer: false,
      installSiteDrawer: false,
      direction: 'rtl',
      typeTempData: {
        EQU_TYPE_ID: '',
        EQU_TYPE_NAME: '',
        CREATED_BY: ''
      },
      workPlaceTempData: {
        PlaceId: '',
        PlaceName: '',
        ParentPlaceId: '',
        CREATED_BY: ''
      },
      installSiteTempData: {
        SiteId: '',
        SiteName: '',
        PlaceId: '',
        CREATED_BY: ''
      },
      rules: {
        EQU_TYPE_ID: [
          {
            required: true,
            message: '设备类型编号不能为空',
            trigger: 'blur'
          }
        ],
        EQU_TYPE_NAME: [
          {
            required: true,
            message: '设备类型名称不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: true,
            message: '添加人不能为空',
            trigger: 'blur'
          }
        ]
      },
      rules2: {
        PlaceId: [
          {
            required: true,
            message: '车间地点Id不能为空',
            trigger: 'blur'
          }
        ],
        PlaceName: [
          {
            required: true,
            message: '车间地点名称不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: true,
            message: '添加人员不能为空',
            trigger: 'blur'
          }
        ]
      },
      rules3: {
        SiteId: [
          {
            required: true,
            message: '安装地点Id不能为空',
            trigger: 'blur'
          }
        ],
        SiteName: [
          {
            required: true,
            message: '安装地点名称不能为空',
            trigger: 'blur'
          }
        ],
        PlaceId: [
          {
            required: true,
            message: '所属车间Id不能为空',
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
      multipleTypeSelection: [], //类型表格行数据
      multipleWorkPlaceSelection: [], //车间地点行数据
      multipleInstallSiteSelection: [], //安装地点行数据
      typeId: null, //设备Id
      workPlaceId: null, //车间地点Id
      installSiteId: null, //安装地点Id
      workPlaceFilterData: null,
      options: [] //级联选择器数据
    }
  },
  created() {},
  mounted() {
    this.getEquType()
    this.getWorkPlaceData()
    this.getInstallSiteData()
  },
  methods: {
    onClick1() {
      this.typeShow = true
      this.placeShow = false
      this.siteShow = false
    },
    onClick2() {
      this.typeShow = false
      this.placeShow = true
      this.siteShow = false
    },
    onClick3() {
      this.typeShow = false
      this.placeShow = false
      this.siteShow = true
    },
    handleSelect() {},
    // 设备类型获取
    getEquType() {
      EquInfo.GetAllEquType().then((response) => {
        if (response.Code === 200) {
          this.typeData = response.Data
        }
      })
    },
    // 类型新增按钮
    typeAdd() {
      this.typeDrawer = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
        this.resetTypeData()
      })
    },
    // 类型新增函数
    typeAddFun(data) {
      EquInfo.AddEquType(data).then((response) => {
        if (response.Code === 200) {
          this.$notify({
            title: '成功',
            message: '新增成功',
            type: 'success',
            duration: 2000
          })
        } 
        this.getEquType()
      }).catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 类型删除函数
    typeDeleteFun(id) {
      EquInfo.DeleteEquType(id)
        .then((response) => {
          if (response.Code === 200) {
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getEquType()
        })
        .catch(() => {
          this.$message({
            message: '抱歉，该设备类型下绑定多条信息，无法越级删除',
            type: 'error'
          })
        })
    },
    // 类型删除按钮
    typeDelete() {
      if (this.typeId === null) {
        this.$notify({
          title: '警告',
          message: '至少删除一条数据',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.$confirm('此操作将永久删除该行数据，是否继续？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          let id = this.typeId
          this.typeDeleteFun(id)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 设备类型表格行点击事件
    typeRowClick(row) {
      this.$refs.typeTable.clearSelection()
      this.$refs.typeTable.toggleRowSelection(row)
      this.typeId = row.EQU_TYPE_ID
    },
    // 车间地点表格行点击事件
    workPlaceRowClick(row) {
      this.$refs.workPlaceTable.clearSelection()
      this.$refs.workPlaceTable.toggleRowSelection(row)
      this.workPlaceId = row.PlaceId
    },
    // 安装地点表格行点击事件
    installSiteRowClick(row) {
      this.$refs.installSiteTable.clearSelection()
      this.$refs.installSiteTable.toggleRowSelection(row)
      this.installSiteId = row.SiteId
    },
    // 设备表格选框事件
    typeHandleSelectionChange(val) {
      this.multipleTypeSelection = val
    },
    // 车间地点选框事件
    workPlaceHandleSelectionChange(val) {
      this.multipleWorkPlaceSelection = val
    },
    // 安装地点选框事件
    installSiteHandleSelectionChange(val) {
      this.multipleInstallSiteSelection = val
    },
    // 车间地点数据获取
    getWorkPlaceData() {
      EquInfo.GetWorkPlaceTree()
        .then((response) => {
          if (response.Code === 200) {
            this.workPlaceData = response.Data
            this.workPlaceFilterData = response.Data
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 车间地点新增按钮
    workPlaceAdd() {
      this.workPlaceDrawer = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm2'].clearValidate()
        this.resetWorkPlaceData()
      })
    },
    // 车间地点新增函数
    workPlaceAddFun(data) {
      EquInfo.AddWorkPlace(data)
        .then((response) => {
          if (response.Code === 200) {
            this.$notify({
              title: '成功',
              message: '新增成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getWorkPlaceData()
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 车间地点删除按钮
    workPlaceDelete() {
      if (this.workPlaceId === null) {
        this.$notify({
          title: '警告',
          message: '至少删除一条数据',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.$confirm('此操作将永久删除该行数据，是否继续？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          let id = this.workPlaceId
          let count = 0
          for (let i = 0; i < this.workPlaceFilterData.length; i++) {
            if (this.workPlaceFilterData[i].ParentPlaceId === id) {
              count++
            }
          }
          if (count !== 0) {
            this.$message({
              type: 'warning',
              message: `抱歉，该项数据绑定${count}条子数据，请先进行删除子数据`
            })
            return
          }
          this.workPlaceDeleteFun(id)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 车间删除函数
    workPlaceDeleteFun(id) {
      EquInfo.DeleteWorkPlace(id)
        .then((response) => {
          if (response.Code === 200) {
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getWorkPlaceData()
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 获取安装地点数据
    getInstallSiteData() {
      EquInfo.GetAllInstallSiteData()
        .then((response) => {
          if (response.Code === 200) {
            this.installSiteData = response.Data
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 安装地点新增按钮
    installSiteAdd() {
      this.installSiteDrawer = true
      // 获取级联选择器数据
      this.getPlaceData()
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm3'].clearValidate()
        this.resetInstallSiteData()
      })
    },
    // 安装地点新增函数
    installSiteAddFun(data) {
      EquInfo.AddInstallSite(data)
        .then((response) => {
          if (response.Code === 200) {
            this.$notify({
              title: '成功',
              message: '新增成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getInstallSiteData()
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    //安装地点删除按钮
    installSiteDelete() {
      if (this.installSiteId === null) {
        this.$notify({
          title: '警告',
          message: '至少删除一条数据',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.$confirm('此操作将永久删除该行数据，是否继续？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          let id = this.installSiteId
          this.installSiteDeleteFun(id)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 安装地点删除函数
    installSiteDeleteFun(id) {
      EquInfo.DeleteInstallSite(id)
        .then((response) => {
          if (response.Code === 200) {
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
          }
          this.getInstallSiteData()
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    //获取级联选择器数据
    getPlaceData() {
      EquInfo.GetWorkPlaceTree().then((response) => {
        if (response.Code === 200) {
          var tempTree = response.Data.map(function (item) {
            return {
              id: item.PlaceId,
              value: item.PlaceId,
              label: item.PlaceName,
              parentId: item.ParentPlaceId
            }
          })
          var tempPlaceTreeData = JSON.parse(JSON.stringify(tempTree))
          this.options = listToTreeSelect(tempPlaceTreeData)
          //console.log(this.options)
        }
      })
    },
    //级联选择器改变事件
    handleChange(val) {
      for (let i = 0; i < val.length; i++) {
        if (i === val.length - 1) {
          this.installSiteTempData.PlaceId = val[i]
        }
        //console.log('----数据',this.installSiteTempData.PlaceId)
      }
    },
    // 设备新增右侧弹窗关闭事件
    handleClose(done) {
      this.$confirm('确认关闭？')
        .then((_) => {
          if (
            this.typeTempData.EQU_TYPE_ID === '' ||
            this.typeTempData.EQU_TYPE_NAME === '' ||
            this.typeTempData.CREATED_BY === ''
          ) {
            this.$notify({
              title: '警告',
              message: '数据为空，无法进行新增',
              type: 'warning',
              duration: 2000
            })
            done()
            return
          }
          this.typeAddFun(this.typeTempData)
          done()
        })
        .catch((_) => {})
    },
    // 车间地点新增弹窗关闭事件
    handleClose2(done) {
      this.$confirm('确认关闭？')
        .then((_) => {
          if (this.workPlaceTempData.ParentPlaceId === '') {
            this.workPlaceTempData.ParentPlaceId = null
          }
          if (
            this.workPlaceTempData.PlaceId === '' ||
            this.workPlaceTempData.PlaceName === '' ||
            this.workPlaceTempData.CREATED_BY === ''
          ) {
            this.$notify({
              title: '警告',
              message: '数据为空，无法进行新增',
              type: 'warning',
              duration: 2000
            })
            done()
            return
          }
          this.workPlaceAddFun(this.workPlaceTempData)
          done()
        })
        .catch((_) => {})
    },
    //安装地点新增弹窗关闭事件
    handleClose3(done) {
      this.$confirm('确认关闭？')
        .then((_) => {
          if (
            this.installSiteTempData.SiteId === '' ||
            this.installSiteTempData.SiteName === '' ||
            this.installSiteTempData.PlaceId === '' ||
            this.installSiteTempData.CREATED_BY === ''
          ) {
            this.$notify({
              title: '警告',
              message: '数据为空，无法进行新增',
              type: 'warning',
              duration: 2000
            })
            done()
            return
          }
          this.installSiteAddFun(this.installSiteTempData)
          done()
        })
        .catch((_) => {})
    },
    // 表头颜色
    headClass() {
      return {
        background: '#ccc',
        'font-weight': '700'
      }
    },
    // 重置设备类型新增输入框
    resetTypeData() {
      this.typeTempData = {
        EQU_TYPE_ID: '',
        EQU_TYPE_NAME: '',
        CREATED_BY: ''
      }
    },
    // 重置车间地点新增输入框
    resetWorkPlaceData() {
      this.workPlaceTempData = {
        PlaceId: '',
        PlaceName: '',
        ParentPlaceId: '',
        CREATED_BY: ''
      }
    },
    //重置安装地点新增输入框
    resetInstallSiteData() {
      this.installSiteTempData = {
        SiteId: '',
        SiteName: '',
        PlaceId: '',
        CREATED_BY: ''
      }
    }
  }
}
</script>

<style scoped>
.app {
  width: 100%;
  height: 100%;
}
.header {
  width: 100%;
}
.infos {
  width: 100%;
  height: calc(100% - 80px);
  margin-top: 10px;
  background-color: #fff;
}
.infos .head {
  width: 100%;
  height: 40px;
  margin-bottom: 10px;
  background-color: #333;
}
</style>