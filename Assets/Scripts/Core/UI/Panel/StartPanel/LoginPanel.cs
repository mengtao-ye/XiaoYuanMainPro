using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoginPanel : BaseCustomPanel
    {
        private string YongHuXieYi = "";
        private string GeRenXinXiText = "";
        private InputField mAccount;
        public string account { get { return mAccount.text; } }
        private InputField mPassword;
        public string password { get { return mPassword.text; } }
        private InputField mCode;
        public string code { get { return mCode.text; } }

        private Toggle mYongHuXieYiTG;
        private Button mLoginBtn;
        private GameObject mPasswordIF;
        private GameObject mCodeIF;
        private bool mIsAccountLogin;//是否是账号登录
        private Button mPhoneLoginBtn;
        private Button mAccountLoginBtn;
        private Text mVarifyTime;//短信验证倒计时时间
        private Coroutine mVarifyTimeCor;//倒计时协程
        private int mVarifyTimer;
        private Button mVarifyTimeBtn;
        public LoginPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            Init();
#if UNITY_EDITOR
            AudoLogin("1", "1");
#endif
        }
        private void Init()
        {
            mIsAccountLogin = true;
            mVarifyTime = transform.FindObject<Text>("VarifyTime");
            mPasswordIF = transform.FindObject("PasswordIF");
            mCodeIF = transform.FindObject("CodeIF");
            mLoginBtn = transform.FindObject<Button>("LoginBtn");
            mYongHuXieYiTG = transform.FindObject<Toggle>("IsRead");
            mYongHuXieYiTG.isOn = true;
            mAccount = transform.FindObject<InputField>("Account");
            mPassword = transform.FindObject<InputField>("Password");
            mCode = transform.FindObject<InputField>("Code");
            YongHuXieYi = "用户协议";
            GeRenXinXiText = "个人信息";
            transform.FindObject<Text>("YongHuXieYiText").rectTransform.AddEventTrigger(
                UnityEngine.EventSystems.EventTriggerType.PointerClick,
                (data) =>
                {
                    mUICanvas.showTipsPanel.ShowTipsUI<CommonTipsUI>().ShowContent(YongHuXieYi, "用户协议");
                });
            transform.FindObject<Text>("GeRenXinXiText").rectTransform.AddEventTrigger(
               UnityEngine.EventSystems.EventTriggerType.PointerClick,
               (data) =>
               {
                   mUICanvas.showTipsPanel.ShowTipsUI<CommonTipsUI>().ShowContent(GeRenXinXiText, "个人信息及隐私保护");
               });
            mLoginBtn.onClick.AddListener(LoginBtnListener);
            transform.FindObject<Button>("RegisterBtn").onClick.AddListener(() =>
            {
                
            });
            mPhoneLoginBtn = transform.FindObject<Button>("PhoneLoginBtn");
            mPhoneLoginBtn.onClick.AddListener(() =>
            {
                IsAccountLogin(false);
            });
            mAccountLoginBtn = transform.FindObject<Button>("AccountLoginBtn");
            mAccountLoginBtn.onClick.AddListener(() =>
            {
                IsAccountLogin(true);
            });
            mVarifyTimeBtn = transform.FindObject<Button>("VarifyCodeBtn");
            mVarifyTimeBtn.gameObject.SetAvtiveExtend(true);
            mVarifyTimeBtn.onClick.AddListener(VarifyCodeBtnListener);
        }

        private void LoginBtnListener()
        {
            if (!mYongHuXieYiTG.isOn)
            {
                AppTools.LogError("请勾选用户协议");
                return;
            }
            if (mAccount.text == "")
            {
                AppTools.LogError("账号不能为空");
                return;
            }
            if (mIsAccountLogin)
            {
                if (mPassword.text == "")
                {
                    AppTools.LogError("密码不能为空");
                    return;
                }
            }
            else
            {
                if (mCode.text == "")
                {
                    AppTools.LogError("验证码不能为空");
                    return;
                }
                else if (mCode.text.Length != 6)
                {
                    AppTools.LogError("验证码长度错误");
                    return;
                }
            }
            if (mIsAccountLogin)
            {
                IListData<byte[]> loginBytes = ClassPool<ListData<byte[]>>.Pop();
                loginBytes.Add(mAccount.text.ToInt().ToBytes());
                loginBytes.Add(mPassword.text.ToBytes());
                byte[] sendDatas = loginBytes.list.ToBytes();
                loginBytes.Recycle();
                //AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.LoginAccount, sendDatas);
            }
        }

        private void VarifyCodeBtnListener()
        {
            mVarifyTimeCor = IEnumeratorModule.StartCoroutine(IEVarifyTime());
        }


        private System.Collections.IEnumerator IEVarifyTime()
        {
            mVarifyTimeBtn.gameObject.SetAvtiveExtend(false);
            mVarifyTime.gameObject.SetAvtiveExtend(true);
            mVarifyTimer = 60;
            while (mVarifyTimer > 0)
            {
                yield return Yielders.GetSeconds(1);
                mVarifyTimer--;
                mVarifyTime.text = mVarifyTimer.ToString();
            }
            mVarifyTimeBtn.gameObject.SetAvtiveExtend(true);
            mVarifyTime.gameObject.SetAvtiveExtend(false);
        }

        private void IsAccountLogin(bool b)
        {
            mIsAccountLogin = b;
            mPasswordIF.SetAvtiveExtend(b);
            mCodeIF.SetAvtiveExtend(!b);
            mPhoneLoginBtn.gameObject.SetAvtiveExtend(b);
            mAccountLoginBtn.gameObject.SetAvtiveExtend(!b);
        }

        public void AudoLogin(string account, string password)
        {
            mYongHuXieYiTG.isOn = true;
            mAccount.text = account;
            mPassword.text = password;
            mLoginBtn.onClick.Invoke();
        }

    }
}
