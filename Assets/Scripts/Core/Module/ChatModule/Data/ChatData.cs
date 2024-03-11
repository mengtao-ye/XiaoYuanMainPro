using YFramework;

namespace Game
{
    /// <summary>
    /// 聊天信息
    /// </summary>
    public class ChatData : IPool, IDataConverter
    {
        public long id;
        public long send_userid;
        public long receive_userid;
        public byte msg_type;
        public string chat_msg;
        public long time;
        public bool isPop { get; set; }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }

        public  void Recycle()
        {
            ClassPool<ChatData>.Push(this);
        }
        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(send_userid.ToBytes());
            bytes.Add(receive_userid.ToBytes());
            bytes.Add(msg_type.ToBytes());
            bytes.Add(chat_msg.ToBytes());
            bytes.Add(time.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToLong();
            send_userid = bytes[1].ToLong();
            receive_userid = bytes[2].ToLong();
            msg_type = bytes[3].ToByte();
            chat_msg = bytes[4].ToStr();
            time = bytes[5].ToLong();
            bytes.Recycle();
        }
    }
}
