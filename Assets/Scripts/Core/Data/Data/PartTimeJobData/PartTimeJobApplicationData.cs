using YFramework;

namespace Game
{
    public class PartTimeJobApplicationData : IPool, IDataConverter
    {
        public int id;
        public long account;
        public int partTimeJobID;
        public string name;
        public bool isMan;
        public int age;
        public string call;
        public bool isPop { get ; set ; }
        public void PopPool()   {  }
        public void PushPool() { }
        public  void Recycle()
        {
            ClassPool<PartTimeJobApplicationData>.Push(this);
        }


        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(account.ToBytes());
            bytes.Add(partTimeJobID.ToBytes());
            bytes.Add(name.ToBytes());
            bytes.Add(isMan.ToBytes());
            bytes.Add(age.ToBytes());
            bytes.Add(call.ToBytes());
            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            account = bytes[1].ToLong();
            partTimeJobID = bytes[2].ToInt();
            name = bytes[3].ToStr();
            isMan = bytes[4].ToBool();
            age = bytes[5].ToInt();
            call = bytes[6].ToStr();
            bytes.Recycle();
        }
    }
}
