using UnityEngine;
using YFramework;

namespace Game
{
    public class PartTimeJobScrollViewItem : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1080,200);
        public override float anchoredPositionX =>0;
        public string title;
        public string time;
        public string position;
        public int price;
        public byte priceType;
        public int id;
        public string detail;
        public bool isMyApplication;//是否是我的报名列表
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            PartTimeJobPool pool = gameObjectPoolTarget as PartTimeJobPool;
            pool.ID = id;
            pool.SetData(title,time,position,price,priceType, detail, isMyApplication);
        }

        public override void Recycle()
        {
            ClassPool<PartTimeJobScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<PartTimeJobPool>(mParent,PopTarget);
        }
    }
}
