import request from '@/utils/request'

//获取所有点检项目
export function GetAllCheckItem(){
    return request({
        url:'/CheckItem/GetAll',
        method:'GET'
    })
}

// 获取点检项目分页数据
export function GetCheckItemPage(params){
    return request({
        url:'/CheckItem/GetPageList',
        method:'GET',
        params:params
    })
}

// 根据typeId获取点检项目接口
export function GetItemsWithTypeId(data){
    return request({
        url:'/CheckItem/GetByTypeId',
        method:'POST',
        data:data
    })
}

//点检项可批量删除接口
export function DeleteByIds(ids){
    return request({
        url:'/CheckItem/DeleteByIds',
        method:'DELETE',
        data:ids
    })
}

//点检项新增接口
export function AddCheckItem(data){
    return request({
        url:'/CheckItem/CreateCheckItem',
        method:'POST',
        data:data
    })
}
// 点检项模糊查询接口
export function QueryAndPage(data){
    return request({
        url:'/CheckItem/QueryAndPage',
        method:'POST',
        data:data
    })
}