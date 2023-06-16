import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllEquipmentInformation(){
    return request({
        url:'/EquipmentInformation/GetAllEquipmentInformation',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetEquipmentInformationPageList(params){
    return request({
        url:'/EquipmentInformation/GetEquipmentInformationPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryEquipmentInformationList(params){
    return request({
        url:'/EquipmentInformation/GetEquipmentInformationByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeleteEquipmentInformation(id){
    return request({
        url:'/EquipmentInformation/DeleteEquipmentInformationById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteEquipmentInformationByIds(ids){
    return request({
        url:'/EquipmentInformation/DeleteEquipmentInformationsById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddEquipmentInformation(data){
    return request({
        url:'/EquipmentInformation/AddEquipmentInformation',
        method:'POST',
        data:data
    })
}
// 更新数据接口
export function UpdateEquipmentInformation(id,params){
    return request({
        url:'/EquipmentInformation/UpdateEquipmentInformationById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}

// 添加车间地点接口
export function AddWorkPlace(data){
    return request({
        url:'/AddWorkPlace/AddWorkPlace',
        method:'POST',
        data:data
    })
}

//获取车间地点全部数据接口
export function GetAllWorkPlace(){
    return request({
        url:'/AddWorkPlace/GetAllWorkPlace',
        method:'GET'
    })
}

// 添加设备类型接口
export function AddEqType(data){
    return request({
        url:'/AddEqType/AddEqType',
        method:'POST',
        data:data
    })
}

//获取设备类型全部数据接口
export function GetAllEqType(){
    return request({
        url:'/AddEqType/GetAllEqType',
        method:'GET'
    })
}


