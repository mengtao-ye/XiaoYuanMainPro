using YFramework;

namespace Game
{
    /// <summary>
    /// 添加好友申请列表
    /// </summary>
    public class AddFriendRequestData : IPool, IDataConverter
    {
        public int id;
        public long friendAccount;
        public string addContent;
        public bool isPop { get ; set; }
        public NewFriendItemPool poolItem;
        public void PopPool()
        {
        }

        public void PushPool()
        {

        }

        public  void Recycle()
        {
            GameObjectPoolModule.Push(poolItem);
            ClassPool<AddFriendRequestData>.Push(this);
        }
        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(friendAccount.ToBytes());
            bytes.Add(addContent.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }
        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            friendAccount = bytes[1].ToLong();
            addContent = bytes[2].ToStr();
            bytes.Recycle();
        }
    }
}
