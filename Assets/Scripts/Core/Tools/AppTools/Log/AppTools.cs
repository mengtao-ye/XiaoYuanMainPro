namespace Game
{
    public static partial class AppTools
    {
        /// <summary>
        ///打印成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void Log<T>( T msg )
        {
            GameCenter.Instance.LogSuccess(msg);
        }
        /// <summary>
        /// 打印警告消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void LogNotify<T>(T msg)
        {
            GameCenter.Instance.LogNotify(msg);
        }
        /// <summary>
        /// 打印错误消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void LogError<T>(T msg)
        {
            YFramework.LogHelper.LogError(msg);
            GameCenter.Instance.LogError(msg);
        }
    }
}
