using YFramework;

namespace Game
{
    public class CampusCircleReplayCommitData : IPool, IDataConverter
    {
        public long ID;
        public long ReplayCommitID;//回复的评论ID
        public long Account;
        public string Content;
        public long ReplayID;//回复 回复 评论的ID
        public string ReplayContent;//回复 回复 评论的内容
        public bool isPop { get; set; }
        public void PopPool() { }
        public void PushPool() { }
        public void Recycle()
        {
            ClassPool<CampusCircleReplayCommitData>.Push(this);
        }

        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(ID.ToBytes());
            bytes.Add(ReplayCommitID.ToBytes());
            bytes.Add(Account.ToBytes());
            bytes.Add(Content.ToBytes());
            bytes.Add(ReplayID.ToBytes());
            bytes.Add(ReplayContent.ToBytes());

            byte[] returnBytes = bytes.list.ToBytes();
            bytes.Recycle();
            return returnBytes;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            ID = bytes[0].ToInt();
            ReplayCommitID = bytes[1].ToLong();
            Account = bytes[2].ToLong();
            Content = bytes[3].ToStr();
            ReplayID = bytes[4].ToLong();
            ReplayContent = bytes[5].ToStr();

            bytes.Recycle();
        }
    }
}
