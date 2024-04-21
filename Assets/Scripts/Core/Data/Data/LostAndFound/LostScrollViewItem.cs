using UnityEngine;
using YFramework;

namespace Game
{
    public class LostScrollViewItem : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1080,160);
        public override float anchoredPositionX => 0;
        public int id;
        public string name;
        public string pos;
        public long startTime;
        public long endTime;
        public long account;
        public string images;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            LostItemPool lostItemPool = gameObjectPoolTarget as LostItemPool;
            lostItemPool.SetData(name,pos,startTime,endTime,images);
        }

        public override void Recycle()
        {
            ClassPool<LostScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<LostItemPool>(scrollViewTarget.content,PopTarget);
        }
    }
}
