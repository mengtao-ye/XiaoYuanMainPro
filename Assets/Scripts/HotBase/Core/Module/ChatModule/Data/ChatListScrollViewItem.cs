﻿using UnityEngine;
using YFramework;

namespace Game
{
    public class ChatListScrollViewItem : BaseScrollViewItem,IDataConverter
    {
        public long account;
        public string topMsg = string.Empty;
        public byte msgType;
        public long time;//时间戳
        public int unreadCount = 0;//未读的信息数量
        public override Vector2 size { get; set; } = new Vector2(1080,122);
        public override float anchoredPositionX => 0;

        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            ChatListItemPool chatMsgItemPool = gameObjectPoolTarget as ChatListItemPool;
            chatMsgItemPool.SetFriendAccount(account);
            chatMsgItemPool.SetTopTime(time);
            chatMsgItemPool.SetTopMsg(msgType, topMsg);
            chatMsgItemPool.SetUnreadCount(unreadCount);
            chatMsgItemPool.ID = ViewItemID;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData() 
        {
            if (isInstantiate) 
            {
                ChatListItemPool chatMsgItemPool = poolTarget as ChatListItemPool;
                chatMsgItemPool.SetFriendAccount(account);
                chatMsgItemPool.SetTopTime(time);
                chatMsgItemPool.SetTopMsg(msgType, topMsg);
                chatMsgItemPool.SetUnreadCount(unreadCount);
                chatMsgItemPool.ID = ViewItemID;
            }
        }

        public override void Recycle()
        {
            ClassPool<ChatListScrollViewItem>.Push(this);
        }

        public byte[] ToBytes()
        {
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(account.ToBytes());
            list.Add(topMsg.ToBytes());
            list.Add(msgType.ToBytes());
            list.Add(time.ToBytes());
            list.Add(unreadCount.ToBytes());
            byte[] bytes = list.list.ToBytes();
            list.Recycle();
            return bytes;
        }

        public void ToValue(byte[] data)
        {
            IListData<byte[]> list = data.ToListBytes();
            account = list[0].ToLong();
            topMsg = list[1].ToStr();
            msgType = list[2].ToByte();
            time = list[3].ToLong();
            unreadCount = list[4].ToInt();
            list.Recycle();
        }
        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<ChatListItemPool>(mParent,PopTarget);
        }
    }
}
