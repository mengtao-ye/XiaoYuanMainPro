using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class ChatPanel : BaseCustomPanel
    {
        private Text mName;
        private InputField mContentIF;
        public long friendAccount { get; private set; }
        private int mCurMsgIndex;
        private int mReadCount = 10;
        private IScrollView mScrollView;
        private Button mAddFriendBtn;
        public override void Awake()
        {
            base.Awake();
            mScrollView = transform.FindObject("ChatScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mName = transform.FindObject<Text>("Name");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(BackBtnListener);
            transform.FindObject<Button>("SetBtn").onClick.AddListener(SetBtnListener);

            mContentIF = transform.FindObject<InputField>("ContentIF");
            transform.FindObject<Button>("SendBtn").onClick.AddListener(SendBtnListener);
            mScrollView.SetUpFrashCallBack(UpFrashCallBack);
            mAddFriendBtn = transform.FindObject<Button>("AddFriendBtn");
            mAddFriendBtn.onClick.AddListener(AddFriendBtnListener);
            IsFriend(true);
        }

        private void AddFriendBtnListener()
        {
            GameCenter.Instance.ShowPanel<SendAddFriendPanel>((ui)=> 
            {
                ui.SetFriendAccount(friendAccount);
            });
        }

        public void IsFriend(bool isFriend)
        { 
            mAddFriendBtn.gameObject.SetActiveExtend(!isFriend);
        }
        /// <summary>
        /// 验证是否是好友
        /// </summary>
        /// <param name="isFriend"></param>
        /// <param name="friendAccount"></param>
        public void ConfineIsFriend(bool isFriend,long friendAccount)
        {
            if (friendAccount == this.friendAccount) 
            {
                IsFriend(isFriend);
            }
            else 
            {
                IsFriend(true);
            }
        }
        private void SetBtnListener()
        {
            GameCenter.Instance.ShowPanel<UserMainPagePanel>((ui) =>
            {
                ui.ShowContent(friendAccount);
            });
        }

        private void BackBtnListener()
        {
            GameCenter.Instance.ShowPanel<MainPanel>((ui) =>
            {
                ChatListScrollViewItem data = ui.msgSubUI.scrollView.Get(friendAccount) as ChatListScrollViewItem;
                if (data != null)
                {
                    data.unreadCount = 0;
                    ChatModule.UpdateChatListItem(data);
                    data.UpdateData();
                }
            });
            mScrollView.ClearItems();
            IsFriend(true);
        }

        private void UpFrashCallBack()
        {
            LoadChatData(friendAccount, true);
        }

        public override void Show()
        {
            base.Show();
            mScrollView.SetUpFrashState(true);
          
           
        }

        public override void Hide()
        {
            base.Hide();
            mCurMsgIndex = 0;
        }

        private void SendBtnListener()
        {
            if (mContentIF.text.IsNullOrEmpty())
            {
                AppTools.Toast("请输入内容");
                return;
            }
            SendMsgToServer((byte)ChatMsgType.Text, mContentIF.text, friendAccount);
            mContentIF.text = string.Empty;
        }
        /// <summary>
        /// 发送消息到服务器端
        /// </summary>
        private void SendMsgToServer(byte msgType, string content, long receiveAccount)
        {
            IListData<byte[]> datas = ClassPool<ListData<byte[]>>.Pop();
            datas.Add(AppVarData.Account.ToBytes());
            datas.Add(receiveAccount.ToBytes());
            datas.Add(msgType.ToBytes());
            datas.Add(content.ToBytes());
            datas.Add(DateTimeOffset.Now.ToUnixTimeSeconds().ToBytes());
            byte[] returnBytes = datas.list.ToBytes();
            datas.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SendChatMsg, returnBytes);
        }

        /// <summary>
        /// 设置聊天数据
        /// </summary>
        /// <param name="account"></param>
        public void SetChatData(long account)
        {
            friendAccount = account;
            FriendScrollViewItem friendScrollViewItem = ChatModule.GetFriendData(account);
            if (friendScrollViewItem != null)
            {
                mName.text = friendScrollViewItem.notes;
                friendScrollViewItem?.Recycle();
            }
            else
            {
                UserDataModule.MapUserData(account, mName);
            }
            mCurMsgIndex = 0;
            LoadChatData(friendAccount, false);
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), friendAccount.ToBytes());
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.IsFriend, sendBytes);
        }

        private void LoadChatData(long friendAccount, bool isLoadData)
        {
            IListData<ChatData> listData = ChatModule.LoadChatMsg(friendAccount, mCurMsgIndex, mReadCount);
            if (listData.IsNullOrEmpty())
            {
                //数据加载完了
                mScrollView.SetUpFrashState(false);
                return;
            }
            mCurMsgIndex += listData.Count;
            if (isLoadData)
            {
                for (int i = 0; i < listData.Count; i++)
                {
                    AddMsg(listData[i], false, isLoadData);
                }
            }
            else
            {
                for (int i = listData.Count - 1; i >= 0; i--)
                {
                    AddMsg(listData[i], false, isLoadData);
                }
            }

            if (listData.Count != mReadCount)
            {
                mScrollView.SetUpFrashState(false);
                //数据加载完了    
            }
            listData.Recycle();
        }


        public void AddMsg(ChatData data, bool isMySendMsg, bool isLoadData)
        {
            if (isMySendMsg)
            {
                mCurMsgIndex++;
            }
            MsgScrollViewItem msgScrollViewItem = null;
            if (data.send_userid == AppVarData.Account)
            {
                msgScrollViewItem = ClassPool<MyChatScrollViewItem>.Pop();
                msgScrollViewItem.account = data.send_userid;

            }
            else
            {
                msgScrollViewItem = ClassPool<FriendChatScrollViewItem>.Pop();
                msgScrollViewItem.account = data.receive_userid;
            }
            msgScrollViewItem.id = data.id;
            msgScrollViewItem.msg_type = data.msg_type;
            msgScrollViewItem.chat_msg = data.chat_msg;
            msgScrollViewItem.time = data.time;
            if (isLoadData)
            {
                mScrollView.Insert(msgScrollViewItem, 0);
            }
            else
            {
                mScrollView.Add(msgScrollViewItem);

            }
        }
    }
}
