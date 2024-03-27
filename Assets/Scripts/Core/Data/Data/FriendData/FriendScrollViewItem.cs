using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 好友对象数据
    /// </summary>
    public class FriendScrollViewItem : BaseScrollViewItem< FriendScrollViewItem>, IPool, IDataConverter
    {
        public int id;
        public long friendAccount;
        public string notes;
        public bool isPop { get ; set ; }
        public override Vector2 size { get; set; } = new Vector2(1080,150);
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public  void Recycle()
        {
            ClassPool<FriendScrollViewItem>.Push(this);
        }
        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(friendAccount.ToBytes());
            bytes.Add(notes.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }

        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            friendAccount = bytes[1].ToLong();
            notes = bytes[2].ToStr();
            bytes.Recycle();
        }

        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            FriendItemPool friendItemPool = gameObjectPoolTarget.As<FriendItemPool>();
            friendItemPool.SetFriendData(friendAccount, notes);
        }

        protected override IGameObjectPoolTarget PopTarget()
        {
            return GameObjectPoolModule.Pop<FriendItemPool>(mParent);
        }
    }
}
