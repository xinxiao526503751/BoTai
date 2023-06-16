import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllCutterType(){
    return request({
        url:'/CutterType/GetAllCutterType',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetCutterTypePageList(params){
    return request({
        url:'/CutterType/GetCutterPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryCutterTypeList(params){
    return request({
        url:'/CutterType/GetCutterTypeByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeleteCutterType(id){
    return request({
        url:'/CutterType/DeleteCutterTypeById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteCutterTypeByIds(ids){
    return request({
        url:'/CutterType/DeleteCutterTypesById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddCutterType(data){
    return request({
        url:'/CutterType/AddCutterType',
        method:'POST',
        data:data
    })
}
// 更新数据接口
export function UpdateCutterType(id,params){
    return request({
        url:'/CutterType/UpdateCutterTypeById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}



