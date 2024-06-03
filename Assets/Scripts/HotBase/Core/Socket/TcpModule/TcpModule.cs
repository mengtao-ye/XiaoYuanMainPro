using System.Collections.Generic;
using YFramework;

namespace Game
{
    public class TcpModule : BaseModule
    {
        private Dictionary<TcpSubServerType, XiaoYuanTCPSocketManager> mTcpDict;
        private List<XiaoYuanTCPSocketManager> mTcpList;
        public Dictionary<TcpSubServerType, XiaoYuanTCPSocketManager> tcpDict { get { return mTcpDict; } }
        private TcpHandleMapper mMapper;
        public XiaoYuanTCPSocketManager centerTcpServer { get; private set; }
        public TcpModule(Center center) : base(center)
        {
        }
        public override void Awake()
        {
            base.Awake();
            isRun = true;
            mTcpList = new List<XiaoYuanTCPSocketManager>();
            mTcpDict = new Dictionary<TcpSubServerType, XiaoYuanTCPSocketManager>();
            mMapper = new TcpHandleMapper();
            //centerTcpServer = AddTcpServer(TcpSubServerType.Center, SocketVarData.IPAddress, SocketVarData.TcpServerPort, "Tcp主服务器");
        }

        /// <summary>
        /// 添加分布式服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public XiaoYuanTCPSocketManager AddTcpServer(TcpSubServerType subServerType, string ipAddress, int port, string name)
        {
            if (mTcpDict.ContainsKey(subServerType))
            {
                mTcpList.Remove(mTcpDict[subServerType]);
                mTcpDict.Remove(subServerType);
            }
            XiaoYuanTCPSocketManager socket = null;
            switch (subServerType)
            {
                case TcpSubServerType.Center:
                    socket = new XiaoYuanTCPSocketManager(center, mMapper, subServerType, name);
                    break;
                case TcpSubServerType.Login:
                    socket = new XiaoYuanLoginTcpSocketManager(center, mMapper, subServerType, name);
                    break;
            }
            socket.Launcher(ipAddress, port);
            mTcpDict.Add(subServerType, socket);
            mTcpList.Add(socket);
            return socket;
        }

        public override void Update()
        {
            base.Update();
            for (int i = 0; i < mTcpList.Count; i++)
            {
                mTcpList[i].Update();
            }
        }
    }
}
