using YFramework;

namespace Game
{
    /// <summary>
    /// 失物招领信息
    /// </summary>
    public class FoundData : IPool,IDataConverter
    {
        public int id;
        public long account;
        public string name;
        public string pos;
        public long time;
        public byte[] images;
        public long schoolCode;
        public long updateTime;
        public string detail;
        public byte contactType;
        public string contact;
        public string quest;
        public string result;
        public IListData<SelectImageData> imageListData;
        public bool isPop { get ; set ; }
        public void PopPool()  {}
        public void PushPool()  { }
        public  void Recycle()
        {
            imageListData?.Recycle();
            ClassPool<FoundData>.Push(this);
        }


        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(account.ToBytes());
            bytes.Add(name.ToBytes());
            bytes.Add(pos.ToBytes());
            bytes.Add(time.ToBytes());
            bytes.Add(images);
            bytes.Add(schoolCode.ToBytes());
            bytes.Add(updateTime.ToBytes());
            bytes.Add(detail.ToBytes());
            bytes.Add(contactType.ToBytes());
            bytes.Add(contact.ToBytes());
            bytes.Add(quest.ToBytes());
            bytes.Add(result.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            account = bytes[1].ToLong();
            name = bytes[2].ToStr();
            pos = bytes[3].ToStr();
            time = bytes[4].ToLong();
            images = bytes[5];
            imageListData = SelectImageDataTools.GetData(images);
            schoolCode = bytes[6].ToLong();
            updateTime = bytes[7].ToLong();
            detail = bytes[8].ToStr();
            contactType = bytes[9].ToByte();
            contact = bytes[10].ToStr();
            quest = bytes[11].ToStr();
            result = bytes[12].ToStr();
            bytes.Recycle();
        }
    }
}
