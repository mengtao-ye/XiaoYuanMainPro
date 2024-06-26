using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public enum PageType
    {
        Main,
        Msg,
        Menu,
        Mine
    }
    public class MainPageData
    {
        public ISubUI subUI;
        public MainPanel panel;
        public PageType pageType;
        private float mPreClickTime;
        private Text mBtnText;
        private Image mBtnImage;
        public MainPageData(MainPanel main, ISubUI subUI, Button button, PageType pageType)
        {
            this.pageType = pageType;
            this.subUI = subUI;
            panel = main;
            button.onClick.AddListener(ClickListener);
            mBtnImage = button.transform.FindObject<Image>("Icon");
            mBtnText = button.transform.FindObject<Text>("T");
            main.AddSubUI(subUI);
            mBtnImage.color = ColorConstData.BottomNormalColor;
            mBtnText.color = ColorConstData.BottomNormalColor;
        }
        private void ClickListener()
        {
            panel.ShowPage(pageType);
            if (Time.time - mPreClickTime < 0.2f)
            {
                subUI.Refresh();
            }
            mPreClickTime = Time.time;
        }

        public void Hide()
        {
            mBtnImage.color = ColorConstData.BottomNormalColor;
            mBtnText.color = ColorConstData.BottomNormalColor;
            subUI.Hide();
        }
        public void Show()
        {
            mBtnImage.color = ColorConstData.BottomSelectColor;
            mBtnText.color = ColorConstData.BottomSelectColor;
            subUI.Show();
        }
    }

    public class MainPanel : BaseCustomPanel
    {
        private Game.ILive mGetAddFriendRequestUpdateLife;
        private byte[] mSendBytes;

        private MainPageData mMainPage;
        private MainPageData mMsgPage;
        private MainPageData mMenuPage;
        private MainPageData mMinePage;

        public MsgPageSubUI msgSubUI { get; private set; }
        public MainPageSubUI mainSubUI { get; private set; }
        public MenuPageSubUI menuSubUI { get; private set; }
        public MinePageSubUI mineSubUI { get; private set; }

        private MainPageData mCurPage;



        public MainPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();

            mainSubUI = new MainPageSubUI(transform.FindObject<Transform>("MainArea"), this);
            msgSubUI = new MsgPageSubUI(transform.FindObject<Transform>("MsgArea"), this);
            menuSubUI = new MenuPageSubUI(transform.FindObject<Transform>("MenuArea"), this);
            mineSubUI = new MinePageSubUI(transform.FindObject<Transform>("MineArea"), this);

            mMainPage = new MainPageData(this, mainSubUI, transform.FindObject<Button>("MainBtn"), PageType.Main);
            mMsgPage = new MainPageData(this, msgSubUI, transform.FindObject<Button>("MsgBtn"), PageType.Msg);
            mMenuPage = new MainPageData(this, menuSubUI, transform.FindObject<Button>("MenuBtn"), PageType.Menu);
            mMinePage = new MainPageData(this, mineSubUI, transform.FindObject<Button>("MineBtn"), PageType.Mine);

            mCurPage = mMainPage;
            mGetAddFriendRequestUpdateLife = GameCenter.Instance.AddUpdate(1f, GetAddFriendRequestCallBack);
        }

        public override void Show()
        {
            base.Show();
            mCurPage.Show();
        }
        //public override void FirstShow()
        //{
        //    base.FirstShow();
        //    mCurPage.Show();
        //}
        private void GetAddFriendRequestCallBack()
        {
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), ChatModule.GetLastAddFriendID().ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetAddFriendRequest, mSendBytes);
        }
        public void ShowPage(PageType pageType)
        {
            if (pageType == mCurPage.pageType) return;
            GetPageData(pageType).Show();
            mCurPage.Hide();
            mCurPage = GetPageData(pageType);
        }
        public MainPageData GetPageData(PageType pageType)
        {
            if (mMainPage.pageType == pageType) return mMainPage;
            if (mMsgPage.pageType == pageType) return mMsgPage;
            if (mMenuPage.pageType == pageType) return mMenuPage;
            if (mMinePage.pageType == pageType) return mMinePage;
            return null;
        }

        public override void OnDestory()
        {
            base.OnDestory();
            GameCenter.Instance.RemoveLife(mGetAddFriendRequestUpdateLife);
        }
    }
}
