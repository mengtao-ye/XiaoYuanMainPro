using UnityEngine;
using YFramework;

namespace Game
{
    public class XiaoYuanLoginUDPSocketManager : XiaoYuanUDPScoketManager
    {
        private int mReconnectCount;
        public XiaoYuanLoginUDPSocketManager(Center center, IMap<short, IUdpRequestHandle> map, SubServerType subServerType,string name) : base(center, map, subServerType,name)
        {

        }
        public override void Awake()
        {
            base.Awake();
            mReconnectCount = 0;
        }

        public override void ReConnectServer()
        {
            base.ReConnectServer();
            if (IsConnect)
            {
                mReconnectCount = 0;
            }
            else
            {
                mReconnectCount++;
                if (mReconnectCount > 3)
                {
                    AppTools.ToastError(subServerType.ToString()+"服务器断开连接");
                    //超过三次了表示这个连接断开了，需要重新发起连接
                    GameCenter.Instance.RemoveUdpServer(subServerType);
                    IProcess process = GameCenter.Instance.processController.Create()
                             .Concat(new CheckMainServerIsInitProcess())
                             .Concat(new GetLoginServerPointProcess())
                             ;
                    process.processManager.Launcher();
                }
            }
        }
    }
}
