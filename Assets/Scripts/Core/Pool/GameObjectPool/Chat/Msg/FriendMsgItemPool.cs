using YFramework;

namespace Game
{
    public class FriendMsgItemPool : MsgItemPool<FriendItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Chat/FriendMsgItem";
        public override bool isUI => true;

        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
            ClassPool<FriendMsgItemPool>.Push(this);
        }
    }
}
