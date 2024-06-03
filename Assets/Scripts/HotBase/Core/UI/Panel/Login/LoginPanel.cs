using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoginPanel : BaseCustomPanel
    {
        private InputField mAccount;
        public string account { get { return mAccount.text; } }
        private InputField mPassword;
        public string password { get { return mPassword.text; } }
        private Button mLoginBtn;
        private Coroutine mAutoLoginCor;
        public LoginPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            Init();
            IProcess process = GameCenter.Instance.processController.Create()
                 .Concat(new CheckMainServerIsInitProcess())
              .Concat(new GetLoginServerPointProcess())
              ;
            process.processManager.Launcher();
        }

        public override void Start()
        {
            base.Start();
            //#if UNITY_EDITOR
            //            AudoLogin(18379366315, "528099tt...");
            //#endif
            mAutoLoginCor = IEnumeratorModule.StartCoroutine(IEAutoLogin());
        }

        public override void Hide()
        {
            base.Hide();
            if (mAutoLoginCor != null)
            {
                IEnumeratorModule.StopCoroutine(mAutoLoginCor);
                mAutoLoginCor = null;
            }
        }

        private IEnumerator IEAutoLogin()
        {
            if (!PlayerPrefsModule.Contains(PlayerPrefsData.AutoLoginAccount))
            {
                yield break;
            }
            var account_password = PlayerPrefsTools.GetLoginAccount();
            if (account_password.Item1.IsNullOrEmpty() || account_password.Item2.IsNullOrEmpty())
            {
                yield break;
            }
            while (!GameCenter.Instance.TcpSubServerIsContains(TcpSubServerType.Login))
            {
                yield return Yielders.WaitForEndOfFrame;
            }
            AudoLogin(account_password.Item1.ToLong(), account_password.Item2);
        }

        private void Init()
        {
            mLoginBtn = transform.FindObject<Button>("LoginBtn");
            mAccount = transform.FindObject<InputField>("AccountIF");
            mPassword = transform.FindObject<InputField>("PasswordIF");
            mLoginBtn.onClick.AddListener(LoginBtnListener);
            transform.FindObject<Button>("RegisterBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<RegisterPanel>();
            });
            transform.FindObject<Button>("QuitBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui) =>
                {
                    ui.SetType(CommonTwoTipID.QuitGame);
                    ui.ShowContent("是否退出应用？", "退出应用", "取消", null, "退出", () => { AppTools.QuitApp(); });
                });
            });
        }

        private void LoginBtnListener()
        {
            if (mAccount.text == string.Empty)
            {
                AppTools.ToastError("账号不能为空");
                return;
            }
            if (mAccount.text.Length != 11)
            {
                AppTools.ToastError("账号必须为11位");
                return;
            }
            if (mPassword.text == string.Empty)
            {
                AppTools.ToastError("密码不能为空");
                return;
            }
            if (mPassword.text.Length < 6)
            {
                AppTools.ToastError("密码长度为6-18位");
                return;
            }
            IListData<byte[]> loginBytes = ClassPool<ListData<byte[]>>.Pop();
            loginBytes.Add(mAccount.text.ToLong().ToBytes());
            loginBytes.Add(mPassword.text.ToBytes());
            byte[] sendDatas = loginBytes.list.ToBytes();
            loginBytes.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.LoginAccount, sendDatas);
        }

        public void AudoLogin(long account, string password)
        {
            mAccount.text = account.ToString();
            mPassword.text = password;
            mLoginBtn.onClick.Invoke();
        }
    }
}
