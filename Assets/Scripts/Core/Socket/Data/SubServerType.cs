namespace Game
{
    /// <summary>
    /// 分布式服务器类型
    /// </summary>
    public enum SubServerType : byte
    {
        /// <summary>
        /// 中心服务器
        /// </summary>
        Center = 1,
        /// <summary>
        /// 分布式登录服务器
        /// </summary>
        Login = 2,
        /// <summary>
        /// 分布式校园服务器
        /// </summary>
        MetaSchool = 3,
    }
}
