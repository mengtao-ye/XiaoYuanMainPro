using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class AddFriendRequestViewPanel : BaseCustomPanel
    {
        public IScrollView scrollView { get; private set; }
        private GameObject mNewFriendScrollView;
        private GameObject mNotFindTip;
        public AddFriendRequestViewPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mNewFriendScrollView = transform.FindObject("NewFriendScrollView");
            mNotFindTip = transform.FindObject("NotFindTip");
            mNewFriendScrollView.SetActiveExtend(false);
            mNotFindTip.SetActiveExtend(false);
            scrollView = transform.FindObject("NewFriendScrollView").AddComponent<RecyclePoolScrollView>();
            scrollView.Init();
            scrollView.SetSpace(10,10,10);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
        }
        public override void Show()
        {
            base.Show();
            bool res = ChatModule.LoadAddFriendList(scrollView);
            mNewFriendScrollView.SetActiveExtend(res);
            mNotFindTip.SetActiveExtend(!res);
        }
    }
}
