﻿using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FindFriendPanel : BaseCustomPanel
    {
        private InputField mFriendMsgIF;
        private GameObject mUserDataGo;
        private Image mHead;
        private Text mName;
        private long mAccount;
        private byte[] mSendBytes;
        private GameObject mNotFindTip;
        private Button mAddFriendBtn;
        private Button mViewBtn;
        public FindFriendPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mNotFindTip = transform.FindObject("NotFindTip");
            mUserDataGo = transform.FindObject("UserDataArea");
            mHead = mUserDataGo.transform.FindObject<Image>("Head");
            mName = mUserDataGo.transform.FindObject<Text>("Name");
            mAddFriendBtn = mUserDataGo.transform.FindObject<Button>("AddBtn");
            mViewBtn = mUserDataGo.transform.FindObject<Button>("ViewBtn");
            mAddFriendBtn.onClick.AddListener(AddFriendListener);
            mFriendMsgIF = transform.FindObject<InputField>("FriendMsgIF");
            transform.FindObject<Button>("SearchFriendBtn").onClick.AddListener(SearchBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { mUICanvas.CloseTopPanel(); });
        }

        public override void Show()
        {
            base.Show();
            mNotFindTip.SetActiveExtend(false);
            mUserDataGo.SetActiveExtend(false);
            mAddFriendBtn.gameObject.SetActiveExtend(false);
            mViewBtn.gameObject.SetActiveExtend(false);
        }


        private void AddFriendListener()
        {
            if (mAccount == 0) 
            {
                AppTools.ToastError("好友信息异常");
                return;
            }
            GameCenter.Instance.ShowPanel<SendAddFriendPanel>((ui)=> { ui.SetFriendAccount(mAccount); });
        }
        /// <summary>
        /// 查找按钮事件
        /// </summary>
        private void SearchBtnListener()
        {
            if (mFriendMsgIF.text.IsNullOrEmpty())
            {
                AppTools.Toast("请输入账号/ID");
                return;
            }
            mNotFindTip.SetActiveExtend(false);
            mUserDataGo.SetActiveExtend(false);
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mFriendMsgIF.text.ToLong().ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SearchFriendData, mSendBytes);
        }
        /// <summary>
        /// 显示查找到的好友信息
        /// </summary>
        /// <param name="account"></param>
        public void ShowFriendData(bool isFriend,long account)
        {
            mUserDataGo.SetActiveExtend(true);
            if (account == AppVarData.Account)
            {
                //搜索的是自己的账号
                mAddFriendBtn.gameObject.SetActiveExtend(false);
                mViewBtn.gameObject.SetActiveExtend(false);
            }
            else 
            {
                mAddFriendBtn.gameObject.SetActiveExtend(!isFriend);
                mViewBtn.gameObject.SetActiveExtend(isFriend);
            }
            mNotFindTip.SetActiveExtend(false);
            mAccount = account;
            UserDataModule.MapUserData(account,mHead,mName);
        }
        public void NotFindFriendData()
        {
            mNotFindTip.SetActiveExtend(true);
            mUserDataGo.SetActiveExtend(false);
        }

    }
}
