using YFramework;
using static YFramework.Utility;
using UnityEngine;

namespace Game
{
    public class UdpMetaSchoolRequestHandle : BaseUdpRequestHandle
    {
        private MultiPlayerChildController mMultiPlayerChildController;
        private MultiPlayerChildController multiPlayerChildController 
        {
            get {
                if (mMultiPlayerChildController == null) {
                    if (GameCenter.Instance.curScene.sceneName == SceneID.MetaSchoolScene.ToString()) {
                        mMultiPlayerChildController = GameCenter.Instance.curController.GetChildController<MultiPlayerChildController>();
                    }
                }
                return mMultiPlayerChildController;
            }
        }
        
        protected override short mRequestCode => (short)UdpRequestCode.MetaSchoolServer;
        protected override void ComfigActionCode()
        {
            Add((short)MetaSchoolUdpCode.MetaSchoolHeartBeat, MetaSchoolHeartBeat);
            Add((short)MetaSchoolUdpCode.SendOtherPlayerDataToSelf, SendOtherPlayerDataToSelf);
        }

        private void SendOtherPlayerDataToSelf(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long account = data.ToLong();
            byte[] datas = ByteTools.SubBytes(data,8);
            multiPlayerChildController.SetPlayerData(account,datas);
        }

        private void MetaSchoolHeartBeat(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            GameCenter.Instance.UdpHeart(UdpSubServerType.MetaSchool);
        }
    }
}
