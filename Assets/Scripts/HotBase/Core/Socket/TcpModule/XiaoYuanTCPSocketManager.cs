using UnityEngine;
using YFramework;

namespace Game
{
    public class XiaoYuanTCPSocketManager : TCPServer
    {
        public XiaoYuanTCPSocketManager(Center center, IMap<short, ITcpRequestHandle> map) : base(center, map)
        {

        }
        public override void Awake()
        {
            try
            {
                Open(SocketVarData.IPAddress, SocketVarData.TcpServerPort);
            }
            catch (System.Exception e)
            {
                Debug.LogError("TCP启动失败：" + e.Message);
                return;
            }
            Debug.Log("TCP服务器启动成功！");
            isRun = true;
        }
    }
}
