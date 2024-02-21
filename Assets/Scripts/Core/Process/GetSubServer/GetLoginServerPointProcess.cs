using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 获取登录服务器point
    /// </summary>
    public class GetLoginServerPointProcess : BaseProcess
    {
        public static bool IsGetData { get; set; }
        private float mTimer;
        private float mTime = 1;
        public override void Enter()
        {
            IsGetData = false;
            mTimer = 1;
        }

        public override void Exit()
        {
            AppTools.Log("登录服务器连接完成");
        }

        public override void Update()
        {
            mTimer += Time.deltaTime;
            if (mTimer > mTime) 
            {
                mTimer = 0;
                
                AppTools.UdpSend( SubServerType.Center,(short) MainUdpCode.GetBaseServerPoint, new byte[] {(byte)SubServerType.Login });
            }
            if (IsGetData)
            {
                IsGetData = false;
                DoNext();
            }
        }
    }
}
