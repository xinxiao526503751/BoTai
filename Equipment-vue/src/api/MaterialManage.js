import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllMaterial(){
    return request({
        url:'/MaterialManagement/GetAllMaterialManagement',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetMaterialPageList(params){
    return request({
        url:'/MaterialManagement/GetMaterialManagementPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryMaterialList(params){
    return request({
        url:'/MaterialManagement/GetMaterialManagementByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeleteMaterial(id){
    return request({
        url:'/MaterialManagement/DeleteMaterialManagementById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteMaterialByIds(ids){
    return request({
        url:'/MaterialManagement/DeleteMaterialManagementsById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddMaterial(data){
    return request({
        url:'/MaterialManagement/AddMaterialManagement',
        method:'POST',
        data:data
    })
}
// 更新数据接口
export function UpdateMaterial(id,params){
    return request({
        url:'/MaterialManagement/UpdateMaterialManagementById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}



