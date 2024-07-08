using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class RegisterCodeSubUI : BaseCustomSubUI
    {
        private RegisterPanel mRegisterPanel;
        private InputField mAccountIF;
        private InputField mCodeIF;
        private Text mVarifyTime;
        private int mCountDown = 0;
        private Button mVarifyBtnBtn;
        private Coroutine mCountDownCor;
        public RegisterCodeSubUI(Transform trans, RegisterPanel registerPanel) : base(trans)
        {
            mRegisterPanel = registerPanel;
        }
        public override void Awake()
        {
            base.Awake();
            mVarifyTime = transform.FindObject<Text>("VarifyTime");
            mVarifyTime.text = "60";
            mVarifyTime.gameObject.SetActive(false);
            mAccountIF = transform.FindObject<InputField>("Account");
            mCodeIF = transform.FindObject<InputField>("Code");
            transform.FindObject<Button>("NextBtn").onClick.AddListener(NextBtn);
            mVarifyBtnBtn = transform.FindObject<Button>("VarifyCodeBtn");
            mVarifyBtnBtn.onClick.AddListener(VarifyCodeBtnListener);
            mVarifyBtnBtn.gameObject.SetActiveExtend(true);
        }

        public override void Show()
        {
            base.Show();
            mVarifyTime.gameObject.SetActiveExtend(false);
            mVarifyBtnBtn.gameObject.SetActiveExtend(true);
        }

        public override void Hide()
        {
            base.Hide();
            if (mCountDownCor != null) 
            {
                IEnumeratorModule.StopCoroutine(mCountDownCor);
                mCountDownCor = null;
            }
        }

        private void VarifyCodeBtnListener() 
        {
            mVarifyTime.gameObject.SetActiveExtend(true);
            mVarifyBtnBtn.gameObject.SetActiveExtend(false);
            mCountDown = 60;
            mCountDownCor =  IEnumeratorModule.StartCoroutine(VarifyCodeCountDown());
        }

        private IEnumerator VarifyCodeCountDown() 
        {
            while (mCountDown > 0) 
            {
                mVarifyTime.text = mCountDown.ToString();
                mCountDown--;
                yield return Yielders.GetSeconds(1);
            }
            mVarifyTime.gameObject.SetActiveExtend(false);
            mVarifyBtnBtn.gameObject.SetActiveExtend(true);
            mCountDownCor = null;
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
