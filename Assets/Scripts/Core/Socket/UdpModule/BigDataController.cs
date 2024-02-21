using System;
using System.Collections.Generic;
using System.Linq;
using YFramework;

namespace Game
{
    /// <summary>
    ///大数据管理器
    /// </summary>
    public class BigDataController
    {
        Dictionary<short, List<UdpBigDataItem>> mBigData; //Key为Udp事件，Value为对应的数数据
        private Dictionary<byte, byte[]> mTempDict;
        private int mReceiveMsgID = -1;
        private UdpServer mUdpManager;
        public BigDataController(UdpServer udpManager)
        {
            mUdpManager = udpManager;
            Init();
        }
        private void Init()
        {
            mTempDict = new Dictionary<byte, byte[]>();
            mBigData = new Dictionary<short, List<UdpBigDataItem>>();
            BoardCastModule.AddListener((int)BoardCastID.LostLine, LostLine);
        }
        /// <summary>
        /// 掉线
        /// </summary>
        private void LostLine()
        {
            mBigData.Clear();
        }
       
        /// <summary>
        /// 接收收到的大数据
        /// </summary>
        public void ReceiveData(short udpCode, byte[] data)
        {
            AnalysicsReceiveData(udpCode,data, ReceiveBigDataCallBack);
        }
        /// <summary>
        /// 接收到完整的大数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="udpCode"></param>
        private void ReceiveBigDataCallBack(short udpCode,byte[] data)
        {
            mUdpManager.Response(udpCode, data);
        }

        /// <summary>
        /// 解析收到的数据
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        private void AnalysicsReceiveData(short udpCode, byte[] data, Action<short,byte[]> callBack)
        {
            UdpBigDataItem bigData = YFramework.Utility.ConverterDataTools.ToObject<UdpBigDataItem>(data);
            if (bigData == null) return;
            if (!mBigData.ContainsKey(udpCode)) mBigData.Add(udpCode, new List<UdpBigDataItem>());
            List<UdpBigDataItem> bigDataList = mBigData[udpCode];
            if (bigDataList.Count > 0 && bigDataList[0].msgID < bigData.msgID)
            {
                bigDataList.Clear();
                GameCenter.Instance.RemoveBigDataItem(udpCode);
            }
            bool isContainsData = false;
            for (int i = 0; i < bigDataList.Count; i++)
            {
                if (bigDataList[i].index == bigData.index)
                {
                    isContainsData = true;
                    break;
                }
            }
            bool isReceive = false;
            if (!isContainsData)
            {
                bigDataList.Add(bigData);
                isReceive = AnalysisBigData(udpCode,bigDataList, callBack);
                if (isReceive)
                {
                    bigDataList.Clear();
                    GameCenter.Instance.ReceiveCallBack(udpCode, 0, true);
                }

                mTempDict.Clear();
                mTempDict.Add(0, BitConverter.GetBytes(AppVarData.UserID));//UserID
                mTempDict.Add(1, BitConverter.GetBytes(bigData.index));//index
                mTempDict.Add(2, isReceive .ToBytes());//IsReceive
                mTempDict.Add(3, BitConverter.GetBytes(udpCode));//UdpCode
                AppTools.UdpSend( SubServerType.Center,(short)MainUdpCode.ClientBigDataResponse,mTempDict.ToBytes());
            }
        }
        /// <summary>
        /// 解析大数据
        /// </summary>
        /// <param name="bigDatas"></param>
        /// <returns></returns>
        private bool AnalysisBigData(short udpCode,List<UdpBigDataItem> bigDatas, Action<short,byte[]> callBack)
        {
            if (bigDatas.IsNullOrEmpty()) return false;
            if (bigDatas.Count == bigDatas[0].lastIndex + 1)//这里的加1是因为下标是从0开始的
            {
                if (mReceiveMsgID == bigDatas[0].msgID) return true;
                mReceiveMsgID = bigDatas[0].msgID;
                //接收到了所有的数据
                //开始解析数据
                int dataLength = 0;
                for (int i = 0; i < bigDatas.Count; i++)
                {
                    dataLength += bigDatas[i].Data == null ? 0 : bigDatas[i].Data.Length;
                }
                bigDatas = bigDatas.OrderBy(item => item.index).ToList();
                byte[] data = new byte[dataLength];
                int index = 0;
                for (int i = 0; i < bigDatas.Count; i++)
                {
                    if (bigDatas[i].Data != null)
                    {
                        for (int j = 0; j < bigDatas[i].Data.Length; j++)
                        {
                            data[index++] = bigDatas[i].Data[j];
                        }
                    }
                }
                callBack?.Invoke(udpCode,data);
                return true;
            }
            return false;
        }
    }

}
