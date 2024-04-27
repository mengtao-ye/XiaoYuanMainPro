using YFramework;

namespace Game
{
    public class Role1Pool : BaseGameObjectPoolTarget<Role1Pool>, IRolePool
    {
        public override string assetPath => "Prefabs/Roles/Role1";
        public override bool isUI => false;
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
        public override void Push()
        {
            base.Push();
            transform.parent = null;
        }
    }
}
