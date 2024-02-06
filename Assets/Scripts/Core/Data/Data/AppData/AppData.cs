namespace Game
{
    public enum ServerNetType 
    { 
        Ali,
        Tencent,
        Local
    }

    public static class AppData
    {
#if UNITY_EDITOR
        /// <summary>
        /// 当前数据通道
        /// </summary>
        public const ServerNetType netType =  ServerNetType.Local;
#else
        /// <summary>
        /// 当前数据通道
        /// </summary>
       public const ServerNetType netType =  ServerNetType.Tencent;
#endif


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
