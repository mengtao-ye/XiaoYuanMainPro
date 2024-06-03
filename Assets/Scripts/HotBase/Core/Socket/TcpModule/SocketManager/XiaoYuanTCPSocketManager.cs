using UnityEngine;
using YFramework;

namespace Game
{
    public class XiaoYuanTCPSocketManager : TCPServer
    {
        private TcpSubServerType mTcpSubServerType;
        private string mName;
        public XiaoYuanTCPSocketManager(Center center, IMap<short, ITcpRequestHandle> map, TcpSubServerType tcpSubServerType,string name) : base(center, map)
        {
            mTcpSubServerType = tcpSubServerType;
            mName = name;
        }
        public void Launcher(string ipAddress,int port) 
        {
            Open(ipAddress,port,mName);
        }
       
    }
}
