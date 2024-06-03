using UnityEngine;
using YFramework;

namespace Game
{
    public class LostScrollViewItem : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1080,300);
        public override float anchoredPositionX => 0;
        public LostData lostData;
        public bool isMy;//是否是我的失物招领
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            LostItemPool lostItemPool = gameObjectPoolTarget as LostItemPool;
            lostItemPool.SetData(lostData,isMy);
        }

        public override void Recycle()
        {
            isMy = false;
            ClassPool<LostScrollViewItem>.Push(this);
            lostData?.Recycle();
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<LostItemPool>(scrollViewTarget.content,PopTarget);
        }
    }
}
