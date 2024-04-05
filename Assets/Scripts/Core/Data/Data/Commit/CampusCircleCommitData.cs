using YFramework;

namespace Game
{
    public class CampusCircleCommitData : IPool,IDataConverter
    {
        public int ID;
        public long Account;
        public long CampusCircleID;
        public long ReplayID;
        public string Content;
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
            bytes.Add(ReplayID.ToBytes());
            bytes.Add(Content.ToBytes());
            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            ID = bytes[0].ToInt();
            Account = bytes[1].ToLong();
            CampusCircleID = bytes[2].ToLong();
            ReplayID = bytes[3].ToLong();
            Content = bytes[4].ToStr();
            bytes.Recycle();
        }
    }
}
