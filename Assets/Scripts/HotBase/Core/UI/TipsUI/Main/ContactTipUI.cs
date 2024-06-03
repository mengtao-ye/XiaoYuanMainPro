using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ContactTipUI : BaseCustomTipsUI
    {
        private Text mContactText;
        private GameObject mQQ;
        private GameObject mWeChat;
        private GameObject mPhone;
        public ContactTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mQQ = transform.FindObject("QQ");
            mWeChat = transform.FindObject("WeChat");
            mPhone = transform.FindObject("Phone");
            mContactText = transform.FindObject<Text>("ContactText");
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
        }


        public override void Show()
        {
            base.Show();
            mQQ.SetAvtiveExtend(false);
            mWeChat.SetAvtiveExtend(false);
            mPhone.SetAvtiveExtend(false);
        }

        public void SetData(byte contactType,string contact)
        {
            switch (contactType)
            {
                case 0:
                    mPhone.SetAvtiveExtend(true);
                    break;
                case 1:
                    mWeChat.SetAvtiveExtend(true);
                    break;
                case 2:
                    mQQ.SetAvtiveExtend(true);
                    break;
            }
            mContactText.text = contact;
        }
    }
}
