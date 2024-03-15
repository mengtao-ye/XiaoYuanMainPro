using YFramework;

namespace Game
{
    public class MyMsgItemPool : BaseMsgItemPool<MyMsgItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Chat/MyMsgItem";
        public override bool isUI { get; } = true;
        public override void Recycle()
        {
            ClassPool<MyMsgItemPool>.Push(this);
        }
    }
}
