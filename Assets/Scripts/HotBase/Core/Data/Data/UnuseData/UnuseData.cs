using YFramework;

namespace Game
{
    public class UnuseData : IDataConverter,IPool
    {
        public int id;
        public long account;
        public long time;
        public string content;
        public byte[] images;
        public byte type;
        public int price;
        public byte contactType;
        public string contact;
        public bool isPop { get ; set ; }
        public void PopPool(){}
        public void PushPool(){}
        public IListData<SelectImageData> imageTargets;
        public  void Recycle()
        {
            ClassPool<UnuseData>.Push(this);
            imageTargets?.Recycle();
        }

        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(account.ToBytes());
            bytes.Add(time.ToBytes());
            bytes.Add(content.ToBytes());
            bytes.Add(images);
            bytes.Add(type.ToBytes());
            bytes.Add(price.ToBytes());
            bytes.Add(contactType.ToBytes());
            bytes.Add(contact.ToBytes());
            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            account = bytes[1].ToLong();
            time = bytes[2].ToLong();
            content = bytes[3].ToStr();
            images = bytes[4];
            imageTargets = SelectImageDataTools.GetData(images);
            type = bytes[5].ToByte();
            price = bytes[6].ToInt();
            contactType = bytes[7].ToByte();
            contact = bytes[8].ToStr();
            bytes.Recycle();
        }
    }
}
