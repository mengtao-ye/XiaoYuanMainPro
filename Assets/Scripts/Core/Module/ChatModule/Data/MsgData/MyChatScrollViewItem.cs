using UnityEngine;
using YFramework;

namespace Game
{
    public class MyChatScrollViewItem : MsgScrollViewItem, IPool
    {
        public override void Recycle()
        {
            ClassPool<MyChatScrollViewItem>.Push(this);
        }

        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
            MyMsgItemPool itemPool = gameObjectPoolTarget.As<MyMsgItemPool>();
            itemPool.SetContent(chat_msg, (ChatMsgType)msg_type);
            float textLen = itemPool.content.preferredWidth;
            float splitLen = YFrameworkHelper.Instance.ScreenSize.x * 0.6f;
            int col = ((int)(textLen / splitLen)) + 1;
            itemPool.content.rectTransform.sizeDelta = new Vector2(splitLen, itemPool.content.preferredHeight);
            if (col == 1)
            {
                itemPool.rectTransform.sizeDelta = new Vector2(textLen + 20, itemPool.content.preferredHeight + 20);
            }
            else
            {
                itemPool.rectTransform.sizeDelta = new Vector2(splitLen + 20, itemPool.content.preferredHeight + 20);
            }
            scrollViewTarget.UpdateSize(this, itemPool.rectTransform.sizeDelta);
        }
        protected override IGameObjectPoolTarget PopTarget()
        {
            return GameObjectPoolModule.Pop<MyMsgItemPool>(mParent);
        }
    }
}
