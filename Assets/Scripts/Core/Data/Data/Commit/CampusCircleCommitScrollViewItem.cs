using UnityEngine;
using YFramework;

namespace Game
{
    public class CampusCircleCommitScrollViewItem : BaseScrollViewItem, IPool
    {
        private static Vector2 DEFAULT_SIZE = new Vector2(1080, 100);
        public override Vector2 size { get; set; } = DEFAULT_SIZE;
        public bool isPop { get; set; }
        public override float anchoredPositionX => 0;

        public int ID;
        public long Account;
        public long CampusCircleID;
        public long ReplayID;
        public string Content;

        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            CampusCircleCommitPool campusCircleCommitPool = gameObjectPoolTarget as CampusCircleCommitPool;
            float len = campusCircleCommitPool.SetData(Account, ID, Content, ReplayID);
            if (len + 50 > DEFAULT_SIZE.y)
            {
                Vector2 tempSize = new Vector2(DEFAULT_SIZE.x, len + 50);
                UpdateSize(tempSize);
            }
        }

        public void SetData()
        {
        
        }

        public void PopPool()
        {

        }

        public void PushPool()
        {
        
        }

        public void Recycle()
        {
            GameObjectPoolModule.Push(poolTarget);
            ClassPool<CampusCircleCommitScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<CampusCircleCommitPool>(mParent, PopTarget);
        }
    }
}
