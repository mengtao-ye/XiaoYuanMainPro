using YFramework;

namespace Game
{
    /// <summary>
    /// 失物招领信息
    /// </summary>
    public class LostData : IPool,IDataConverter
    {
        public int id;
        public string name;
        public string pos;
        public long startTime;
        public long endTime;
        public long account;
        public string images;
        public bool isPop { get ; set; }
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public  void Recycle()
        {
            ClassPool<LostData>.Push(this);
        }


        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(name.ToBytes());
            bytes.Add(pos.ToBytes());
            bytes.Add(startTime.ToBytes());
            bytes.Add(endTime.ToBytes());
            bytes.Add(images.ToBytes());
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
            images = bytes[5].ToStr();
            bytes.Recycle();
        }
    }
}
