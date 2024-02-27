﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class RegisterNormalSubUI : BaseSubUI
    {
        private RegisterPanel mRegisterPanel;
        private InputField mUsernameIF;
        private InputField mPasswordIF;
        private InputField mComfinePasswordIF;
        private string mAccount;
        public RegisterNormalSubUI(Transform trans,RegisterPanel registerPanel) : base(trans)
        {
            mRegisterPanel = registerPanel;
        }

        public override void Awake()
        {
            base.Awake();
            mUsernameIF = transform.FindObject<InputField>("Username");
            mPasswordIF = transform.FindObject<InputField>("Password");
            mComfinePasswordIF = transform.FindObject<InputField>("ComfinePassword");
            transform.FindObject<Button>("RegisterBtn").onClick.AddListener(Register);
        }

        private void Register()
        {
            if (mUsernameIF.text == "")
            {
                AppTools.LogError("用户名不能为空");
                return;
            }
            if (mPasswordIF.text == "" || mComfinePasswordIF.text == "")
            {
                AppTools.LogError("密码不能为空");
                return;
            }
            if (mPasswordIF.text != mComfinePasswordIF.text)
            {
                AppTools.LogError("密码不一致");
                return;
            }
            IListData<byte[]> tempDataDict = ClassPool<ListData< byte[]>>.Pop();
            tempDataDict.Add( mAccount.ToLong().ToBytes());
            tempDataDict.Add( mUsernameIF.text.ToBytes());
            tempDataDict.Add( mPasswordIF.text.ToBytes());
            byte[] returnDatas = tempDataDict.list.ToBytes();
            tempDataDict.Recycle();
            AppTools.UdpSend( SubServerType.Login,(short) LoginUdpCode.RegisterAccount, returnDatas);
        }
        /// <summary>
        ///  设置账号数据
        /// </summary>
        /// <param name="account"></param>
        public void SetAccount(string account) 
        {
            mAccount = account;
        }
    }
}
