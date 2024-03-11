using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public abstract class BaseMsgItemPool<T> : BaseGameObjectPoolTarget<T> where T : IGameObjectPoolTarget, new()
    {
        public Text content;
        public Image bgImg;
        public LayoutElement layoutElement ;
        private ChatData mChatData;
        public ContentSizeFitter contentSizeFitter;
        public override void Init(GameObject target)
        {
            base.Init(target);
            bgImg = target.GetComponent<Image>();
            contentSizeFitter = bgImg.GetComponent<ContentSizeFitter>();
            content = target.transform.FindObject<Text>("TextContent");
            layoutElement = content.GetComponent<LayoutElement>();
            target.GetComponent<Button>().onClick.AddListener(ClickBtnListener);
        }
        /// <summary>
        /// 设置聊天内容
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(ChatData data)
        {
            mChatData = data;
            ChatMsgType chatMsgType = (ChatMsgType)data.msg_type;
            switch (chatMsgType)
            {
                case ChatMsgType.Null:
                    break;
                case ChatMsgType.Text:
                    content.text = data.chat_msg;
                    break;
                case ChatMsgType.Audio:
                    break;
                case ChatMsgType.Image:
                    break;
            }
        }

        private void ClickBtnListener()
        {

        }
    }
}
