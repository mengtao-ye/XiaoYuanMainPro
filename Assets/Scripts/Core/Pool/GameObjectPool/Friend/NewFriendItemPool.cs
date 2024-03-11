using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class NewFriendItemPool : BaseGameObjectPoolTarget<NewFriendItemPool>
    {
        public override int Type => (int)GameObjectPoolID.NewFriendItemPool;
        public override string assetPath => "Prefabs/UI/Item/Chat/NewFriendItem";
        private Image mHead;
        private Text mName;
        private Text mContent;
        private long mFriendAccount = 0;
        private byte[] mSendBytes = null;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mContent = transform.FindObject<Text>("Content");
            transform.FindObject<Button>("ConfineBtn").onClick.AddListener(ClickListener);
            transform.FindObject<Button>("RefuseBtn").onClick.AddListener(RefuseBtnListener);
        }

        private void RefuseBtnListener() 
        {
            if (mFriendAccount == 0)
            {
                AppTools.ToastError("用户信息异常");
                return;
            }
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(),mFriendAccount.ToBytes());
            AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.RefuseFriend, mSendBytes);
        }

        private void ClickListener()
        {
            if (mFriendAccount == 0)
            {
                AppTools.ToastError("用户信息异常");
                return;
            }
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mFriendAccount.ToBytes());
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.ConfineFriend, mSendBytes);
        }
        public void SetNewFriendData(long account,string content)
        {
            mFriendAccount = account;
            mContent.text = content;
            UserDataModule.MapUserData(account, mHead, mName);
        }
    }
}
