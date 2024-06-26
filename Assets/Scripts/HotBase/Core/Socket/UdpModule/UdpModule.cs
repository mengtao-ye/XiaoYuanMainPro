﻿using System.Collections.Generic;
using YFramework;

namespace Game
{
    public class UdpModule : BaseModule
    {
        private Dictionary<UdpSubServerType, XiaoYuanUDPScoketManager> mUdpDict;
        private List<XiaoYuanUDPScoketManager> mUdpList;
        public Dictionary<UdpSubServerType, XiaoYuanUDPScoketManager> udpDict { get { return mUdpDict; } }
        private UdpHandleMapper mMapper;
        public XiaoYuanUDPScoketManager centerUdpServer { get; private set; }
        public UdpModule(Center center) : base(center)
        {

        }
        public override void Awake()
        {
            base.Awake();
            isRun = true;
            mUdpList = new List<XiaoYuanUDPScoketManager>();
            mUdpDict = new Dictionary<UdpSubServerType, XiaoYuanUDPScoketManager>();
            mMapper = new UdpHandleMapper();
            centerUdpServer =  AddUdpServer( UdpSubServerType.Center, SocketVarData.IPAddress ,SocketVarData.UdpServerPort, (short)MainUdpCode.MainServerHeartBeat,"Udp主服务器");
        }

        /// <summary>
        /// 添加分布式服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public XiaoYuanUDPScoketManager AddUdpServer(UdpSubServerType subServerType, string ipAddress, int port, short heartBeatID,string name)
        {
            if (mUdpDict.ContainsKey(subServerType))
            {
                mUdpList.Remove(mUdpDict[subServerType]);
                mUdpDict.Remove(subServerType);
            }
            XiaoYuanUDPScoketManager socket = null;
            switch (subServerType)
            {
                case UdpSubServerType.Center:
                    socket = new XiaoYuanUDPScoketManager(center, mMapper, subServerType, name);
                    break;
                case UdpSubServerType.Login:
                    socket = new XiaoYuanLoginUDPSocketManager(center, mMapper, subServerType, name);
                    break;
                case UdpSubServerType.MetaSchool:
                    socket = new XiaoYuanMetaSchoolUDPSocketManager(center, mMapper, subServerType, name);
                    break;
            }
            socket.Launcher(ipAddress,port, heartBeatID);
            mUdpDict.Add(subServerType, socket);
            mUdpList.Add(socket);
            return socket;
        }
        /// <summary>
        /// 移除udpserver
        /// </summary>
        /// <param name="subServerType"></param>
        public void RemoveUdpServer(UdpSubServerType subServerType)
        {
            if (mUdpDict.ContainsKey(subServerType)) 
            {
                mUdpDict.Remove(subServerType);
            }
            for (int i = 0; i < mUdpList.Count; i++)
            {
                if (mUdpList[i].subServerType == subServerType)
                {
                    mUdpList.RemoveAt(i);
                    break;
                }
            }
        }

        public override void Update()
        {
            for (int i = 0; i < mUdpDict.Count; i++)
            {
                mUdpList[i].Update();
            }
        }
        public bool IsConnect(UdpSubServerType type)
        {
            if (!udpDict.ContainsKey(type))
            {
                return false;
            }
            return udpDict[type].IsConnect;
        }
        /// <summary>
        /// 接收到心跳包
        /// </summary>
        /// <param name="type"></param>
        public void Heart(UdpSubServerType type) 
        {
            if (udpDict.ContainsKey(type))
            {
                udpDict[type].Heart();
            }
        }

    }
}
