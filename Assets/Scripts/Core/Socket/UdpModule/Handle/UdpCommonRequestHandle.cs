using System;
using YFramework;

namespace Game
{
    public class UdpCommonRequestHandle : BaseUdpRequestHandle
    {
        protected override short mRequestCode => (short)UdpRequestCode.Common;
        protected override void ComfigActionCode()
        {
            Add((short)UdpCode.ServerBigDataResponse, ServerBigDataResponse);
        }
        /// <summary>
        /// 接收到大数据下标
        /// </summary>
        /// <param name="data"></param>
        private void ServerBigDataResponse(byte[] data)
        {
            if (data == null || data.Length != 3) return;
            ushort index = BitConverter.ToUInt16(data, 0);
            bool isReceive = data[2] == 1;
            short udpCode = BitConverter.ToInt16(data, 3);
            GameCenter.Instance.ReceiveCallBack(udpCode, index, isReceive);
        }
    }

}