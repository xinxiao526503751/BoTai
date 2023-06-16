import request from '@/utils/request'

//分页查询接口函数
export function GetPageList(params){
    return request({
        url:'/EquMaintainItem/QueryPageList',
        method:'Get',
        params:params
    })
}

//获取全部数据接口函数
export function GetAll(){
    return request({
        url:'/EquMaintainItem/GetAll',
        method:'GET'
    })
}

//根据双Id删除数据接口函数
export function DeleteByTwoId(params){
    return request({
        url:'/EquMaintainItem/DeleteByTwoId',
        method:'DELETE',
        params:params
    })
}

// 根据TypeId获取数据接口函数
export function GetByEquId(data){
    return request({
        url:'/EquMaintainItem/GetByEquId',
        method:'POST',
        data:data
    })
}
// 新增接口函数
export function Add(data){
    return request({
        url:'/EquMaintainItem/Create',
        method:'POST',
        data:data
    })
}

// 设备维修项模糊查询接口
export function QueryEquMaintainItem(data){
    return request({
        url:'/EquMaintainItem/QueryAndPage',
        method:'POST',
        data:data
    })
}

export function UpdateWithTwoId(params,data){
    return request({
        url:'/EquMaintainItem/UpdateWithTwoId',
        method:'PUT',
        params:params,
        data:data
    })
}

// 过滤分页查询
export function FilterAndPage(data){
    return request({
        url:'/EquMaintainItem/FilterAndPage',
        method:'POST',
        data:data
    })
}