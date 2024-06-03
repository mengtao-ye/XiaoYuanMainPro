using UnityEngine;
using YFramework;

namespace Game
{
    public class CampusCircleCommitScrollViewItem : BaseScrollViewItem
    {
        private static Vector2 DEFAULT_SIZE = new Vector2(1080, 150);
        public override Vector2 size { get; set; } = DEFAULT_SIZE;
        public override float anchoredPositionX => 0;

        public long ID;
        public long Account;
        public string Content;
        public int ReplayCount;//回复数量
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            CampusCircleCommitPool campusCircleCommitPool = gameObjectPoolTarget as CampusCircleCommitPool;
            float len = campusCircleCommitPool.SetData(Account, ID, Content, ReplayCount);
            if (len + 100 > DEFAULT_SIZE.y)
            {
                Vector2 tempSize = new Vector2(DEFAULT_SIZE.x, len + 100);
                UpdateSize(tempSize);
            }
        }
        public override void Recycle()
        {
            size = DEFAULT_SIZE;
            Content = string.Empty;
            ID = 0;
            Account = 0;
            ReplayCount = 0;
            ClassPool<CampusCircleCommitScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<CampusCircleCommitPool>(mParent, PopTarget);
        }
    }
}
