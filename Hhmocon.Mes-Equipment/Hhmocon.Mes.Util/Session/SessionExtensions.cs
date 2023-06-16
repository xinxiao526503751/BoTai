using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Hhmocon.Mes.Util
{
    public static class SessionExtensions
    {
        /// <summary>
        /// 存储对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObj<T>(this ISession session, string key, T value)
        {
            string jsonstr = JsonConvert.SerializeObject(value);
            byte[] byteArray = Encoding.Default.GetBytes(jsonstr);
            session.Set(key, byteArray);
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key)
        {
            bool isvalue = session.TryGetValue(key, out byte[] byteArray);
            if (isvalue)
            {
                string str = Encoding.Default.GetString(byteArray);
                T val = JsonConvert.DeserializeObject<T>(str);
                return val;
            }
            else
            {
                return default(T);
            }
        }
    }
}
