using YFramework;

namespace Game
{
    public class SchoolData : IPool,IDataConverter
    {
        public int schoolID;
        public string name;
        public string icon;
        public string bg;
        public bool isPop { get; set ; }
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
            list.Add(icon.ToBytes());
            list.Add(bg.ToBytes());
            byte[] bytes = list.list.ToBytes();
            list.Recycle();
            return bytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> list = data.ToListBytes();
            schoolID = list[0].ToInt();
            name = list[1].ToStr();
            icon = list[2].ToStr();
            bg = list[3].ToStr();
            list.Recycle();
        }
    }
}
