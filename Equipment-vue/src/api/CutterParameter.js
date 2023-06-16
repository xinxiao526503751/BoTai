import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllCutterParameter(){
    return request({
        url:'/CutterParameter/GetAllCutterParameter',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetCutterParameterPageList(params){
    return request({
        url:'/CutterParameter/GetCutterPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryCutterParameterList(params){
    return request({
        url:'/CutterParameter/GetCutterParameterByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeleteCutterParameter(id){
    return request({
        url:'/CutterParameter/DeleteCutterParameterById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteCutterParameterByIds(ids){
    return request({
        url:'/CutterParameter/DeleteCutterParametersById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddCutterParameter(addData){
    return request({
        url:'/CutterParameter/AddCutterParameter',
        method:'POST',
        data:addData
    })
}
// 更新数据接口
export function UpdateCutterParameter(id,params){
    return request({
        url:'/CutterParameter/UpdateCutterParameterById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}



