using YFramework;

namespace Game
{
    public class MyMsgItemPool : MsgItemPool<MyMsgItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Chat/MyMsgItem";
        public override bool isUI => true;

        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
            ClassPool<MyMsgItemPool>.Push(this);
        }
    }
}
