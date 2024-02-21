using System;
using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 项目核心处理器
    /// </summary>
    public class GameCenter : SingletonMono<GameCenter>
    {
        private static bool mRun = true; //是否运行YFramwork框架
        #region Field
        public Center center { get; private set; } = null;
        private UdpModule mUdpModule;
        private XiaoYuanSceneManager mSceneManager;
        public CommonManager commonManager { get; private set; }
        private BridgeManager mBridgeManager;
        private ILiveManager mLiveManager;
        private IHttpManager mHttpManager;
        public IScene curScene { get { return mSceneManager.curScene; } }
        public ICanvas curCanvas { get { return mSceneManager.curScene.canvas; } }
        public IModel curModel { get { return mSceneManager.curScene.model; } }
        public IController curController { get { return mSceneManager.curScene.controller; } }
        private ProcessController mProcessController;
        public ProcessController processController { get { return mProcessController; } }
        #endregion
        #region Init
     
        /// <summary>
        /// 最先执行的方法
        /// </summary>
        private void InitData()
        {
            Instance = this;
            YFrameworkHelper.Instance = new XiaoYuanYFrameworkHelper();
        }
        private void Awake()
        {
              if (!mRun) return;
            InitData();
            mProcessController = new ProcessController();
            center = new Center(new UnityDebug(), new MyResources());
            mSceneManager = new XiaoYuanSceneManager(center, new SceneMapper());
            mUdpModule = new UdpModule(center);
            commonManager = new CommonManager(center);
            mLiveManager = new LiveManager(center);
            mBridgeManager = new BridgeManager(center);
            mHttpManager = new HttpManager(center);

            ConfigSceneManager();
            center.AddGame(mUdpModule);
            center.AddGame(commonManager);
            center.AddGame(mLiveManager);
            center.AddGame(mBridgeManager);
            center.AddGame(mHttpManager);
            center.AddGame(mSceneManager);

            center.Awake();
        }
        private void Start()
        {
            center.Start();
        }
        private void Update()
        {
            mProcessController.Update();
            center.Update();
        }

        public void LateUpdate()
        {
            center.LaterUpdate();
        }

        private void FixedUpdate()
        {
            center.FixedUpdate();
        }
        #endregion
        #region UI
        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void LogSuccess<T>(T msg)
        {
            ShowLogUI<MidLogUI>().ShowContent(msg.ToString(), NotifyType.Success);
        }

        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void LogError<T>(T msg)
        {
            ShowLogUI<MidLogUI>().ShowContent(msg.ToString(), NotifyType.Error);
        }

        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void LogNotify<T>(T msg)
        {
            ShowLogUI<MidLogUI>().ShowContent(msg.ToString(), NotifyType.Notify);
        }
        /// <summary>
        /// 显示LogUI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T ShowLogUI<T>() where T : BaseCustomLogUI, new()
        {
            return curCanvas.logUIManager.ShowLogUI<T>();
        }
        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T ShowTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            return curCanvas.showTipsPanel.ShowTipsUI<T>();
        }
        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            curCanvas.showTipsPanel.HideTipsUI<T>();
        }
        /// <summary>
        /// 获取提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            return curCanvas.showTipsPanel.GetTipsUI<T>();
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T ShowPanel<T>() where T : BaseCustomPanel, new()
        {
            return curCanvas.ShowPanel<T>();
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetPanel<T>() where T : BaseCustomPanel, new()
        {
            return curCanvas.FindPanel<T>();
        }
        #endregion
        #region SceneManager
        /// <summary>
        /// 配置场景加载数据
        /// </summary>
        private void ConfigSceneManager()
        {
            mSceneManager.SetLoadCompleteCallBack(SceneLoadComplete);
        }
        /// <summary>
        /// 场景加载完成的回调
        /// </summary>
        /// <param name="sceneName"></param>
        private void SceneLoadComplete(string sceneName)
        {
            GameObjectPoolModule.Clear();
            IEnumeratorModule.StopAllCoroutine();//停止所有的携程
        }
        /// <summary>
        /// 场景加载
        /// </summary>
        /// <param name="sceneName">加载的名称</param>
        /// <param name="loadPorcess">加载的进度</param>
        public void LoadScene(SceneID sceneName, Action<float> loadPorcess = null)
        {
            mSceneManager.LoadScene(sceneName.ToString(), loadPorcess);
        }
        #endregion
        #region UdpManager
        public bool CenterUdpServerIsConnect
        {
            get
            {
                return mUdpModule.centerUdpServer.IsConnect;
            }
        }
        /// <summary>
        /// 接收到心跳包
        /// </summary>
        /// <param name="subServerType"></param>
        public void UdpHeart(SubServerType subServerType)
        {
            mUdpModule.Heart(subServerType);
        }

        /// <summary>
        /// udp发送数据
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        private void UdpSend(short udpCode, byte[] data, XiaoYuanUDPScoketManager udpServer)
        {
            if (!mUdpModule.isRun) return;
            if (data == null) return;
            if (udpServer == null) return;
            if (data.Length < SocketData.UDP_SPLIT_LENGTH) //发送的是小数据
            {
                udpServer.UdpSend(udpCode, (byte)UdpMsgType.SmallData, data);
            }
            else
            {
                udpServer.UdpSendBigData(udpCode, data, AppVarData.UserID);
            }
        }
        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void AddUdpServer(SubServerType subServerType, string ipAddress, int port, short heartBeatID)
        {
            mUdpModule.AddUdpServer(subServerType, ipAddress, port, heartBeatID);
        }
        /// <summary>
        /// 移除udpServer
        /// </summary>
        /// <param name="subServerType"></param>
        public void RemoveUdpServer(SubServerType subServerType)
        {
            mUdpModule.RemoveUdpServer(subServerType);
        }
        /// <summary>
        ///udp发送消息
        /// </summary>
        /// <param name="type">服务器类型</param>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        public void UdpSend(SubServerType type, short udpCode, byte[] data)
        {
            if (mUdpModule.udpDict.ContainsKey(type))
            {
                UdpSend(udpCode, data, mUdpModule.udpDict[type]);
            }
        }
        public void ReConnectServer()
        {
            if (!mUdpModule.isRun) return;
            mUdpModule.centerUdpServer.ReConnectServer();
        }
        /// <summary>
        /// 移除大数据里面的数据块
        /// </summary>
        /// <param name="udpCode"></param>
        public void RemoveBigDataItem(short udpCode)
        {
            if (!mUdpModule.isRun) return;

            mUdpModule.centerUdpServer.Remove(udpCode);
        }
        /// <summary>
        /// 移除大数据里面的数据块
        /// </summary>
        /// <param name="udpCode"></param>
        public void ReceiveCallBack(short udpCode, ushort index, bool isReceive)
        {
            if (!mUdpModule.isRun) return;

            mUdpModule.centerUdpServer.ReceiveCallBack(udpCode, index, isReceive);
        }
        #endregion
        #region TcpMangaer
        /// <summary>
        /// Tcp发送数据
        /// </summary>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        //public void TcpSend(short actionCode, byte[] data)
        //{
        //    mTcpSocket.TcpSend(actionCode, data);
        //}
        #endregion
        #region LiveManager
        public ILive AddUpdate(float freshTime, Action callBack)
        {
            return mLiveManager.AddUpdate(freshTime, callBack);
        }
        #endregion
        #region 原生之间调用
        public string UnityToAndroid(int id, int value1, int value2, int value3, string str1, string str2, string str3)
        {
            if (mBridgeManager.isRun)
            {
                return mBridgeManager.UnityToAndroid(id, value1, value2, value3, str1, str2, str3);
            }
            return null;
        }
        #endregion
        #region HttpManager
        /// <summary>
        /// 发送获取图片的Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <param name="errorCallBack"></param>
        public void SendGetSpriteRequest(string url, Action<Sprite> callBack, Action<string> errorCallBack)
        {
            mHttpManager.SendSpriteRequest(url, callBack, errorCallBack);
        }
        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <param name="errorCallBack"></param>
        public void SendSpriteRequest<T>(string url, Action<Sprite, T> callBack, Action<string> errorCallBack, T value)
        {
            mHttpManager.SendSpriteRequest(url, callBack, errorCallBack, value);
        }
        #endregion
    }
}
