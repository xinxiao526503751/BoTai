using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Reflection;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// app层除了 添加 的操作
    /// 删改查基本都可以由皮卡丘来做掉
    /// </summary>
    public class BaseCustomerApp
    {
        private readonly IBaseCustomerRepository _baseCustomerRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseCustomerApp(IBaseCustomerRepository IBaseCustomerRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseCustomerRepository = IBaseCustomerRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }


        public class Test
        {
            private readonly IBaseCustomerRepository _baseCustomerRepository;
            private readonly PikachuRepository _pikachuRepository;
            private readonly IAuth _auth;

            public Test(IBaseCustomerRepository IBaseCustomerRepository, PikachuRepository pikachuRepository, IAuth auth)
            {
                _baseCustomerRepository = IBaseCustomerRepository;
                _pikachuRepository = pikachuRepository;
                _auth = auth;
            }
            public void method<T1, T2>()
            {
                System.Collections.Generic.List<T1> item = _pikachuRepository.GetAll<T1>();
            }

            public void ExportByClassName(object typename1, object typename2)
            {
                Type t1 = typename1.GetType();
                Type t2 = typename1.GetType();
                Type t3 = Type.GetType("base_customer");
                MethodInfo mi = GetType().GetMethod("method").MakeGenericMethod(new Type[] { Type.GetType("base_customer"), Type.GetType("base_customer") });
                mi.Invoke(this, null);
            }
        }

        /// <summary>
        /// 添加用户数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_customer InsertCustomer(base_customer data)
        {
            //取ID
            data.customer_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);

            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }
    }
}
