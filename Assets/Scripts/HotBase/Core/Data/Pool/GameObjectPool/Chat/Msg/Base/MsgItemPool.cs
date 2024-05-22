using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public abstract class MsgItemPool<T> : BaseGameObjectPoolTarget<T> where T : IGameObjectPoolTarget, new()
    {
        public Text content;
        public RectTransform rectTransform;
        public MsgItemPool()
        {

        }
        public override void Init(GameObject target)
        {
            base.Init(target);
            rectTransform = target.GetComponent<RectTransform>();
            content = target.transform.FindObject<Text>("TextContent");
        }
        /// <summary>
        /// 设置聊天内容
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(string msg, ChatMsgType chatMsgType)
        {
            switch (chatMsgType)
            {
                case ChatMsgType.Null:
                    break;
                case ChatMsgType.Text:
                    content.text = msg;
                    break;
                case ChatMsgType.Audio:
                    break;
                case ChatMsgType.Image:
                    break;
            }
        }

    
    }
}
