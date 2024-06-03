using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 获取分布式服务器point
    /// </summary>
    public class GetMetaSchoolServerPointProcess : BaseProcess
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
                byte[] sendBytes = ByteTools.Concat((byte)UdpSubServerType.MetaSchool,SchoolGlobalVarData.SchoolCode.ToBytes());
                AppTools.UdpSend( UdpSubServerType.Center,(short) MainUdpCode.GetMetaSchoolServerPoint, sendBytes);
            }
            if (IsGetData)
            {
                IsGetData = false;
                DoNext();
            }
        }
    }
}
