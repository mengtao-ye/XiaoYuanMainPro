using UnityEngine;
using YFramework;

namespace Game
{
    public class PartTimeJobApplicationScrollViewItem : BaseScrollViewItem
    {
        public override Vector2 size { get; set; } = new Vector2(1080, 300);
        public override float anchoredPositionX =>0;
        public int id;
        public string name;
        public bool isMan;
        public int age;
        public string call;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            PartTimeJobApplicationPool pool = gameObjectPoolTarget as PartTimeJobApplicationPool;
            pool.ID = id;
            pool.SetData(name,isMan,age,call);
        }

        public override void Recycle()
        {
            ClassPool<PartTimeJobApplicationScrollViewItem>.Push(this);
        }

        protected override void StartPopTarget()
        {
            GameObjectPoolModule.AsyncPop<PartTimeJobApplicationPool>(mParent,PopTarget);
        }
    }
}
