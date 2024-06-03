using UnityEngine;
using YFramework;

namespace Game
{
    public class CampusCircleReplayCommitScrollViewItem : BaseScrollViewItem
    {
        private static Vector2 DEFAULT_SIZE = new Vector2(1080, 150);
        public override Vector2 size { get; set; } = DEFAULT_SIZE;
        public override float anchoredPositionX => 0;

        public long ID;
        public long ReplayCommitID;//回复的评论ID
        public long Account;
        public string Content;
        public long ReplayID;//回复 回复 评论的ID
        public string ReplayContent;//回复 回复 评论的内容

        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            CampusCircleReplayCommitPool campusCircleCommitPool = gameObjectPoolTarget as CampusCircleReplayCommitPool;
            float len = campusCircleCommitPool.SetData(Account, ID, Content, ReplayCommitID, ReplayContent);
            if (len + 100 > DEFAULT_SIZE.y)
            {
                Vector2 tempSize = new Vector2(DEFAULT_SIZE.x, len + 100);
                UpdateSize(tempSize);
            }
        }

        public override void Recycle()
        {
            ID = 0;
            ReplayCommitID = 0;
            Account = 0;
            Content = string.Empty;
            ReplayID = 0;
            ReplayContent = string.Empty;
            ClassPool<CampusCircleReplayCommitScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<CampusCircleReplayCommitPool>(mParent, PopTarget);
        }
    }
}
