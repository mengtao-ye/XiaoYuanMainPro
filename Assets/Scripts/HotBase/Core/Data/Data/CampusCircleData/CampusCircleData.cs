using YFramework;

namespace Game
{
    public class CampusCircleData : IPool,IDataConverter
    {
        public long ID;
        public long Account;
        public string Content;
        public byte[] Images;
        public long SchoolID;
        public long Time;
        public bool IsAnonymous;//是否是匿名

        public bool isPop { get; set ; }
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public  void Recycle()
        {
            ClassPool<CampusCircleData>.Push(this);
        }

        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(ID.ToBytes());
            bytes.Add(Account.ToBytes());
            bytes.Add(Content.ToBytes());
            bytes.Add(Images);
            bytes.Add(SchoolID.ToBytes());
            bytes.Add(Time.ToBytes());
            bytes.Add(IsAnonymous.ToBytes());
            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            ID = bytes[0].ToLong();
            Account = bytes[1].ToLong();
            Content = bytes[2].ToStr();
            Images = bytes[3];
            SchoolID = bytes[4].ToLong();
            Time = bytes[5].ToLong();
            IsAnonymous = bytes[6].ToBool();
            bytes.Recycle();
        }
    }
}
