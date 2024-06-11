using YFramework;

namespace Game
{
    public class SocketReturnMsg : IPool, IDataConverter
    {
        public byte resultCode;
        public string msg;
        public byte[] datas;
        public bool isPop { get; set; }
        public bool isSuccess { get { return resultCode == (byte)SocketResultCode.Success; } }
        public void PopPool()
        {

        }
        public void PushPool()
        {
            resultCode = 0;
            msg = null;
            datas = null;
        }
        public void Recycle()
        {
            ClassPool<SocketReturnMsg>.Push(this);
        }

        public byte[] ToBytes()
        {
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(resultCode.ToBytes());
            list.Add(msg.ToBytes());
            list.Add(datas);
            byte[] bytes = list.list.ToBytes();
            list.Recycle();
            return bytes;
        }

        public void ToValue(byte[] data)
        {
            IListData<byte[]> list = data.ToListBytes();
            resultCode = list[0].ToByte();
            msg = list[1].ToStr();
            datas = list[2];
            list.Recycle();
        }
    }
}
