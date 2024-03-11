using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FriendListPanel : BaseCustomPanel
    {
        private Transform mContent;
        private byte[] mSendBytes;
        private ILive mLive;
        public FriendListPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mContent = transform.FindObject<Transform>("Content");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            transform.FindObject<Button>("SetBtn").onClick.AddListener(() => { });
        }

        public override void Show()
        {
            base.Show();
            ChatModule.LoadFriendList(mContent);
            if (mLive == null || !mLive.isPop)
            {
                mLive = GameCenter.Instance.AddUpdate(1f, GetFriendUpdate);
            }
        }
        public void GetFriendUpdate()
        {
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), ChatModule.GetLastFirendListID().ToBytes());
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetFriendList, mSendBytes);
        }
        public override void OnDestory()
        {
            base.OnDestory();
            RemoveGetFriendLife();
        }
        /// <summary>
        /// 移除获取好友列表请求
        /// </summary>
        public void RemoveGetFriendLife()
        {
            GameCenter.Instance.RemoveLife(mLive);
        }

        /// <summary>
        /// 设置好友列表数据
        /// </summary>
        public void SetData(IListData<FriendPairData> listData)
        {
            ChatModule.SetFriendListData(listData,mContent);
        }
    }
}
