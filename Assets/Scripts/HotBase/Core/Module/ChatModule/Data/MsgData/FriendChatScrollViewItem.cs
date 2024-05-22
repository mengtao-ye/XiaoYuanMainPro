using System.Text;
using UnityEngine;
using YFramework;

namespace Game
{
    public class FriendChatScrollViewItem : MsgScrollViewItem, IPool
    {
        public override void ParentRecycle()
        {
            ClassPool<FriendChatScrollViewItem>.Push(this);
        }

        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            FriendMsgItemPool itemPool = gameObjectPoolTarget.As<FriendMsgItemPool>();
            itemPool.SetContent(chat_msg, (ChatMsgType)msg_type);
            float textLen = itemPool.content.preferredWidth;
            float splitLen = YFrameworkHelper.Instance.ScreenSize.x * 0.6f;
            int col = ((int)(textLen / splitLen)) + 1;
            itemPool.content.rectTransform.sizeDelta = new Vector2(splitLen, itemPool.content.preferredHeight);
            if (col == 1)
            {
                itemPool.rectTransform.sizeDelta = new Vector2((int)textLen + 20, (int)itemPool.content.preferredHeight + 20);
            }
            else
            {
                itemPool.rectTransform.sizeDelta = new Vector2((int)splitLen + 20, (int)itemPool.content.preferredHeight + 20);
            }
            if (size != itemPool.rectTransform.sizeDelta)
            {
                scrollViewTarget.UpdateSize(this, itemPool.rectTransform.sizeDelta);
            }
        }

        protected override void VirtualStartPopTarget()
        {
            base.VirtualStartPopTarget();
            GameObjectPoolModule.AsyncPop<FriendMsgItemPool>(mParent,PopTarget);
        }
    }
}
