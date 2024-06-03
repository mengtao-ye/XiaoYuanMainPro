using YFramework;
using static YFramework.Utility;
using UnityEngine;

namespace Game
{
    public class UdpLoginRequestHandle : BaseUdpRequestHandle
    {
        protected override short mRequestCode => (short)UdpRequestCode.LoginServer;
        protected override void ComfigActionCode()
        {
            
        }
    }
}
