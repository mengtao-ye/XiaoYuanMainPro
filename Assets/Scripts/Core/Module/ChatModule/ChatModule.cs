using System.IO;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class ChatModule
    {

        #region Chat
        /// <summary>
        /// 加载聊天数据
        /// </summary>
        /// <param name="friendAccount"></param>
        /// <param name="msgIndex"></param>
        /// <param name="readCount"></param>
        /// <returns></returns>
        public static IListData<ChatData> LoadChatMsg(long friendAccount, int msgIndex, int readCount)
        {
            string dir = ChatPathData.ChatMsgDetailDir(friendAccount);
            string configPath = dir + "/Config.txt";
            if (!File.Exists(configPath)) return null;
            byte[] configBytes = File.ReadAllBytes(configPath);
            int startIndex = msgIndex * 8;
            IListData<ChatData> listData = null;
            for (int i = 0; i < readCount; i++)
            {
                int tempStartIndex = configBytes.Length - (startIndex + 8);
                if (tempStartIndex < 0)
                {
                    return listData;
                }
                byte[] chatIDBytes = ByteTools.SubBytes(configBytes, tempStartIndex, 8);
                if (chatIDBytes == null)
                {
                    return listData;
                }
                if (listData == null)
                {
                    listData = ClassPool<ListPoolData<ChatData>>.Pop();
                }
                string chatDataPath = dir + "/" + chatIDBytes.ToLong() + ".txt";
                if (File.Exists(chatDataPath))
                {
                    byte[] chatDataBytes = File.ReadAllBytes(chatDataPath);
                    ChatData chatData = ConverterDataTools.ToPoolObject<ChatData>(chatDataBytes);
                    if (chatData == null)
                    {
                        return listData;
                    }
                    listData.Add(chatData);
                    startIndex += 8;
                }
                else
                {
                    return listData;
                }
            }
            return listData;
        }

        /// <summary>
        /// 保存聊天记录到本地
        /// </summary>
        /// <param name="account"></param>
        /// <param name="chatData"></param>
        public static void SaveChatMsgToLocal(long friendID, ChatData chatData)
        {
            string dir = ChatPathData.ChatMsgDetailDir(friendID);
            FileTools.Write(dir + "/" + chatData.id + ".txt", chatData.ToBytes());
            SaveChatMsgConfigToLocal(dir + "/Config.txt", chatData.id);
        }
        #endregion
        #region ChatList
        private static long mLastMsgID = -1;
        
        /// <summary>
        /// 保存聊天记录配置表到本地
        /// </summary>
        private static void SaveChatMsgConfigToLocal(string filePath, long msgID)
        {
            if (!File.Exists(filePath))
            {
                FileTools.CreateFile(filePath);
            }
            FileTools.InsertToTailSubBytes(filePath, msgID.ToBytes());
        }
        /// <summary>
        /// 获取聊天列表数据
        /// </summary>
        /// <returns></returns>
        public static void LoadChatList(IScrollView scrollView)
        {
            string path = ChatPathData.ChatListDir();
            if (!Directory.Exists(path)) return;
            string[] files = Directory.GetFiles(ChatPathData.ChatListDir());
            if (files.IsNullOrEmpty()) return;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                ChatListScrollViewItem chatListItemData = ConverterDataTools.ToPoolObject<ChatListScrollViewItem>(chatBytes);
                chatListItemData.ViewItemID = chatListItemData.account;
                scrollView.Add(chatListItemData);
            }
        }
        /// <summary>
        /// 获取最近的聊天ID
        /// </summary>
        /// <returns></returns>
        public static long GetLastChatID()
        {
            if (mLastMsgID == -1)
            {
                if (File.Exists(ChatPathData.ChatListIDPath))
                {
                    mLastMsgID = File.ReadAllBytes(ChatPathData.ChatListIDPath).ToLong();
                }
                else
                {
                    mLastMsgID = 0;
                }
            }
            return mLastMsgID;
        }
        /// <summary>
        /// 设置聊天数据
        /// </summary>
        /// <param name="chatDatas"></param>
        /// <param name="parent"></param>
        public static void SetChatData(IListData<ChatData> chatDatas, IScrollView scrollView)
        {
            if (chatDatas.IsNullOrEmpty()) return;
            SetLastMsgID(chatDatas[chatDatas.Count - 1].id);
            for (int i = 0; i < chatDatas.list.Count; i++)
            {
                long friendAccount = chatDatas.list[i].send_userid == AppVarData.Account ? chatDatas.list[i].receive_userid : chatDatas.list[i].send_userid;
                SaveChatMsgToLocal(friendAccount, chatDatas.list[i]);
                if (scrollView.Contains(chatDatas.list[i].send_userid))
                {
                    ChatListScrollViewItem chatListItem = scrollView.Get(chatDatas.list[i].send_userid) as ChatListScrollViewItem;
                    ChatData chatData = chatDatas.list[i];
                    chatListItem.topMsg = chatData.chat_msg;
                    chatListItem.msgType = chatData.msg_type;
                    chatListItem.time = chatData.msg_type;
                    chatListItem.unreadCount++;
                    SaveChatListToLocal(chatListItem);
                    scrollView.Insert(chatListItem,0);
                }
                else
                {
                    ChatListScrollViewItem chatListItemData = ClassPool<ChatListScrollViewItem>.Pop();
                    ChatData chatData =  chatDatas.list[i];
                    chatListItemData.msgType = chatData.msg_type;
                    chatListItemData.topMsg = chatData.chat_msg;
                    chatListItemData.account = chatData.send_userid;
                    chatListItemData.time = chatData.time;
                    chatListItemData.unreadCount++;
                    chatListItemData.ViewItemID = chatData.send_userid;
                    SaveChatListToLocal(chatListItemData);
                    scrollView.Insert(chatListItemData, 0);
                }
            }
        }
        /// <summary>
        /// 记录最后一个聊天ID
        /// </summary>
        /// <param name="msgID"></param>
        public static void SetLastMsgID(long msgID)
        {
            if (msgID == mLastMsgID) return;
            mLastMsgID = msgID;
            FileTools.Write(ChatPathData.ChatListIDPath, msgID.ToBytes());
        }
        /// <summary>
        /// 将聊天列表对象写入本地
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void SaveChatListToLocal(ChatListScrollViewItem chatListItemData)
        {
            string path = ChatPathData.ChatListDir() + "/" + chatListItemData.account + ".txt";
            FileTools.Write(path, chatListItemData.ToBytes());
        }
        /// <summary>
        /// 更新聊天列表数据
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void UpdateChatListItem(ChatListScrollViewItem chatListItemData)
        {
            if (chatListItemData == null) return;
            SaveChatListToLocal(chatListItemData);
        }
        #endregion
        #region NewFriend
        private static int mLastAddFriendID = -1;
        /// <summary>
        /// 加载好友申请数据
        /// </summary>
        /// <returns></returns>
        public static void LoadAddFriendList(IScrollView scrollView)
        {
            if (!Directory.Exists(ChatPathData.AddFriendListDir())) return;
            string[] files = Directory.GetFiles(ChatPathData.AddFriendListDir());
            if (files.IsNullOrEmpty()) return;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                NewFriendScrollViewItem friendPairData = ConverterDataTools.ToPoolObject<NewFriendScrollViewItem>(chatBytes);
                if (!scrollView.Contains(friendPairData.friendAccount))
                {
                    friendPairData.ViewItemID = friendPairData.friendAccount;
                    scrollView.Add(friendPairData);
                }
                else
                {
                    friendPairData.Recycle();
                }
            }
        }
        /// <summary>
        /// 获取最近的添加好友ID
        /// </summary>
        /// <returns></returns>
        public static long GetLastAddFriendID()
        {
            if (mLastAddFriendID == -1)
            {
                if (File.Exists(ChatPathData.AddFriendIDPath))
                {
                    mLastAddFriendID = File.ReadAllBytes(ChatPathData.AddFriendIDPath).ToInt();
                }
                else
                {
                    mLastAddFriendID = 0;
                }
            }
            return mLastAddFriendID;
        }
        /// <summary>
        /// 设置好友申请列表数据
        /// </summary>
        /// <param name="addFriendList"></param>
        /// <param name="parent"></param>
        public static void SetAddFriendListData(IListData<NewFriendScrollViewItem> addFriendList)
        {
            if (addFriendList.IsNullOrEmpty()) return;
            SetLastAddFriendID(addFriendList[addFriendList.Count - 1].id);
            for (int i = 0; i < addFriendList.list.Count; i++)
            {
                SaveAddFriendRequestToLocal(addFriendList.list[i]);
            }
        }
        /// <summary>
        /// 记录最后一个好友列表聊天ID
        /// </summary>
        /// <param name="msgID"></param>
        public static void SetLastAddFriendID(int addFriendLastID)
        {
            if (addFriendLastID == mLastAddFriendID) return;
            mLastAddFriendID = addFriendLastID;
            FileTools.Write(ChatPathData.AddFriendIDPath, addFriendLastID.ToBytes());
        }
        /// <summary>
        /// 将聊天列表对象写入本地
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void SaveAddFriendRequestToLocal(NewFriendScrollViewItem data)
        {
            string path = ChatPathData.AddFriendListDir() + "/" + data.friendAccount + ".txt";
            FileTools.Write(path, data.ToBytes());
        }
        /// <summary>
        /// 移除本地添加好友请求数据
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void RemoveLocalAddFriendRequest(long account)
        {
            string path = ChatPathData.AddFriendListDir() + "/" + account + ".txt";
            FileTools.ForceDelete(path);
        }

        /// <summary>
        /// 设置好友申请状态
        /// </summary>
        public static void SetAddFriendState(long account)
        {
            RemoveLocalAddFriendRequest(account);
            GameCenter.Instance.GetPanel<AddFriendRequestViewPanel>().scrollView.Delete(account); ;
        }

        #endregion
        #region Friend
        private static int mLastFriendID = -1;
        private static FriendGroupData[] mFriendListGroupList = new FriendGroupData[PinYinConstData.MAX];
        /// <summary>
        /// 加载好友数据
        /// </summary>
        /// <returns></returns>
        public static void LoadFriendList(IScrollView scrollView)
        {
            if (!Directory.Exists(ChatPathData.FriendListDir())) return;
            string[] files = Directory.GetFiles(ChatPathData.FriendListDir());
            if (files.IsNullOrEmpty()) return;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                FriendScrollViewItem friendPairData = ConverterDataTools.ToPoolObject<FriendScrollViewItem>(chatBytes);
                if (!scrollView.Contains(friendPairData.friendAccount))
                {
                    char showName = default(char);
                    if (friendPairData.notes.IsNullOrEmpty())
                    {
                        showName = PinYinConstData.DEFAULT;
                    }
                    else
                    {
                        showName = friendPairData.notes[0];
                    }
                    PinYinTools.GetPinYin(showName, (ch) =>
                    {
                        int pinYinCode = PinYinTools.YinPinCodeToIndex(ch);
                        FriendGroupData friendGroupData = mFriendListGroupList[pinYinCode];
                        if (friendGroupData == null)
                        {
                            friendGroupData = new FriendGroupData();
                        }
                        friendGroupData.count++;
                        int index = GetIndex(pinYinCode);
                        friendPairData.ViewItemID = friendPairData.friendAccount;
                        scrollView.Insert(friendPairData, index);
                    });
                }
                else 
                {
                    friendPairData.Recycle();
                }
            }
        }
        private static int GetIndex(int codeIndex)
        {
            int index = 0;
            for (int i = 0; i < codeIndex; i++)
            {
                if (mFriendListGroupList[i] != null)
                {
                    index += mFriendListGroupList[i].count;
                }
            }
            return index;
        }
        /// <summary>
        /// 获取最近好友列表ID
        /// </summary>
        /// <returns></returns>
        public static int GetLastFirendListID()
        {
            if (mLastFriendID == -1)
            {
                if (File.Exists(ChatPathData.FriendListIDPath))
                {
                    mLastFriendID = File.ReadAllBytes(ChatPathData.FriendListIDPath).ToInt();
                }
                else
                {
                    mLastFriendID = 0;
                }
            }
            return mLastFriendID;
        }
        /// <summary>
        /// 设置好友列表数据
        /// </summary>
        /// <param name="friendList"></param>
        /// <param name="parent"></param>
        public static void SetFriendListData(IListData<FriendScrollViewItem> friendList)
        {
            if (friendList.IsNullOrEmpty()) return;
            SetLastFriendID(friendList[friendList.Count - 1].id);
            for (int i = 0; i < friendList.list.Count; i++)
            {
                SaveFriendListToLocal(friendList.list[i]);
            }
        }
        /// <summary>
        /// 记录最后一个好友列表聊天ID
        /// </summary>
        /// <param name="msgID"></param>
        public static void SetLastFriendID(int friendID)
        {
            if (friendID == mLastFriendID) return;
            mLastFriendID = friendID;
            FileTools.Write(ChatPathData.FriendListIDPath, friendID.ToBytes());
        }
        /// <summary>
        /// 将聊天列表对象写入本地
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void SaveFriendListToLocal(FriendScrollViewItem friendPair)
        {
            string path = ChatPathData.FriendListDir() + "/" + friendPair.friendAccount + ".txt";
            FileTools.Write(path, friendPair.ToBytes());
        }
        #endregion

    }
}
