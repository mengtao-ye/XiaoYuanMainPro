using UnityEngine;
using YFramework;

namespace Game
{
    public class UnuseScrollViewItemData : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1080, 300);
        public override float anchoredPositionX => 0;
        private UnuseData mUnuseData;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            UnuseItemPool unuseItemPool = gameObjectPoolTarget as UnuseItemPool;
            unuseItemPool.SetData(mUnuseData);
        }

        public void SetData(UnuseData unuseData)
        {
            mUnuseData = unuseData;
        }

        public override void Recycle()
        {
            mUnuseData?.Recycle();
            ClassPool<UnuseScrollViewItemData>.Push(this);
            
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<UnuseItemPool>(mParent,PopTarget);
        }
    }
}
