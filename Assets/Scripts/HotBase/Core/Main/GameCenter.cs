using System;
using System.Text;
using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 项目核心处理器
    /// </summary>
    public class GameCenter : SingletonMono<GameCenter>
    {
        public static bool isRun =//是否运行YFramwork框架  
#if UNITY_EDITOR
         true;
#else
          false; 
#endif
        #region Public 
        public PlatformType platformType;
        public Center center { get; private set; } = null;
        #endregion
        #region Field
        private UdpModule mUdpModule;
        private TcpModule mTcpModule;
        private XiaoYuanSceneManager mSceneManager;
        public CommonManager commonManager { get; private set; }
        //private BridgeManager mBridgeManager;
        private ILiveManager mLiveManager;
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
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            YFrameworkHelper.Instance = new XiaoYuanYFrameworkHelper();
            LogHelper.Instance = new UnityLogHelper();
#if UNITY_EDITOR
            ResourceHelper.Instance = new ResourcesLoadHelper();
#else
            ResourceHelper.Instance = new XiaoYuanABLoadHelper();
#endif
#if UNITY_EDITOR

            AppVarData.userData = new UserData();
            AppVarData.userData.Account = 18379366314;
            SchoolGlobalVarData.SchoolCode = 4136014839;
            MetaSchoolGlobalVarData.SetMyMetaSchoolData(new MyMetaSchoolData() { RoleID = 10000001 });
            MetaSchoolGlobalVarData.SetSchoolData(new SchoolData() { assetBundleName = "default_scene" });
#endif
            AppData.platformType = platformType;
            Debug.Log("platformType:" + AppData.platformType.ToString());
#if UNITY_EDITOR
            Init();
#endif
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            mProcessController = new ProcessController();
            center = new Center();
            mUdpModule = new UdpModule(center);
            mTcpModule = new TcpModule(center);
            commonManager = new CommonManager(center);
            mLiveManager = new LiveManager(center);
            //mBridgeManager = new BridgeManager(center);
            mSceneManager = new XiaoYuanSceneManager(center, new SceneMapper());
            ConfigSceneManager();
            center.AddGame(mUdpModule);
            center.AddGame(mTcpModule);
            center.AddGame(commonManager);
            center.AddGame(mLiveManager);
            //center.AddGame(mBridgeManager);
            center.AddGame(mSceneManager);

            center.Awake();
            center.Start();
        }

        private void Update()
        {
            if (!isRun) return;
            mProcessController.Update();
            center.Update();
        }

        public void LateUpdate()
        {
            if (!isRun) return;
            center.LaterUpdate();
        }

        private void FixedUpdate()
        {
            if (!isRun) return;
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
            ShowLogUI<MidLogUI>((ui) =>
            {
                ui.ShowContent(msg.ToString(), NotifyType.Success);
            });
        }

        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void LogError<T>(T msg)
        {
            ShowLogUI<MidLogUI>((ui) =>
            {
                ui.ShowContent(msg.ToString(), NotifyType.Error);
            });
        }

        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void LogNotify<T>(T msg)
        {
            ShowLogUI<MidLogUI>((ui) =>
            {
                ui.ShowContent(msg.ToString(), NotifyType.Notify);
            });
        }
        /// <summary>
        /// 显示LogUI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowLogUI<T>(Action<T> action = null) where T : BaseCustomLogUI, new()
        {
            curCanvas.logUIManager.ShowLogUI<T>(action);
        }

        /// <summary>
        /// 显示LogUI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideLogUI<T>() where T : BaseCustomLogUI, new()
        {
            curCanvas.logUIManager.HideLogUI<T>();
        }

        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowTipsUI<T>(Action<T> action = null) where T : BaseCustomTipsUI, new()
        {
            curCanvas.showTipsPanel.ShowTipsUI<T>(action);
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
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideTipsUI<T>(int type) where T : BaseCustomTipsUI, new()
        {
            curCanvas.showTipsPanel.HideTipsUI<T>(type);
        }
        /// <summary>
        /// 获取提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            return curCanvas.showTipsPanel.FindTipsPanel<T>();
        }

        public void CloseTopPanel() 
        {
            curCanvas.CloseTopPanel();
        }

        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowPanel<T>(Action<T> callBack = null) where T : BaseCustomPanel, new()
        {
            curCanvas.ShowPanel<T>(callBack);
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetPanel<T>() where T : BaseCustomPanel, new()
        {
            return curCanvas.FindPanel<T>();
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool IsExist<T>() where T : BaseCustomPanel, new()
        {
            return curCanvas.IsExist<T>();
        }
        ///// <summary>
        ///// 是否存在
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //public T GetCurPanel<T>() where T : BaseCustomPanel, new()
        //{
        //    return curCanvas<T>();
        //}
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
            TextPoolModule.Clear();
        }
        /// <summary>
        /// 场景加载
        /// </summary>
        /// <param name="sceneName">加载的名称</param>
        /// <param name="loadPorcess">加载的进度</param>
        public void LoadScene(SceneID sceneName, ABTagEnum tag, Action<float> loadPorcess = null)
        {
#if UNITY_EDITOR
            mSceneManager.LoadScene(sceneName.ToString(), loadPorcess);
#else
            LauncherBridge.SendLoadScene(tag.ToString(), sceneName.ToString());
#endif
        }

        /// <summary>
        /// 切换scene
        /// </summary>
        /// <param name="sceneName">加载的名称</param>
        /// <param name="loadPorcess">加载的进度</param>
        public void ChangeScene(string sceneName)
        {
            mSceneManager.ChangeScene(sceneName);
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
        public void UdpHeart(UdpSubServerType subServerType)
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
        public void AddUdpServer(UdpSubServerType subServerType, string ipAddress, int port, short heartBeatID, string name)
        {
            mUdpModule.AddUdpServer(subServerType, ipAddress, port, heartBeatID, name);
        }
        /// <summary>
        /// 移除udpServer
        /// </summary>
        /// <param name="subServerType"></param>
        public void RemoveUdpServer(UdpSubServerType subServerType)
        {
            mUdpModule.RemoveUdpServer(subServerType);
        }
        /// <summary>
        ///udp发送消息
        /// </summary>
        /// <param name="type">服务器类型</param>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        public void UdpSend(UdpSubServerType type, short udpCode, byte[] data)
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
        #region TcpManager
        public bool TcpSubServerIsContains(TcpSubServerType type) 
        {
            return mTcpModule.tcpDict.ContainsKey(type);
        }
        public void TcpSend(TcpSubServerType type, short udpCode, byte[] data)
        {
            if (mTcpModule.tcpDict.ContainsKey(type))
            {
                mTcpModule.tcpDict[type].TcpSend(udpCode, data);
            }
        }
        /// <summary>
        /// 添加Tcp服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void AddTcpServer(TcpSubServerType subServerType, string ipAddress, int port, string name)
        {
            mTcpModule.AddTcpServer(subServerType, ipAddress, port, name);
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
        /// <summary>
        /// 添加帧函数
        /// </summary>
        /// <param name="live"></param>
        public ILive AddUpdate(float freshTime, Action callBack)
        {
            return mLiveManager.AddUpdate(freshTime, callBack);
        }
        /// <summary>
        /// 移除生命周期
        /// </summary>
        /// <param name="live"></param>
        public void RemoveLife(ILive live)
        {
            if (live != null)
                mLiveManager.RemoveLive(live);
        }
        #endregion
        #region 原生之间调用
        //public string UnityToAndroid(int id, int value1, int value2, int value3, string str1, string str2, string str3)
        //{
        //    if (mBridgeManager.isRun)
        //    {
        //        return mBridgeManager.UnityToAndroid(id, value1, value2, value3, str1, str2, str3);
        //    }
        //    return null;
        //}
        #endregion
    }
}
