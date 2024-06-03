using UnityEngine;
using YFramework;

namespace Game
{
    public class FoundScrollViewItem : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1080,300);
        public override float anchoredPositionX => 0;
        public FoundData foundData;
        public bool isMy;//是否是我的寻物
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            FoundItemPool foundItemPool = gameObjectPoolTarget as FoundItemPool;
            foundItemPool.SetData(foundData,isMy);
        }

        public override void Recycle()
        {
            isMy = false;
            ClassPool<FoundScrollViewItem>.Push(this);
            foundData?.Recycle();
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<FoundItemPool>(scrollViewTarget.content,PopTarget);
        }
    }
}
