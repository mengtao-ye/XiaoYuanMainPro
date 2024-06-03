using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MsgPageSubUI : BaseCustomSubUI
    {
        private MainPanel mMainPanel;
        private RectTransform mNofityBtnRect;
        private ILive mGetFriendListLive;
        private byte[] mSendGetFriendListBytes;
        public IScrollView scrollView;
        public MsgPageSubUI(Transform trans, MainPanel mainPanel) : base(trans)
        {
            mMainPanel = mainPanel;
        }
        public override void Awake()
        {
            base.Awake();
            scrollView = transform.FindObject("MsgScrollView").AddComponent<RecyclePoolScrollView>();
            scrollView.Init();
            scrollView.SetSpace(10,10,10);
            transform.FindObject<Button>("FriendBtn").onClick.AddListener(FriendBtnListener);
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(SearchBtnListener);
            mNofityBtnRect = transform.FindObject<RectTransform>("NotifyBtn");
            mNofityBtnRect.GetComponent<Button>().onClick.AddListener(ClickNotifyTipUIListener);
        }

        public override void FirstShow()
        {
            base.FirstShow();
            ChatModule.LoadChatList(scrollView);
        }

        public override void Show()
        {
            base.Show();
            if (mGetFriendListLive == null || !mGetFriendListLive.isPop)
            {
                mGetFriendListLive = GameCenter.Instance.AddUpdate(1f, GetFriendUpdate);
            }
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
            GameCenter.Instance.RemoveLife(mGetFriendListLive);
        }

        public void GetFriendUpdate()
        {
            mSendGetFriendListBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), ChatModule.GetLastFirendListID().ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetFriendList, mSendGetFriendListBytes);
        }

        /// <summary>
        /// 设置好友列表数据
        /// </summary>
        public void SetFriendData(IListData<FriendScrollViewItem> listData)
        {
            ChatModule.SetFriendListData(listData);
        }

        private void ClickNotifyTipUIListener()
        {
            GameCenter.Instance.ShowTipsUI<NotifyTipUI>((ui)=>
            {
                ui.SetPos(mNofityBtnRect.position);
            });
        }
        private void FriendBtnListener()
        {
            GameCenter.Instance.ShowPanel<FriendListPanel>();
        }
        private void SearchBtnListener()
        {

        }
        public void SetMsgData(IListData<ChatData> chatData)
        {
            if (chatData.IsNullOrEmpty()) return;
            ChatModule.SetChatData(chatData,scrollView);
            chatData.Recycle();
        }

        public void DeleteChatListItem(long account) 
        {
            scrollView.Delete(account);
            ChatModule.DeleteLocalChatList(account);
            ChatModule.DeleteLocalChatMsg(account);
        }
    }
}
