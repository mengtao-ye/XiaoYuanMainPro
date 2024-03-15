using YFramework;

namespace Game
{
    public class FriendMsgItemPool : BaseMsgItemPool<FriendMsgItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Chat/FriendMsgItem";
        public override bool isUI { get; } = true;
        public override void Recycle()
        {
            ClassPool<FriendMsgItemPool>.Push(this);
        }
    }
}
