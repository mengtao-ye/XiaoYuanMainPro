using UnityEngine;
using YFramework;

namespace Game
{
    public class SearchSchoolScrollViewItem : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1050,200);
        public override float anchoredPositionX => 0;
        public SchoolData schoolData;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            SearchSchoolItemPool searchSchoolItemPool = gameObjectPoolTarget as SearchSchoolItemPool;
            searchSchoolItemPool.SetData(schoolData);
        }

        public override void Recycle()
        {
            schoolData?.Recycle();
            ClassPool<SearchSchoolScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<SearchSchoolItemPool>(mParent,PopTarget);
        }
    }
}
