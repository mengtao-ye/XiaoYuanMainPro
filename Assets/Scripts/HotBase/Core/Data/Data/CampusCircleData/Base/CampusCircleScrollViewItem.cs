using UnityEngine;
using YFramework;

namespace Game
{
    public class CampusCircleScrollViewItem : BaseScrollViewItem
    {
        private static Vector2 DEFAULT_SIZE = new Vector2(1080, 250);
        public override Vector2 size { get; set; } = DEFAULT_SIZE;
        public long id;
        public long account;
        public string content;
        public long time;
        public bool isAnonymous;
        public bool isLike;
        public override float anchoredPositionX => 0;
        public bool hasData;
        public IListData<SelectImageData> imageTargets;
        public bool isFriendCampusCircle;//是否是好友朋友圈
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            CampusCircleItemPool campuscircle = gameObjectPoolTarget as CampusCircleItemPool;
            if (hasData)
            {
                float len = campuscircle.SetData(id, account, content, imageTargets, time, isAnonymous, isLike, isFriendCampusCircle);
                Vector2 tempSize = new Vector2(DEFAULT_SIZE.x, DEFAULT_SIZE.y + len);
                if (size != tempSize)
                {
                    scrollViewTarget.UpdateSize(this, tempSize);
                    campuscircle.rectTransform.sizeDelta += new Vector2(0, len);
                }
            }
            else
            {
                campuscircle.SetLoading();
            }
        }
        public void SetIsLike(bool isLike)
        {
            this.isLike = isLike;
            if (poolTarget != null && poolTarget.GameObjectIsPop)
            {
                CampusCircleItemPool campuscircle = poolTarget as CampusCircleItemPool;
                campuscircle.SetIsLike(isLike);
            }
        }

        public void IsLike(bool isLike)
        {
            CampusCircleItemPool campuscircle = poolTarget as CampusCircleItemPool;
            campuscircle.isLike = isLike;
        }

        public void SetLikeCount(int count)
        {
            CampusCircleItemPool campuscircle = poolTarget as CampusCircleItemPool;
            campuscircle.SetLikeCount(count);
        }
        public void SetCommitCount(int count)
        {
            CampusCircleItemPool campuscircle = poolTarget as CampusCircleItemPool;
            campuscircle.SetCommitCount(count);
        }
        public override void PushPool()
        {
            base.PushPool();
            hasData = false;
        }

        public override void Recycle()
        {
            imageTargets?.Recycle();
            ClassPool<CampusCircleScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.  AsyncPop<CampusCircleItemPool>(mParent, PopTarget);
        }
    }
}
