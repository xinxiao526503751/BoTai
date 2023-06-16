namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 系统页面用到通用定义
    /// </summary>
    public static class SysDefine
    {

        /// <summary>
        /// 无效的Token
        /// </summary>
        public const int INVALID_TOKEN = 50001;     //token无效 （过期、错误、无效等）

        /// <summary>
        /// Token的名字
        /// </summary>
        public const string TOKEN_NAME = "Authorization";


        /// <summary>
        /// 后期用来区分应用系统。
        /// </summary>
        public const string AppKey = "HhmoconMesV1TZ";

        /// <summary>
        /// 缓存Key_ORG  实际使用时 使用key+org_id 来存储
        /// </summary>
        public const string CacheKey_Org = "SYSORG";

        /// <summary>
        /// 缓存Key
        /// </summary>
        public const string CacheKey_ROLE = "SYSORG";
    }
}
