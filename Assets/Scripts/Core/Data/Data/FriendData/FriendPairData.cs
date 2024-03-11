using YFramework;

namespace Game
{
    /// <summary>
    /// 好友列表
    /// </summary>
    public class FriendPairData : IPool, IDataConverter
    {
        public int id;
        public long friendAccount;
        public string notes;
        public bool isPop { get ; set ; }
        public FriendListItemPool friendListItemPool;
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public  void Recycle()
        {
            ClassPool<FriendPairData>.Push(this);
        }
        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(friendAccount.ToBytes());
            bytes.Add(notes.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            friendAccount = bytes[1].ToLong();
            notes = bytes[2].ToStr();
            bytes.Recycle();
        }
    }
}
