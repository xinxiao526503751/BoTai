const check={
    state:{
        data:''
    },

    actions:{
        getCheck(context,value){
            context.commit('GETCHECK',value)
        }
    },

    mutations:{
        GETCHECK(state,value){
            state.data=value
            console.log('-------store',state.data)
        }
    },

}

export default check
