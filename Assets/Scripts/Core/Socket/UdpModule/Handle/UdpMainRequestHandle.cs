﻿using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class UdpMainRequestHandle : BaseUdpRequestHandle
    {
        protected override short mRequestCode => (short)UdpRequestCode.MainServer;
        protected override void ComfigActionCode()
        {
            Add((short)MainUdpCode.GetLoginServerPoint, GetBaseServerPoint);
            Add((short)MainUdpCode.MainServerHeartBeat, MainServerHeartBeat);
            Add((short)MainUdpCode.GetMetaSchoolServerPoint, GetMetaSchoolServerPoint);
        }

        private void GetMetaSchoolServerPoint(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            EndPointData endPointData = EndPointTools.GetPointData(data, 0);
            if (endPointData == null) return;
            GameCenter.Instance.AddUdpServer(SubServerType.MetaSchool,SocketVarData.MetaSchoolIPAddress, endPointData.port, (short)MetaSchoolUdpCode.MetaSchoolHeartBeat, "校园分布式服务器");
            GetMetaSchoolServerPointProcess.IsGetData = true;
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
            GameCenter.Instance.AddUdpServer(SubServerType.Login,SocketVarData.LoginIPAddress, endPointData.port, (short)LoginUdpCode.LoginHeartBeat,"登录分布式服务器");
            GetLoginServerPointProcess.IsGetData = true;
        }
    }
}
