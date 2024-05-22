using YFramework;

namespace Game
{
    /// <summary>
    /// 失物招领信息
    /// </summary>
    public class LostData : IPool, IDataConverter
    {
        public int id;
        public string name;
        public string pos;
        public long startTime;
        public long endTime;
        public long account;
        public byte[] images;
        public long schoolCode;
        public long updateTime;
        public string detail;
        public byte contactType;
        public string contact;
        public IListData<SelectImageData> imageListData;
        public bool isPop { get; set ; }
        public void PopPool()    { }
        public void PushPool() { }
        public  void Recycle()
        {
            ClassPool<LostData>.Push(this);
            imageListData?.Recycle();
        }

        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(name.ToBytes());
            bytes.Add(pos.ToBytes());
            bytes.Add(startTime.ToBytes());
            bytes.Add(endTime.ToBytes());
            bytes.Add(account.ToBytes());
            bytes.Add(images);
            bytes.Add(schoolCode.ToBytes());
            bytes.Add(updateTime.ToBytes());
            bytes.Add(detail.ToBytes());
            bytes.Add(contactType.ToBytes());
            bytes.Add(contact.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            name = bytes[1].ToStr();
            pos = bytes[2].ToStr();
            startTime = bytes[3].ToLong();
            endTime = bytes[4].ToLong();
            account = bytes[5].ToLong();
            images = bytes[6];
            imageListData = SelectImageDataTools.GetData(images);
            schoolCode = bytes[7].ToLong();
            updateTime = bytes[8].ToLong();
            detail = bytes[9].ToStr();
            contactType = bytes[10].ToByte();
            contact = bytes[11].ToStr();
            bytes.Recycle();
        }
    }
}
