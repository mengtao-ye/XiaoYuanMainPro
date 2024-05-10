namespace Game
{
    /// <summary>
    /// sokcet变量数据
    /// </summary>
    public static class SocketVarData
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public static string IPAddress
        {
            get {
                switch (AppData.platformType)
                {
                    case PlatformType.Pre:
                        return IPAddressConstData.PRE_IP;
                    case PlatformType.Pro:
                        return IPAddressConstData.PRO_IP;
                    case PlatformType.Test:
                        return IPAddressConstData.LOCAL_IP;
                }
                return null;
            }
        }
        /// <summary>
        ///校园服务器 ip地址
        /// </summary>
        public static string MetaSchoolIPAddress
        {
            get
            {
                switch (AppData.platformType)
                {
                    case PlatformType.Pre:
                        return IPAddressConstData.PRE_IP;
                    case PlatformType.Pro:
                        return IPAddressConstData.PRO_IP;
                    case PlatformType.Test:
                        return IPAddressConstData.LOCAL_IP;
                }
                return null;
            }
        }
        /// <summary>
        ///登录服务器 ip地址
        /// </summary>
        public static string LoginIPAddress
        {
            get
            {
                switch (AppData.platformType)
                {
                    case PlatformType.Pre:
                        return IPAddressConstData.PRE_IP;
                    case PlatformType.Pro:
                        return IPAddressConstData.PRO_IP;
                    case PlatformType.Test:
                        return IPAddressConstData.LOCAL_IP;
                }
                return null;
            }
        }

        /// <summary>
        /// udp服务端口号
        /// </summary>
        public const int UdpServerPort = 50000;
        /// <summary>
        /// tcp服务端口号
        /// </summary>
        public const int TcpServerPort = 50001;
    }
}
