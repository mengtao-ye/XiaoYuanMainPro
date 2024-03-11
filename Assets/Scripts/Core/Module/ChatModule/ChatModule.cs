using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class ChatModule
    {
        #region Chat
        private static long mLastMsgID = -1;
        /// <summary>
        /// 聊天列表
        /// </summary>
        public static Dictionary<long, ChatListItemData> chatListItemDict = new Dictionary<long, ChatListItemData>();
        /// <summary>
        /// 聊天列表
        /// </summary>
        public static List<ChatListItemData> chatListItemList = new List<ChatListItemData>();

        /// <summary>
        /// 保存聊天记录到本地
        /// </summary>
        /// <param name="account"></param>
        /// <param name="chatData"></param>
        public static void SaveChatMsgToLocal(long account, ChatData chatData)
        {
            long friendID = chatData.send_userid == account ? chatData.receive_userid : chatData.send_userid;
            string dir = ChatPathData.ChatMsgDetailDir(account, friendID);
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
        /// <summary>
        /// 获取聊天列表数据
        /// </summary>
        /// <returns></returns>
        public static void LoadChatList(Transform parent)
        {
            ClearChatListItem();
            if (!Directory.Exists(ChatPathData.ChatListDir())) return;
            string[] files = Directory.GetFiles(ChatPathData.ChatListDir());
            if (files.IsNullOrEmpty()) return;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                ChatListItemData chatListItemData = ConverterDataTools.ToObject<ChatListItemData>(chatBytes);
                GameObjectPoolModule.AsyncPop<ChatMsgItemPool>((int)GameObjectPoolID.ChatMsgItem, parent, (item) =>
                {
                    item.SetFriendAccount(chatListItemData.account);
                    item.SetTopMsg(chatListItemData.msgType, chatListItemData.topMsg);
                    item.SetTopTime(chatListItemData.time);
                    item.SetUnreadCount(chatListItemData.unreadCount);
                    chatListItemData.poolItem = item;
                    item.data = chatListItemData;
                    int index = GetMsgListIndex(chatListItemData.time);
                    AddChatListItem(chatListItemData);
                    item.Target.transform.SetSiblingIndex(index);
                });
            }
        }
        /// <summary>
        /// 根据消息时间排序
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static int GetMsgListIndex(long time)
        {
            int count = 0;
            for (int i = 0; i < chatListItemList.Count; i++)
            {
                if (time < chatListItemList[i].time)
                {
                    count++;
                }
            }
            return count;
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
        public static void SetChatData(IListData<ChatData> chatDatas, Transform parent)
        {
            if (chatDatas.IsNullOrEmpty()) return;
            SetLastMsgID(chatDatas[chatDatas.Count - 1].id);
            for (int i = 0; i < chatDatas.list.Count; i++)
            {
                SaveChatMsgToLocal(AppVarData.Account, chatDatas.list[i]);
                if (chatListItemDict.ContainsKey(chatDatas.list[i].send_userid))
                {
                    ChatListItemData chatListItemData = chatListItemDict[chatDatas.list[i].send_userid];
                    chatListItemData.poolItem.SetTopMsg(chatDatas.list[i].msg_type, chatDatas.list[i].chat_msg);
                    chatListItemData.poolItem.SetTopTime(chatDatas.list[i].time);
                    chatListItemData.unreadCount++;
                    chatListItemData.poolItem.SetUnreadCount(chatListItemData.unreadCount);
                    chatListItemData.poolItem.Target.transform.SetAsFirstSibling();
                    SaveChatListToLocal(chatListItemData);
                }
                else
                {
                    ChatListItemData chatListItemData = new ChatListItemData();
                    chatListItemData.msgType = chatDatas.list[i].msg_type;
                    chatListItemData.topMsg = chatDatas.list[i].chat_msg;
                    chatListItemData.account = chatDatas.list[i].send_userid;
                    chatListItemData.time = chatDatas.list[i].time;
                    chatListItemData.unreadCount++;
                    SaveChatListToLocal(chatListItemData);
                    GameObjectPoolModule.AsyncPop<ChatMsgItemPool>((int)GameObjectPoolID.ChatMsgItem, parent, (item) =>
                    {
                        item.SetFriendAccount(chatListItemData.account);
                        item.SetTopMsg(chatListItemData.msgType, chatListItemData.topMsg);
                        item.SetTopTime(chatListItemData.time);
                        item.SetUnreadCount(chatListItemData.unreadCount);
                        chatListItemData.poolItem = item;
                        item.data = chatListItemData;
                        AddChatListItem(chatListItemData);
                        item.Target.transform.SetAsFirstSibling();
                    });
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
        public static void SaveChatListToLocal(ChatListItemData chatListItemData)
        {
            string path = ChatPathData.ChatListDir() + "/" + chatListItemData.account + ".txt";
            FileTools.Write(path, chatListItemData.ToBytes());
        }
        /// <summary>
        /// 添加消息列表对象
        /// </summary>
        /// <param name="chatListItemData"></param>
        private static void AddChatListItem(ChatListItemData chatListItemData)
        {
            chatListItemDict.Add(chatListItemData.account, chatListItemData);
            chatListItemList.Add(chatListItemData);
        }
        /// <summary>
        /// 更新聊天列表数据
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void UpdateChatListItem(ChatListItemData chatListItemData)
        {
            if (chatListItemData == null) return;
            if (chatListItemDict.ContainsKey(chatListItemData.account))
            {
                chatListItemDict[chatListItemData.account] = chatListItemData;
            }
            if (chatListItemList.Contains(chatListItemData))
            {
                chatListItemList[chatListItemList.IndexOf(chatListItemData)] = chatListItemData;
            }
            SaveChatListToLocal(chatListItemData);
        }
        /// <summary>
        /// 添加消息列表对象
        /// </summary>
        /// <param name="chatListItemData"></param>
        private static void ClearChatListItem()
        {
            chatListItemDict.Clear();
            chatListItemList.Clear();
        }
        #endregion
        #region NewFriend
        private static int mLastAddFriendID = -1;
        /// <summary>
        /// 好友申请列表
        /// </summary>
        public static Dictionary<long, AddFriendRequestData> addFriendListItemDict = new Dictionary<long, AddFriendRequestData>();
        /// <summary>
        /// 好友申请列表
        /// </summary>
        public static List<AddFriendRequestData> addFriendListItemList = new List<AddFriendRequestData>();

        /// <summary>
        /// 加载好友申请数据
        /// </summary>
        /// <returns></returns>
        public static void LoadAddFriendList(Transform parent)
        {
            GameObjectPoolModule.PushTarget((int)GameObjectPoolID.NewFriendItemPool);
            ClearAddFriendListItem();
            if (!Directory.Exists(ChatPathData.AddFriendListDir())) return;
            string[] files = Directory.GetFiles(ChatPathData.AddFriendListDir());
            if (files.IsNullOrEmpty()) return;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                AddFriendRequestData friendPairData = ConverterDataTools.ToObject<AddFriendRequestData>(chatBytes);
                GameObjectPoolModule.AsyncPop<NewFriendItemPool>((int)GameObjectPoolID.NewFriendItemPool, parent, (item) =>
                {
                    item.SetNewFriendData(friendPairData.friendAccount, friendPairData.addContent);
                    int index = GetAddFriendListIndex(friendPairData.id);
                    item.Target.transform.SetSiblingIndex(index);
                    friendPairData.poolItem = item;
                    AddNewFriendListItem(friendPairData);
                });
            }
        }
        /// <summary>
        /// 根据消息时间排序
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static int GetAddFriendListIndex(int id)
        {
            int count = 0;
            for (int i = 0; i < addFriendListItemList.Count; i++)
            {
                if (id < addFriendListItemList[i].id)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 获取最近的聊天ID
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
        public static void SetAddFriendListData(IListData<AddFriendRequestData> addFriendList)
        {
            if (addFriendList.IsNullOrEmpty()) return;
            SetLastAddFriendID(addFriendList[addFriendList.Count - 1].id);
            for (int i = 0; i < addFriendList.list.Count; i++)
            {
                if (!addFriendListItemDict.ContainsKey(addFriendList.list[i].friendAccount))
                {
                    AddFriendRequestData friendPairData = addFriendList.list[i];
                    SaveAddFriendRequestToLocal(friendPairData);
                }
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
        public static void SaveAddFriendRequestToLocal(AddFriendRequestData data)
        {
            string path = ChatPathData.AddFriendListDir() + "/" + data.friendAccount + ".txt";
            FileTools.Write(path, data.ToBytes());
        }
        /// <summary>
        /// 将聊天列表对象写入本地
        /// </summary>
        /// <param name="chatListItemData"></param>
        public static void RemoveLocalAddFriendRequest(long account)
        {
            string path = ChatPathData.AddFriendListDir() + "/" + account + ".txt";
            FileTools.ForceDelete(path);

        }
        /// <summary>
        /// 添加消息列表对象
        /// </summary>
        /// <param name="chatListItemData"></param>
        private static void AddNewFriendListItem(AddFriendRequestData data)
        {
            if (!addFriendListItemDict.ContainsKey(data.friendAccount))
            {
                addFriendListItemDict.Add(data.friendAccount, data);
                addFriendListItemList.Add(data);
            }
        }
        /// <summary>
        /// 清除好友申请消息列表对象
        /// </summary>
        /// <param name="chatListItemData"></param>
        private static void ClearAddFriendListItem()
        {
            addFriendListItemDict.Clear();
            for (int i = 0; i < addFriendListItemList.Count; i++)
            {
                addFriendListItemList[i].Recycle();
            }
            addFriendListItemList.Clear();
        }

        /// <summary>
        /// 移除好友申请消息列表对象
        /// </summary>
        /// <param name="chatListItemData"></param>
        private static void RemoveAddFriendListItem(long account)
        {
            if (addFriendListItemDict.ContainsKey(account))
            {
                addFriendListItemDict.Remove(account);
            }
            for (int i = 0; i < addFriendListItemList.Count; i++)
            {
                if (addFriendListItemList[i].friendAccount == account)
                {
                    addFriendListItemList.RemoveAt(i);
                    break;
                }
            }
        }
        /// <summary>
        /// 设置好友申请状态
        /// </summary>
        public static void SetAddFriendState(long account)
        {
            if (addFriendListItemDict.ContainsKey(account))
            {
                addFriendListItemDict[account].Recycle();
                RemoveAddFriendListItem(account);
            }
            RemoveLocalAddFriendRequest(account);
        }

        #endregion
        #region Friend
        private static int mLastFriendID = -1;
        private static List<FriendPairData> mFriendList = new List<FriendPairData>();
        private static Dictionary<byte, FriendGroupData> mFriendListGroupDict = new Dictionary<byte, FriendGroupData>();
        /// <summary>
        /// 加载好友数据
        /// </summary>
        /// <returns></returns>
        public static void LoadFriendList(Transform parent)
        {
            GameObjectPoolModule.PushTarget((int)GameObjectPoolID.FriendListItem);
            if (!Directory.Exists(ChatPathData.FriendListDir())) return;
            string[] files = Directory.GetFiles(ChatPathData.FriendListDir());
            if (files.IsNullOrEmpty()) return;
            for (int i = 0; i < files.Length; i++)
            {
                byte[] chatBytes = File.ReadAllBytes(files[i]);
                FriendPairData friendPairData = ConverterDataTools.ToObject<FriendPairData>(chatBytes);
                GameObjectPoolModule.AsyncPop<FriendListItemPool>((int)GameObjectPoolID.FriendListItem, parent, (item) =>
                {
                    item.SetFriendAccount(friendPairData.friendAccount);
                    friendPairData.friendListItemPool = item;
                    mFriendList.Add(friendPairData);
                });
            }
        }
        /// <summary>
        /// 根据好友名称获取下标
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetFriendListIndex(string name)
        {
            return 0;
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
        public static void SetFriendListData(IListData<FriendPairData> friendList, Transform parent)
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
        public static void SaveFriendListToLocal(FriendPairData friendPair)
        {
            string path = ChatPathData.FriendListDir() + "/" + friendPair.friendAccount + ".txt";
            FileTools.Write(path, friendPair.ToBytes());
        }
        #endregion
    }
}
