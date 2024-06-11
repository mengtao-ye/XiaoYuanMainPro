using System.IO;
using UnityEngine;
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
        /// 删除本地聊天信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="chatData"></param>
        public static void DeleteLocalChatMsg(long friendID)
        {
            string dir = ChatPathData.ChatMsgDetailDir(friendID);
            DirectoryTools.ForceDelete(dir);
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
        #endregion
        #region ChatList
        private static long mLastMsgID = -1;


        /// <summary>
        /// 获取聊天列表数据
        /// </summary>
        /// <returns></returns>
        public static bool LoadChatList(IScrollView scrollView)
        {
            string path = ChatPathData.ChatListDir();
            if (!Directory.Exists(path)) return false;
            string[] files = Directory.GetFiles(ChatPathData.ChatListDir());
            if (files.IsNullOrEmpty()) return false;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                ChatListScrollViewItem chatListItemData = ConverterDataTools.ToPoolObject<ChatListScrollViewItem>(chatBytes);
                chatListItemData.ViewItemID = chatListItemData.account;
                scrollView.Add(chatListItemData);
            }
            return true;
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
            long curChatPanelAccount = 0;
            ChatPanel chatPanel = null;
            if (GameCenter.Instance.curCanvas.curPanel != null && GameCenter.Instance.curCanvas.curPanel.uiName == "ChatPanel")
            {
                chatPanel = GameCenter.Instance.GetPanel<ChatPanel>();
                curChatPanelAccount = chatPanel.friendAccount;
            }
            for (int i = 0; i < chatDatas.list.Count; i++)
            {
                long msgTargetAccount = chatDatas.list[i].send_userid == AppVarData.Account ? chatDatas.list[i].receive_userid : chatDatas.list[i].send_userid;
                SaveChatMsgToLocal(msgTargetAccount, chatDatas.list[i]);
                if (curChatPanelAccount != 0 && curChatPanelAccount == msgTargetAccount)
                {
                    bool isMySendMsg = chatDatas.list[i].send_userid == AppVarData.Account;
                    chatPanel.AddMsg(chatDatas.list[i], isMySendMsg, false);
                }
                if (scrollView.Contains(msgTargetAccount))
                {
                    ChatListScrollViewItem chatListItem = scrollView.Get(msgTargetAccount) as ChatListScrollViewItem;
                    ChatData chatData = chatDatas.list[i];
                    chatListItem.topMsg = chatData.chat_msg;
                    chatListItem.msgType = chatData.msg_type;
                    chatListItem.time = chatData.time;
                    chatListItem.unreadCount++;
                    SaveChatListToLocal(chatListItem);
                    chatListItem.UpdateData();
                }
                else
                {
                    ChatListScrollViewItem chatListItemData = ClassPool<ChatListScrollViewItem>.Pop();
                    ChatData chatData = chatDatas.list[i];
                    chatListItemData.msgType = chatData.msg_type;
                    chatListItemData.topMsg = chatData.chat_msg;
                    chatListItemData.account = msgTargetAccount;
                    chatListItemData.time = chatData.time;
                    chatListItemData.unreadCount++;
                    chatListItemData.ViewItemID = msgTargetAccount;
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
        /// 删除本地聊天列表记录
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void DeleteLocalChatList(long account)
        {
            string path = ChatPathData.ChatListDir() + "/" + account + ".txt";
            if (File.Exists(path)) 
            {
                File.Delete(path);
            }
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
        public static bool LoadAddFriendList(IScrollView scrollView)
        {
            if (!Directory.Exists(ChatPathData.AddFriendListDir())) return false;
            string[] files = Directory.GetFiles(ChatPathData.AddFriendListDir());
            if (files.IsNullOrEmpty()) return false;
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
            return true;
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
        private static FriendGroupData[] mFriendListGroupList = new FriendGroupData[PinYinConstData.LEN];
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="account"></param>
        public static void DeleteFriend(long account)
        {
            if (Directory.Exists(ChatPathData.FriendListDir()))
            {
                string path = ChatPathData.FriendListDir() + "/" + account + ".txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        /// <summary>
        /// 加载好友数据
        /// </summary>
        /// <returns></returns>
        public static bool LoadFriendList(IScrollView scrollView)
        {
            if (!Directory.Exists(ChatPathData.FriendListDir())) return false;
            string[] files = Directory.GetFiles(ChatPathData.FriendListDir());
            if (files.IsNullOrEmpty()) return false;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                FriendScrollViewItem friendPairData = ConverterDataTools.ToPoolObject<FriendScrollViewItem>(chatBytes);
                friendPairData.ViewItemID = friendPairData.friendAccount;
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
                            friendPairData.pinYinChar = ch;
                            scrollView.Insert(friendPairData, index);
                        });
                }
                else
                {
                    friendPairData.Recycle();
                }
            }
            return true;
        }
        /// <summary>
        /// 获取好友数据
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static FriendScrollViewItem GetFriendData(long account)
        {
            if (Directory.Exists(ChatPathData.FriendListDir())) {
                string path = ChatPathData.FriendListDir() + "/" + account+".txt";
                if (File.Exists(path))
                {
                    byte[] data = File.ReadAllBytes(path);
                    FriendScrollViewItem friendPairData = ConverterDataTools.ToPoolObject<FriendScrollViewItem>(data);
                    return friendPairData;
                }
            }
            return null;
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

        /// <summary>
        /// 修改好友备注倒本地
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static bool ChangeFriendNotesToLocal(long friendAccount,string notes)
        {
            FriendScrollViewItem friendScrollViewItem = GetFriendData(friendAccount);
            if (friendScrollViewItem == null) return false;
            friendScrollViewItem.notes = notes;
            SaveFriendListToLocal(friendScrollViewItem);
            friendScrollViewItem.Recycle();
            return true;
        }

        #endregion
    }
}
