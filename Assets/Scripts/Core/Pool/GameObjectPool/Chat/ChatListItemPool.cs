using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ChatListItemPool : BaseGameObjectPoolTarget<ChatListItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Chat/ChatMsgItem";
        public override bool isUI { get; } = true;
        private Image mIcon;
        private Text mName;
        private Text mTopMsg;
        private Text mTime;
        private long mFriendAccount;//好友账号
        private GameObject mUnreadBG;
        private Text mUnreadCountText;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mUnreadBG = transform.FindObject("UnreadBG");
            mUnreadBG.SetAvtiveExtend(false);
            mUnreadCountText = transform.FindObject<Text>("UnreadCountText");
            mIcon = target.transform.FindObject<Image>("Head");
            mName =  target.transform.FindObject<Text>("Name");
            mTopMsg = target.transform.FindObject<Text>("TopMsg");
            mTime = target.transform.FindObject<Text>("Time");
            target.GetComponent<Button>().onClick.AddListener(ClickBtnListener);
        }
        private void ClickBtnListener()
        {
            ClearUnreadMsg();
            ChatListScrollViewItem data = GameCenter.Instance.GetPanel<MainPanel>().msgSubUI.scrollView.Get(ID);
            if (data != null) 
            {
                data.unreadCount = 0;
                ChatModule.UpdateChatListItem(data);
            }
            if (mFriendAccount == UserAccountConstData.NEW_FRIEND_ACCOUNT)
            {
                GameCenter.Instance.ShowPanel<AddFriendRequestViewPanel>();
                return;    
            }
            GameCenter.Instance.ShowPanel<ChatPanel>((item)=> { item.SetChatData(mFriendAccount); });
        }

        public void SetFriendAccount(long friendAccount ) 
        {
            mFriendAccount = friendAccount;
            UserDataModule.MapUserData(friendAccount,mIcon, mName);
        }

        public void SetTopTime(long time)
        {
            mTime.text = DateTimeTools.UnixTimeToShowTimeStr(time);
        }

        public void SetTopMsg(byte msgType,string topMsg)
        {
            ChatMsgType chatMsgType = (ChatMsgType)msgType;
            switch (chatMsgType)
            {
                case ChatMsgType.Text:
                    mTopMsg.text = topMsg;
                    break;
                case ChatMsgType.Audio:
                    mTopMsg.text = "[语音]";
                    break;
                case ChatMsgType.Image:
                    mTopMsg.text = "[图片]";
                    break;
                case ChatMsgType.NewFriend:
                    mTopMsg.text = "点击查看新好友";
                    break;
            }
        }
        /// <summary>
        /// 设置未读数量
        /// </summary>
        /// <param name="count"></param>
        public void SetUnreadCount(int count)
        {
            if (count != 0) 
            {
                mUnreadBG.SetAvtiveExtend(true);
                string countText = null;
                if (count > 99)
                {
                    countText = "99+";
                }
                else
                {
                    countText = count.ToString();
                }
                mUnreadCountText.text = countText;
            }
        }
        /// <summary>
        /// 清除未读消息
        /// </summary>
        public void ClearUnreadMsg() 
        {
            mUnreadBG.SetAvtiveExtend(false);
        }
        /// <summary>
        /// 回收
        /// </summary>
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
