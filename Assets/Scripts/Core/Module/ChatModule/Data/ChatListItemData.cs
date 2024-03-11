using YFramework;

namespace Game
{
    public class ChatListItemData : IDataConverter
    {
        public long account;
        public string topMsg = "";
        public byte msgType;
        public long time;
        public int unreadCount = 0;//未读的信息数量
        public ChatMsgItemPool poolItem;
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
    }
}
