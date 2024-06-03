using YFramework;

namespace Game
{
    public class XiaoYuanLoginTcpSocketManager : XiaoYuanTCPSocketManager
    {
        public XiaoYuanLoginTcpSocketManager(Center center, IMap<short, ITcpRequestHandle> map, TcpSubServerType tcpSubServerType, string name) : base(center, map, tcpSubServerType, name)
        {
        }
    }
}
