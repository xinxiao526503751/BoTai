import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllCutterFaultType(){
    return request({
        url:'/CutterFaultType/GetAllCutterFaultType',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetCutterFaultTypePageList(params){
    return request({
        url:'/CutterFaultType/GetCutterPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryCutterFaultTypeList(params){
    return request({
        url:'/CutterFaultType/GetCutterFaultTypeByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeleteCutterFaultType(id){
    return request({
        url:'/CutterFaultType/DeleteCutterFaultTypeById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteCutterFaultTypeByIds(ids){
    return request({
        url:'/CutterFaultType/DeleteCutterFaultTypesById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddCutterFaultType(data){
    return request({
        url:'/CutterFaultType/AddCutterFaultType',
        method:'POST',
        data:data
    })
}
// 更新数据接口
export function UpdateCutterFaultType(id,params){
    return request({
        url:'/CutterFaultType/UpdateCutterFaultTypeById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}



