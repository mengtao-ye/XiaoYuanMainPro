using UnityEngine;
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
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { GameCenter.Instance.ShowPanel<MainPanel>(); });
        }

        public override void Show()
        {
            base.Show();
            mNotFindTip.SetAvtiveExtend(false);
            mUserDataGo.SetAvtiveExtend(false);
            mAddFriendBtn.gameObject.SetAvtiveExtend(false);
            mViewBtn.gameObject.SetAvtiveExtend(false);
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
            mNotFindTip.SetAvtiveExtend(false);
            mUserDataGo.SetAvtiveExtend(false);
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mFriendMsgIF.text.ToLong().ToBytes());
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.SearchFriendData, mSendBytes);
        }
        /// <summary>
        /// 显示查找到的好友信息
        /// </summary>
        /// <param name="account"></param>
        public void ShowFriendData(bool isFriend,long account)
        {
            mUserDataGo.SetAvtiveExtend(true);
            if (account == AppVarData.Account)
            {
                //搜索的是自己的账号
                mAddFriendBtn.gameObject.SetAvtiveExtend(false);
                mViewBtn.gameObject.SetAvtiveExtend(false);
            }
            else 
            {
                mAddFriendBtn.gameObject.SetAvtiveExtend(!isFriend);
                mViewBtn.gameObject.SetAvtiveExtend(isFriend);
            }
            mNotFindTip.SetAvtiveExtend(false);
            mAccount = account;
            UserDataModule.MapUserData(account,mHead,mName);
        }
        public void NotFindFriendData()
        {
            mNotFindTip.SetAvtiveExtend(true);
            mUserDataGo.SetAvtiveExtend(false);
        }

    }
}
