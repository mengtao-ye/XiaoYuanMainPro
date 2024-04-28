using UnityEngine;
using YFramework;

namespace Game
{
    public class XiaoYuanUDPScoketManager : UdpServer
    {
        private YFramework.UdpBigDataManager mBigDataManager;
        private float mBigDataRefreshTimer;
        private BigDataController mBigDataController;
        public bool IsConnect { get; private set; } = true;
        private short mHeartBeatID = 0;
        private float mTimer;
        private float mTime = 1;
        private int mHeartBeatCount;
        public SubServerType subServerType { get; private set; }
        private string mName;
        public XiaoYuanUDPScoketManager(Center center, IMap<short, IUdpRequestHandle> map, SubServerType subServerType, string name) : base(center, map)
        {
            this.subServerType = subServerType;
            if (name.IsNullOrEmpty())
            {
                mName = "默认Udp服务器";
            }
            else
            {
                mName = name;
            }
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void Launcher(string ipAddress, int port, short heartBeatID)
        {
            mTimer = 1;
            isRun = true;
            mHeartBeatID = heartBeatID;
            try
            {
                Open(ipAddress, port);
                mBigDataController = new BigDataController(this);
                mBigDataManager = new UdpBigDataManager(mUdpSocket, YFramework.PlatformType.Client);
                AddReconnectCallback(ReconnectCallback);
            }
            catch (System.Exception e)
            {
                isRun = false;
                Debug.LogError(mName + "启动UDP失败:" + e.Message);
                return;
            }
            Debug.Log(mName + "UDP服务器启动成功！");
        }
        /// <summary>
        /// 重新连接
        /// </summary>
        private void ReconnectCallback()
        {
            ClearBigData();
            mBigDataManager.ResetSocket(mUdpSocket);
        }
        public override void Update()
        {
            base.Update();
            mTimer += Time.deltaTime;
            if (mTimer > mTime)
            {
                mTimer = 0;
                if (!IsConnect)
                {
                    ReConnectServer();
                }
                if (mHeartBeatID != 0)
                {
                    UdpSend(mHeartBeatID, (byte)UdpMsgType.SmallData, BytesConst.TRUE_BYTES);
                }
                mHeartBeatCount++;
                if (mHeartBeatCount > 4)
                {
                    mHeartBeatCount = 0;
                    IsConnect = false;
                }
            }
            mBigDataRefreshTimer += Time.deltaTime;
            if (mBigDataRefreshTimer >= SocketData.BIG_DATA_REFRESH_TIME)
            {
                mBigDataRefreshTimer = 0;
                mBigDataManager.RefreshSendData();
            }
        }
        /// <summary>
        /// 发送大数据到服务器上
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="sendData"></param>
        /// <param name="sendTarget"></param>
        public void UdpSendBigData(short udpCode, byte[] sendData, int userID)
        {
            mBigDataManager.SendBigData(udpCode, sendData, userID, serverPoint);
        }
        /// <summary>
        /// 接收到服务器的回调消息
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="index"></param>
        public void ReceiveCallBack(short udpCode, ushort index, bool isReceive)
        {
            if (isReceive)
            {
                mBigDataManager.Remove(AppVarData.UserID, udpCode);
            }
            else
            {
                mBigDataManager.ReceiveCallBack(AppVarData.UserID, udpCode, index);
            }
        }
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="udpCode"></param>
        public void Remove(short udpCode)
        {
            mBigDataManager.Remove(AppVarData.UserID, udpCode);
        }
        /// <summary>
        /// 清除正在传输中的大数据
        /// </summary>
        private void ClearBigData()
        {
            mBigDataManager.ClearPlayerData(AppVarData.UserID);
        }
        /// <summary>
        /// 接续收到的数据
        /// </summary>
        /// <param name="msg"></param>
        public override void ResponseData(YFramework.UdpMsg msg)
        {
            if (msg == null) return;
            if (msg.type == (byte)UdpMsgType.SmallData) //小数据
            {
                Response(msg.udpCode, msg.Data);
            }
            else//大数据的情况
            {
                mBigDataController.ReceiveData(msg.udpCode, msg.Data);
            }
        }
        /// <summary>
        /// 接收到心跳包
        /// </summary>
        public void Heart()
        {
            mHeartBeatCount = 0;
            IsConnect = true;
        }
    }
}
