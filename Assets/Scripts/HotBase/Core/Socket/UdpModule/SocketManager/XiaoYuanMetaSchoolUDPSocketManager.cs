using UnityEngine;
using YFramework;

namespace Game
{
    public class XiaoYuanMetaSchoolUDPSocketManager : XiaoYuanUDPScoketManager
    {
        private int mReconnectCount;
        public XiaoYuanMetaSchoolUDPSocketManager(Center center, IMap<short, IUdpRequestHandle> map, UdpSubServerType subServerType,string name) : base(center, map, subServerType,name)
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
                             .Concat(new GetMetaSchoolServerPointProcess())
                             ;
                    process.processManager.Launcher();
                }
            }
        }
    }
}
