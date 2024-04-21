using UnityEngine;
using YFramework;

namespace Game
{
    public class CampusCircleScrollViewItem : BaseScrollViewItem
    {
        private static Vector2 DEFAULT_SIZE = new Vector2(1080, 230);
        public override Vector2 size { get; set; } = DEFAULT_SIZE;
      
        public long id;
        public long account;
        public string content;
        public string images;
        public long time;
        public bool isAnonymous;
        public int likeCount;
        public int commitCount;
        public bool isLike;
        public override float anchoredPositionX => 0;
        public bool hasData;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            CampusCircleItemPool campuscircle = gameObjectPoolTarget as CampusCircleItemPool;
            if (hasData)
            {
                float len = campuscircle.SetData(id, account, content, images, time, isAnonymous, likeCount, commitCount, isLike);
                Vector2 tempSize = new Vector2(DEFAULT_SIZE.x, DEFAULT_SIZE.y + len);
                if (size != tempSize)
                {
                    scrollViewTarget.UpdateSize(this, tempSize);
                }
            }
            else
            {
                campuscircle.SetLoading();
            }
        }

        public void SetData()
        {
            CampusCircleItemPool campuscircle = poolTarget as CampusCircleItemPool;
            float len = campuscircle.SetData(id, account, content, images, time, isAnonymous, likeCount, commitCount, isLike);
            scrollViewTarget.UpdateSize(this, new Vector2(size.x, size.y + len));
            campuscircle.rectTransform.sizeDelta += new Vector2(0, len);
        }

        public void SetIsLike(bool isLike,bool needUpdate)
        {
            this.isLike = isLike;
            if (poolTarget != null && poolTarget.GameObjectIsPop)
            {
                CampusCircleItemPool campuscircle = poolTarget as CampusCircleItemPool;
                campuscircle.SetIsLike(isLike);
                if (needUpdate) {
                    if (isLike)
                    {
                        likeCount++;
                    }
                    else
                    {
                        likeCount--;
                    }
                    campuscircle.SetLikeCount(likeCount);
                }
            }
        }



        public override void PushPool()
        {
            base.PushPool();
            hasData = false;
        }

        public override void Recycle()
        {
            ClassPool<CampusCircleScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<CampusCircleItemPool>(mParent, PopTarget);
        }
    }
}
