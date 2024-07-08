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
            mQQ.SetActiveExtend(false);
            mWeChat.SetActiveExtend(false);
            mPhone.SetActiveExtend(false);
        }

        public void SetData(byte contactType,string contact)
        {
            switch (contactType)
            {
                case 0:
                    mPhone.SetActiveExtend(true);
                    break;
                case 1:
                    mWeChat.SetActiveExtend(true);
                    break;
                case 2:
                    mQQ.SetActiveExtend(true);
                    break;
            }
            mContactText.text = contact;
        }
    }
}
