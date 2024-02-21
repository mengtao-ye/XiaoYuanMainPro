using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class UdpMainRequestHandle : BaseUdpRequestHandle
    {
        protected override short mRequestCode => (short)UdpRequestCode.MainServer;
        protected override void ComfigActionCode()
        {
            Add((short)MainUdpCode.GetBaseServerPoint, GetBaseServerPoint);
            Add((short)MainUdpCode.MainServerHeartBeat, MainServerHeartBeat);
        }

        private void MainServerHeartBeat(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            GameCenter.Instance.UdpHeart(SubServerType.Center);
        }

        private void GetBaseServerPoint(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            EndPointData endPointData = EndPointTools.GetPointData(data, 0);
            if (endPointData == null) return;
            GameCenter.Instance.AddUdpServer(SubServerType.Login, endPointData.ipAddress, endPointData.port, (short)LoginUdpCode.LoginHeartBeat);
            GetLoginServerPointProcess.IsGetData = true;
        }
    }
}
