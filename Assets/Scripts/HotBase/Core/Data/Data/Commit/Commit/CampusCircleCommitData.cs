using YFramework;

namespace Game
{
    public class CampusCircleCommitData : IPool,IDataConverter
    {
        public long ID;
        public long Account;
        public long CampusCircleID;
        public string Content;
        public int ReplayCount;//回复数量

        public bool isPop { get; set; }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }
        public  void Recycle()
        {
            ClassPool<CampusCircleCommitData>.Push(this);
        }

        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(ID.ToBytes());
            bytes.Add(Account.ToBytes());
            bytes.Add(CampusCircleID.ToBytes());
            bytes.Add(Content.ToBytes());
            bytes.Add(ReplayCount.ToBytes());
            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            ID = bytes[0].ToLong();
            Account = bytes[1].ToLong();
            CampusCircleID = bytes[2].ToLong();
            Content = bytes[3].ToStr();
            ReplayCount = bytes[4].ToInt();
            bytes.Recycle();
        }
    }
}
