using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// </summary>
    public class NewFriendScrollViewItem : BaseScrollViewItem, IPool, IDataConverter
    {
        public int id;
        public long friendAccount;
        public string addContent;
        public bool isPop { get ; set; }
        public override Vector2 size { get; set; } = new Vector2(1080,150);
        public override float anchoredPositionX =>0;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            NewFriendItemPool newFriendItemPool = gameObjectPoolTarget as NewFriendItemPool;
            newFriendItemPool.SetNewFriendData(friendAccount, addContent);
        }
        public void PopPool()
        {

        }

        public void PushPool()
        {

        }

        public  void Recycle()
        {
            ClassPool<NewFriendScrollViewItem>.Push(this);
        }
        public  byte[] ToBytes()
        {
            IListData<byte[]> bytes = ClassPool<ListData<byte[]>>.Pop();
            bytes.Add(id.ToBytes());
            bytes.Add(friendAccount.ToBytes());
            bytes.Add(addContent.ToBytes());
            byte[] mDatas = bytes.list.ToBytes();
            bytes.Recycle();
            return mDatas;
        }
        public  void ToValue(byte[] data)
        {
            IListData<byte[]> bytes = data.ToListBytes();
            id = bytes[0].ToInt();
            friendAccount = bytes[1].ToLong();
            addContent = bytes[2].ToStr();
            bytes.Recycle();
        }
        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<NewFriendItemPool>(mParent, PopTarget);
        }
    }
}
