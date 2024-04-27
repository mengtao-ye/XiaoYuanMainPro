using YFramework;

namespace Game
{
    public class SchoolData : IPool,IDataConverter
    {
        public int schoolID;
        public string name;
        public bool isPop { get; set ; }
        public long schoolCode;//学校编码
        public string assetBundleName;//ab包资源名称
        public void PopPool()
        {
           
        }

        public void PushPool()
        {
            
        }

        public  void Recycle()
        {
            ClassPool<SchoolData>.Push(this);
        }

        public  byte[] ToBytes()
        {
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(schoolID.ToBytes());
            list.Add(name.ToBytes());
            list.Add(schoolCode.ToBytes());
            list.Add(assetBundleName.ToBytes());
            byte[] bytes = list.list.ToBytes();
            list.Recycle();
            return bytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> list = data.ToListBytes();
            schoolID = list[0].ToInt();
            name = list[1].ToStr();
            schoolCode = list[2].ToLong();
            assetBundleName = list[3].ToStr();
            list.Recycle();
        }
    }
}
