using Game;
using System;
using UnityEngine;
using YFramework;
using static YFramework.Core.Utility.Utility;

namespace Game
{
    /// <summary>
    /// 输入值
    /// </summary>
    public class InputValue
    {
        public float horizontal;//水平输入值
        public float vertical;//垂直输入值
    }
    public class CommonManager : BaseModule
    {
        public InputValue inputValue { get; private set; }
        private bool mIsConnectNet;//是否连接到服务器了
        private float mOnLineTimer = 3;//在线检测时间
        public CommonManager(Center center) : base(center)
        {
        }
        public override void Awake()
        {
            base.Awake();
            Init();
        }
        public override void Start()
        {
            base.Start();
            CollectionError();
        }
        public override void Update()
        {
            base.Update();
            CollectionClickInput();
            CollectionMoveInput();
            SendHeartBeat();
            ChechNetConnect();
            ChechIsOnLine();
        }
        /// <summary>
        /// 初始化值
        /// </summary>
        private void Init() {
            inputValue = new InputValue();
            mIsConnectNet = true;
        }
        /// <summary>
        /// 收集错误
        /// </summary>
        private void CollectionError()
        {
            //string[] errors = RecordTools.GetError();
            //if (!errors.IsNullOrEmpty())
            //{
            //    for (int i = 0; i < errors.Length; i++)
            //    {
            //        if (!string.IsNullOrEmpty(errors[i]))
            //        {
            //            byte[] temp = BinaryMapper.GetBytes(errors[i]);
            //            GameCenter.Instance.UdpSend((short)UdpCode.ErrorContent, temp);
            //        }
            //    }
            //}
            //RecordTools.Clear();
        }
        /// <summary>
        /// 检查鼠标输入
        /// </summary>
        private void CollectionClickInput() 
        {
            if (Input.GetMouseButtonDown(0))
            {
                BoardCastModule.Broadcast((int)BoardCastID.Click);
            }
        }
        /// <summary>
        /// 检查位移输入
        /// </summary>
        private void CollectionMoveInput() {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                inputValue.horizontal = Input.GetAxis("Horizontal");
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                inputValue.vertical = Input.GetAxis("Vertical");
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                inputValue.horizontal = 0;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                inputValue.vertical = 0;
            }
        }
        /// <summary>
        /// 发送心跳包
        /// </summary>
        private void SendHeartBeat()
        {
            //if (UnityEngine.Time.frameCount % 60 == 0)
            //{
            //    if (MyData.IsLogin)
            //    {
            //        GameCenter.Instance.UdpSend((short)UdpCode.OnLine, BitConverter.GetBytes(MyData.UserID));
            //    }
            //}
        }
        /// <summary>
        /// 检查当前连接状态
        /// </summary>
        private void ChechNetConnect() {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (mIsConnectNet)
                {
                    mIsConnectNet = false;
                    AppTools.LogError("网络连接失败");
                    GameCenter.Instance.ReConnectServer();
                    BoardCastModule.Broadcast((int)BoardCastID.LostLine);
                }
            }
            else
            {
                if (!mIsConnectNet)
                {
                    mIsConnectNet = true;
                    AppTools.Log("网络连接成功");
                    BoardCastModule.Broadcast((int)BoardCastID.ConnectLine);
                }
            }
        }
        /// <summary>
        /// 检查当前是否连接网络
        /// </summary>
        private void ChechIsOnLine()
        {
            //if (AppVarData.IsLogin)
            //{
            //    mOnLineTimer -= UnityEngine.Time.deltaTime;
            //    if (mOnLineTimer <= 0)
            //    {
            //       AppTools.LogNotify("正在尝试重新连接网络");
            //        mOnLineTimer = 3;
            //        GameCenter.Instance.ReConnectServer();
            //    }
            //}
        }
        /// <summary>
        ///  重置玩家在线状态
        /// </summary>
        public void SetPlayerIsOnLine()
        {
            mOnLineTimer = 3;
        }
    }
}
