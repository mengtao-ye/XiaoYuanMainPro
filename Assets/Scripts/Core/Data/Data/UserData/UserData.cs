using YFramework;

namespace Game
{
    public class UserData : IPool, IDataConverter
    {
        public int ID;
        public long Account;
        public string Username;
        public string Password;
        public string HeadURL;
        public string IDCard;
        private byte[] mDatas;
        public bool isPop { get ; set ; }
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public void Recycle()
        {
            ClassPool<UserData>.Push(this);
        }

        public  byte[] ToBytes()
        {
            if (mDatas == null)
            {
                IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
                bytes.Add(ID.ToBytes());
                bytes.Add(Account.ToBytes());
                bytes.Add(Username.ToBytes());
                bytes.Add(Password.ToBytes());
                bytes.Add(HeadURL.ToBytes());
                bytes.Add(IDCard.ToBytes());
                mDatas = bytes.list.ToBytes();
                bytes.Recycle();
            }
            return mDatas;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            ID = bytes[0].ToInt();
            Account = bytes[1].ToLong();
            Username = bytes[2].ToStr();
            Password = bytes[3].ToStr();
            HeadURL = bytes[4].ToStr();
            IDCard = bytes[5].ToString();
            bytes.Recycle();
        }
    }
}
