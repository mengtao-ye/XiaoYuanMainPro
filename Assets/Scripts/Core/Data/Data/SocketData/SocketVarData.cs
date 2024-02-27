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
                switch (AppData.netType)
                {
                    case ServerNetType.Ali:
                        return IPAddressConstData.PRE_IP;
                    case ServerNetType.Tencent:
                        return IPAddressConstData.PRO_IP;
                    case ServerNetType.Local:
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
