using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 获取分布式服务器point
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

        }

        public override void Update()
        {
            mTimer += Time.deltaTime;
            if (mTimer > mTime) 
            {
                mTimer = 0;
                AppTools.UdpSend( SubServerType.Center,(short) MainUdpCode.GetLoginServerPoint, new byte[] {(byte)SubServerType.Login });
            }
            if (IsGetData)
            {
                IsGetData = false;
                DoNext();
            }
        }
    }
}
