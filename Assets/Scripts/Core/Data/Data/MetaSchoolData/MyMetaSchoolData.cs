using YFramework;

namespace Game
{
    public class MyMetaSchoolData : IPool, IDataConverter
    {
        public long Account;
        public byte RoleID;
        public bool isPop { get ; set ; }
        public void PopPool()  {}
        public void PushPool()  {   }
        public  void Recycle()
        {
            ClassPool<MyMetaSchoolData>.Push(this);
        }
        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(Account.ToBytes());
            bytes.Add(RoleID.ToBytes());
            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }
        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            Account = bytes[0].ToLong();
            RoleID = bytes[1].ToByte();
            bytes.Recycle();
        }
    }
}
