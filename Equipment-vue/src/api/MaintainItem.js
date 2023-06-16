import request from '@/utils/request'

//获取所有点检项目
export function GetAll(){
    return request({
        url:'/MaintainItem/GetAll',
        method:'GET'
    })
}

// 获取点检项目分页数据
export function GetPageList(params){
    return request({
        url:'/MaintainItem/GetPageList',
        method:'GET',
        params:params
    })
}

// 根据type_id获取点检项目接口
export function GetItemsWithTypeId(data){
    return request({
        url:'/MaintainItem/GetByTypeId',
        method:'POST',
        data:data
    })
}

//点检项可批量删除接口
export function DeleteByIds(ids){
    return request({
        url:'/MaintainItem/DeleteByIds',
        method:'DELETE',
        data:ids
    })
}

//点检项新增接口
export function AddItem(data){
    return request({
        url:'/MaintainItem/Create',
        method:'POST',
        data:data
    })
}
// 点检项模糊查询接口
export function QueryItems(data){
    return request({
        url:'/MaintainItem/QueryAndPage',
        method:'POST',
        data:data
        }
    )
}