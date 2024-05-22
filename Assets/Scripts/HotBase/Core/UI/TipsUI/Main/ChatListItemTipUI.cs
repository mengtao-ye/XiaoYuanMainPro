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
            ID = 0;
            transform.FindObject<Button>("DeleteChatMsgBtn").onClick.AddListener(DeleteChatMsgBtnListener);
        }

        public override void Show()
        {
            base.Show();
            SetBGClickCallBack(Hide);
            rectTransform.anchoredPosition = UITools.GetTipUIShowPos(Input.mousePosition,rectTransform.rect.size);
        }

        public override void Hide()
        {
            base.Hide();
            ID = 0;
        }

        public void SetID(long id) 
        {
            ID = id;    
        }

        private void DeleteChatMsgBtnListener() 
        {
            if (ID == 0)
            {
                AppTools.ToastError("聊天记录删除失败");
            }
            else 
            {
                GameCenter.Instance.GetPanel<MainPanel>().msgSubUI.DeleteChatListItem(ID);
            }
            Hide();
        }
    }
}
