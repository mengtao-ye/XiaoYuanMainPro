﻿using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class LoginPanel : BaseCustomPanel
    {
        private InputField mAccount;
        public string account { get { return mAccount.text; } }
        private InputField mPassword;
        public string password { get { return mPassword.text; } }
        private Button mLoginBtn;
        public LoginPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            Init();
            IProcess process = GameCenter.Instance.processController.Create()
                 .Concat(new CheckUdpServerIsInitProcess())
              .Concat(new GetLoginServerPointProcess())
              ;
            process.processManager.Launcher();
        }
        private void Init()
        {
            mLoginBtn = transform.FindObject<Button>("LoginBtn");
            mAccount = transform.FindObject<InputField>("AccountIF");
            mPassword = transform.FindObject<InputField>("PasswordIF");
            mLoginBtn.onClick.AddListener(LoginBtnListener);
            transform.FindObject<Button>("RegisterBtn").onClick.AddListener(() =>
            {
                
            });
            transform.FindObject<Button>("QuitBtn").onClick.AddListener(() =>
            {

            });
        }

        private void LoginBtnListener()
        {
            if (mAccount.text == "")
            {
                AppTools.LogError("账号不能为空");
                return;
            }
            if (mAccount.text .Length !=11)
            {
                AppTools.LogError("账号必须为11位");
                return;
            }
            if (mPassword.text == "")
            {
                AppTools.LogError("密码不能为空");
                return;
            }
            if (mPassword.text.Length < 6)
            {
                AppTools.LogError("密码长度为6-18位");
                return;
            }
            IListData<byte[]> loginBytes = ClassPool<ListData<byte[]>>.Pop();
            loginBytes.Add(mAccount.text.ToInt().ToBytes());
            loginBytes.Add(mPassword.text.ToBytes());
            byte[] sendDatas = loginBytes.list.ToBytes();
            loginBytes.Recycle();
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.LoginAccount, sendDatas);
        }

        public void AudoLogin(string account, string password)
        {
            mAccount.text = account;
            mPassword.text = password;
            mLoginBtn.onClick.Invoke();
        }
    }
}
