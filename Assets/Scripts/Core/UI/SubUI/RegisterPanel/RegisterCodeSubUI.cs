using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class RegisterCodeSubUI : BaseCustomSubUI
    {
        private RegisterPanel mRegisterPanel;
        private InputField mAccountIF;
        private InputField mCodeIF;
        public RegisterCodeSubUI(Transform trans, RegisterPanel registerPanel) : base(trans)
        {
            mRegisterPanel = registerPanel;
        }
        public override void Awake()
        {
            base.Awake();
            mAccountIF = transform.FindObject<InputField>("Account");
            mCodeIF = transform.FindObject<InputField>("Code");
            transform.FindObject<Button>("NextBtn").onClick.AddListener(NextBtn);
        }

        private void NextBtn()
        {
            if (mAccountIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("账号不能为空");
                return;
            }
            if (mCodeIF.text.IsNullOrEmpty() || mCodeIF.text.Length !=6)
            {
                AppTools.ToastNotify("验证码格式错误");
                return;
            }
            Hide();
            mRegisterPanel.normalSubUI.SetAccount(mAccountIF.text);
            mRegisterPanel.normalSubUI.Show();
        }
    }
}
