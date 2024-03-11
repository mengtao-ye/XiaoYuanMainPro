namespace Game
{
    public class MyMsgItemPool : BaseMsgItemPool<MyMsgItemPool>
    {
        public override int Type =>(int)GameObjectPoolID.MyMsgItem;
        public override string assetPath => "Prefabs/UI/Item/Chat/MyMsgItem";
    }
}
