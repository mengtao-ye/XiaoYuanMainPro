namespace Game
{
    public class FriendMsgItemPool : BaseMsgItemPool<FriendMsgItemPool>
    {
        public override int Type =>(int)GameObjectPoolID.FriendMsgItem;
        public override string assetPath => "Prefabs/UI/Item/Chat/FriendMsgItem";
    }
}
