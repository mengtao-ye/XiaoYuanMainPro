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
        public MainPageData(MainPanel main, ISubUI subUI, Button button, PageType pageType)
        {
            this.pageType = pageType;
            this.subUI = subUI;
            panel = main;
            button.onClick.AddListener(ClickListener);
            main.AddSubUI(subUI);
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
    }

    public class MainPanel : BaseCustomPanel
    {
        private Game.ILive mGetAddFriendRequestUpdateLife;
        private byte[] mSendBytes;

        public MainPageData mainPage { get; private set; }
        public MainPageData msgPage { get; private set; }
        public MainPageData menuPage { get; private set; }
        public MainPageData minePage { get; private set; }

        private MainPageData mCurPage;
        public MainPanel()
        {
            
        }
        public override void Awake()
        {
            base.Awake();
            mainPage = new MainPageData(this, new MainPageSubUI(transform.FindObject<Transform>("MainArea"), this), transform.FindObject<Button>("MainBtn"), PageType.Main);
            msgPage = new MainPageData(this, new MsgPageSubUI(transform.FindObject<Transform>("MsgArea"), this), transform.FindObject<Button>("MsgBtn"), PageType.Msg);
            menuPage = new MainPageData(this, new MenuPageSubUI(transform.FindObject<Transform>("MenuArea"), this), transform.FindObject<Button>("MenuBtn"), PageType.Menu);
            minePage = new MainPageData(this, new MinePageSubUI(transform.FindObject<Transform>("MineArea"), this), transform.FindObject<Button>("MineBtn"), PageType.Mine);
            mCurPage = mainPage;
            mGetAddFriendRequestUpdateLife = GameCenter.Instance.AddUpdate(1f, GetAddFriendRequestCallBack);
        }


        private void GetAddFriendRequestCallBack()
        {
            mSendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), ChatModule.GetLastAddFriendID().ToBytes());
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetAddFriendRequest, mSendBytes);
        }
        public override void FirstShow()
        {
            base.FirstShow();
            mCurPage.subUI.Show();
        }

        public void ShowPage(PageType pageType)
        {
            if (pageType == mCurPage.pageType) return;
            GetPageData(pageType).subUI.Show();
            mCurPage.subUI.Hide();
            mCurPage = GetPageData(pageType);
        }
        public MainPageData GetPageData(PageType pageType)
        {
            if (mainPage.pageType == pageType) return mainPage;
            if (msgPage.pageType == pageType) return msgPage;
            if (menuPage.pageType == pageType) return menuPage;
            if (minePage.pageType == pageType) return minePage;
            return null;
        }

        public override void OnDestory()
        {
            base.OnDestory();
            GameCenter.Instance.RemoveLife(mGetAddFriendRequestUpdateLife);
        }
    } 
}
