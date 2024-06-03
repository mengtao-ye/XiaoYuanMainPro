namespace Game
{
    public static partial class AppTools
    {
        /// <summary>
        /// udp发送消息
        /// </summary>
        /// <param name="type">发送给哪个分布式服务器</param>
        /// <param name="udpCode">消息码</param>
        /// <param name="data">消息数据</param>
        public static void UdpSend(UdpSubServerType type, short udpCode, byte[] data)
        {
            GameCenter.Instance.UdpSend(type, udpCode, data);
        }
        /// <summary>
        /// tcp发送消息
        /// </summary>
        /// <param name="type">发送给哪个分布式服务器</param>
        /// <param name="udpCode">消息码</param>
        /// <param name="data">消息数据</param>
        public static void TcpSend(TcpSubServerType type, short udpCode, byte[] data)
        {
            GameCenter.Instance.TcpSend(type, udpCode, data);
        }
    }
}
