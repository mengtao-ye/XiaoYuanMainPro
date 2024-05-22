using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class LostContactTipUI : BaseCustomTipsUI
    {
        private Text mContactText;
        private long mAccount;
        public LostContactTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mContactText = transform.FindObject<Text>("ContactText");
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
        }
       
        public void SetData(byte contactType,string contact,long account)
        {
            mAccount = account;
            string contactText = "";
            switch (contactType)
            {
                case 0:
                    contactText = "电话号码:";
                    break;
                case 1:
                    contactText = "微信:";
                    break;
                case 2:
                    contactText = "QQ:";
                    break;
            }
            mContactText.text = contactText + contact;
        }
    }
}
