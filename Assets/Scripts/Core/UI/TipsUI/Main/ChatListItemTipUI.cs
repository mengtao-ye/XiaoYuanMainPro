using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ChatListItemTipUI : BaseCustomTipsUI
    {
        private long ID;
        protected override ShowAnimEnum ShowAnim =>  ShowAnimEnum.None;
        protected override HideAnimEnum HideAnim =>  HideAnimEnum.None;
        public ChatListItemTipUI()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("DeleteChatMsgBtn").onClick.AddListener(DeleteChatMsgBtnListener);
        }

        public override void Show()
        {
            base.Show();
            SetBGClickCallBack(Hide);
            rectTransform.anchoredPosition = UITools.GetTipUIShowPos(Input.mousePosition,rectTransform.rect.size);
        }

        public void SetID(long id) 
        {
            ID = id;    
        }

        private void DeleteChatMsgBtnListener() 
        { 
                
        }
    }
}
