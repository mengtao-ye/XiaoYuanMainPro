namespace Game
{
    public enum ServerNetType
    {
        Ali,
        Tencent,
        Local
    }
    /// <summary>
    /// 平台环境
    /// </summary>
    public enum PlatformType
    {
        Test,//测试环境
        Pre,//预生产环境
        Pro//正式环境
    }

    public static class AppData
    {
        public static ServerNetType netType
        {
            get
            {
                if (platformType == PlatformType.Test) return ServerNetType.Local;
                else return ServerNetType.Tencent;

            }
        }
        public static PlatformType platformType = PlatformType.Test;
        /// <summary>
        /// 当前运行的平台
        /// </summary>
        public static string RunPlatformName
        {
            get
            {
#if UNITY_WINDOW
          return "Windows";
#elif UNITY_ANDROID
                return "Android";
#elif UNITY_IOS 
         return "iOS";
#endif
            }
        }
    }
}
