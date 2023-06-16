import request from '@/utils/request'
// 获取设备类型全部数据接口
export function GetAllEquType(){
    return request({
        url:'/EquType/GetAll',
        method:'GET'
    })
}
// 获取设备类型数据分页查询接口
export function GetEquTypePage(params){
    return request({
        url:'/EquType/GetEquTypePage',
        method:'GET',
        params:params
    })
}
// 获取设备信息分页数据接口
export function GetEquInfosPage(params){
    return request({
        url:'/EquInfo/GetPageList',
        method:'GET',
        params:params
    })
}
// 获取每一种设备类型下的所有数据
export function GetByTypeId(data){
    return request({
        url:'/EquInfo/GetByTypeId',
        method:'POST',
        data:data
    })
}
// 设备类型添加接口
export function AddEquType(data){
    return request({
        url:'/EquType/CreateEquType',
        method:'POST',
        data:data
    })
}
// 设备类型删除接口
export function DeleteEquType(id){
    return request({
        url:'/EquType/DeleteEquType',
        method:'DELETE',
        params:{
            id:id
        }
    })
}

// 设备信息删除接口
export function DeleteEquInfo(id){
    return request({
        url:'/EquInfo/DeleteById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}

// 设备信息可批量删除接口
export function DeleteByIds(ids){
    return request({
        url:'/EquInfo/DeleteByIds',
        method:'DELETE',
        data:ids
    })
}

// 设备信息新增接口
export function AddEquInfo(data){
    return request({
        url:'/EquInfo/CreateEquInfo',
        method:'POST',
        data:data
    })
}

// 模糊查询接口
export function QueryEquInfo(data){
    return request({
        url:'/EquInfo/QueryAndPage',
        method:'POST',
        data:data
    })
}

// 设备类型更新接口
export function UpdateEquType(id,data){
    return request({
        url:'/EquInfo/UpdateEquType',
        method:'PUT',
        params:{id:id},
        data:data
    })
}

//获取全部设备信息数据
export function GetAllEquInfo(){
    return request({
        url:'/EquInfo/GetAll',
        method:'GET'
    })
}

// 获取车间树
export function GetWorkPlaceTree(){
    return request({
        url:'/WorkPlace/GetAll',
        method:'GET'
    })
}
// 车间地点新增函数
export function AddWorkPlace(data){
    return request({
        url:'/WorkPlace/AddWorkPlace',
        method:'POST',
        data:data
    })
}

// 车间地点删除函数
export function DeleteWorkPlace(id){
    return request({
        url:'/WorkPlace/DeleteById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 获取全部安装地点数据
export function GetAllInstallSiteData(){
    return request({
        url:'/InstallSite/GetAll',
        method:'GET'
    })
}
// 安装地点新增函数
export function AddInstallSite(data){
    return request({
        url:'/InstallSite/AddInstallSite',
        method:'POST',
        data:data
    })
}

// 安装地点删除函数
export function DeleteInstallSite(id){
    return request({
        url:'/InstallSite/DeleteById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 根据车间地点Id获取安装地点数据
export function GetInstallSiteDataByPlaceId(id){
    return request({
        url:'/InstallSite/GetById',
        method:'GET',
        params:{
            id:id
        }
    })
}