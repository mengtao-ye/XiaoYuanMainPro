namespace Game
{
    /// <summary>
    /// APP变量数据
    /// </summary>
    public static class AppVarData
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public static int UserID { get { return userData != null ? userData.ID : -1; } }
        /// <summary>
        /// 用户账号
        /// </summary>
        public static long Account { get { return userData!=null? userData.Account : -1; } }
        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogin { get { return userData != null; } }
        /// <summary>
        /// 用户数据
        /// </summary>
        public static UserData userData { get; set; }


    }
}
