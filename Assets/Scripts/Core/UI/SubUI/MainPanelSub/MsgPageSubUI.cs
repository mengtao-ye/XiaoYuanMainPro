using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MsgPageSubUI : BaseCustomSubUI
    {
        private MainPanel mMainPanel;
        private Transform mContent;
        private float mTimer;
        private float mTime = 1;
        private byte[] mGetMsgBytes;
        private RectTransform mNofityBtnRect;
        public MsgPageSubUI(Transform trans, MainPanel mainPanel) : base(trans)
        {
            mMainPanel = mainPanel;
        }
        public override void Awake()
        {
            base.Awake();
            mContent = transform.FindObject<Transform>("Content");
            transform.FindObject<Button>("FriendBtn").onClick.AddListener(FriendBtnListener);
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(SearchBtnListener);
            mNofityBtnRect = transform.FindObject<RectTransform>("NotifyBtn");
            mNofityBtnRect.GetComponent<Button>().onClick.AddListener(ClickNotifyTipUIListener);
        }

        private void ClickNotifyTipUIListener()
        {
            GameCenter.Instance.ShowTipsUI<NotifyTipUI>((ui)=>
            {
                ui.SetPos(mNofityBtnRect.position);
            });
        }

        public override void Update()
        {
            base.Update();
            mTimer += Time.deltaTime;
            if (mTimer > mTime)
            {
                mTimer = 0;
                mGetMsgBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), ChatModule.GetLastChatID().ToBytes());
                AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetNewChatMsg, mGetMsgBytes); ;
            }
        }

        public override void FirstShow()
        {
            base.FirstShow();
            ChatModule.LoadChatList(mContent);
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
            ChatModule.SetChatData(chatData,mContent);
            chatData.Recycle();
        }
    }
}
